#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Magazine;
using jhin.Powers;
using jhin.Relics;
using jhin.Utils;

namespace jhin.Actions;

public static class JhinCombatActionUtil
{
    internal static readonly ThrowingPlayerChoiceContext SharedThrowingContext = new();

    public static Task Draw(PlayerChoiceContext choiceContext, Player? player, int amount)
    {
        ArgumentNullException.ThrowIfNull(choiceContext);

        if (player is null || amount <= 0)
        {
            return Task.CompletedTask;
        }

        return CardPileCmd.Draw(choiceContext, amount, player);
    }

    public static Task GainEnergy(Player? player, int amount)
    {
        if (player is null || amount <= 0)
        {
            return Task.CompletedTask;
        }

        return PlayerCmd.GainEnergy(amount, player);
    }

    public static void GainMaxEnergy(Player? player, int amount)
    {
        if (player?.PlayerCombatState is null || amount <= 0)
        {
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            player.PlayerCombatState.AddMaxEnergyToCurrent();
        }
    }

    public static Creature? GetRandomLivingEnemy(Player? player)
    {
        if (player?.Creature?.CombatState is null || player.PlayerRng is null)
        {
            return null;
        }

        List<Creature> enemies = player.Creature.CombatState.HittableEnemies
            .Where(enemy => enemy.IsAlive)
            .ToList();

        return enemies.Count == 0
            ? null
            : player.PlayerRng.Transformations.NextItem(enemies);
    }

    public static ShootCardDamageInput BuildGenericShootDamageInput(
        Player? player,
        Creature? target,
        int displayedBaseDamage,
        int resolvedBaseDamage,
        int baseMarkDamagePerStack,
        int additionalDamagePerMark,
        int flatBonusDamage,
        bool isFlourish)
    {
        if (target is null)
        {
            return default;
        }

        bool hasWhisper = player?.GetRelic<Whisper>() is not null;
        bool hasLastWhisper = player?.GetRelic<LastWhisper>() is not null;
        bool hasFourthBullet = player?.GetRelic<FourthBullet>()?.HasPendingFlourishDamageBonus == true;
        bool hasFineGunOil = player?.GetRelic<FineGunOil>()?.HasPendingShootBonus == true;
        bool isLowHp = DamageCalculationUtil.IsLowHp(target.CurrentHp, target.MaxHp);

        return new ShootCardDamageInput(
            DisplayedBaseDamage: displayedBaseDamage,
            ResolvedBaseDamage: resolvedBaseDamage,
            MarkStacks: ShootAction.GetMarkAmount(target),
            BaseMarkDamagePerStack: baseMarkDamagePerStack,
            AdditionalDamagePerMark: additionalDamagePerMark,
            FlatBonusDamage: flatBonusDamage + (hasFineGunOil ? 4 : 0),
            IsLowHp: isLowHp,
            DamageMultiplier: DamageCalculationUtil.GetShootDamageMultiplier(isFlourish, hasWhisper, hasLastWhisper, hasFourthBullet),
            PostMultiplierFlatBonusDamage: DamageCalculationUtil.GetShootPostMultiplierFlatBonus(isFlourish, isLowHp, hasWhisper, hasLastWhisper),
            IsFlourish: isFlourish);
    }

    public static ShootDamageCalculationResult CalculateGenericShootDamage(Player? player, Creature? target, int baseDamage, bool isFlourish)
    {
        ShootCardDamageInput input = BuildGenericShootDamageInput(
            player,
            target,
            displayedBaseDamage: baseDamage,
            resolvedBaseDamage: baseDamage,
            baseMarkDamagePerStack: MarkPower.DamagePerStack,
            additionalDamagePerMark: 0,
            flatBonusDamage: PerfectTrajectoryPower.GetBonusShootDamage(player?.Creature),
            isFlourish: isFlourish);

        return DamageCalculationUtil.CalculateShootDamage(input);
    }

    public static async Task<bool> ExecuteRandomEnemyShoot(PlayerChoiceContext choiceContext, Player? player, int baseDamage)
    {
        if (player?.Creature is null || !player.Creature.IsAlive || baseDamage <= 0)
        {
            return false;
        }

        Creature? target = GetRandomLivingEnemy(player);
        if (target is null)
        {
            return false;
        }

        ShootResult result = ShootAction.Execute(player);
        if (result == ShootResult.Failed)
        {
            return false;
        }

        bool isFlourish = result == ShootResult.Flourish;
        if (isFlourish)
        {
            ShootAction.TriggerFlourish(choiceContext, player);
        }

        int markAmount = ShootAction.GetMarkAmount(target);

        try
        {
            ShootDamageCalculationResult damageResult = CalculateGenericShootDamage(player, target, baseDamage, isFlourish);
            await CreatureCmd.Damage(choiceContext, target, damageResult.TotalDamage, ValueProp.Move, player.Creature);

            if (markAmount > 0)
            {
                ShootAction.ConsumeMarks(choiceContext, target, player);
            }

            return true;
        }
        finally
        {
            player.GetRelic<FineGunOil>()?.ConsumeShootBonus();

            if (isFlourish)
            {
                player.GetRelic<FourthBullet>()?.ConsumeFlourishDamageBonus();
                FlourishContext.End();
            }
        }
    }

    public static bool HasShotThisTurn(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.UsedShootThisTurn ?? false;
    }

    public static bool HasBulletCount(Player? player, params int[] bulletCounts)
    {
        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(player);
        if (state is null)
        {
            return false;
        }

        return bulletCounts.Contains(state.Bullets);
    }

    public static bool IsFlourishBullet(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.WouldFlourishOnNextShot() ?? false;
    }

    public static void DisableFlourishThisTurn(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.DisableFlourishThisTurn();
    }

    public static bool HasForcedFlourish(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.HasForcedFlourish ?? false;
    }

    public static void ForceNextShotFlourish(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.ForceNextShotFlourish();
    }

    public static async Task ApplyOrStackVulnerable(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        await CommonActions.Apply<VulnerablePower>(SharedThrowingContext, target, null, amount);
    }

    public static async Task ApplyOrStackWeak(Creature? target, int amount, Creature? weakSource = null)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        await CommonActions.Apply<WeakPower>(SharedThrowingContext, target, null, amount);

        StageControlPower.TryApplyMarkOnWeak(target, weakSource);
    }

    public static async Task ApplyOrStackStrength(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        await CommonActions.Apply<StrengthPower>(SharedThrowingContext, target, null, amount);
    }

    public static async Task ApplyOrStackDexterity(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        await CommonActions.Apply<DexterityPower>(SharedThrowingContext, target, null, amount);
    }

    public static bool HasPlayedSkillThisTurn(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.UsedSkillThisTurn ?? false;
    }

    public static bool HasPlayedNonShootAttackThisTurn(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.UsedNonShootAttackThisTurn ?? false;
    }

    public static int GetAttackCardCountThisTurn(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.AttackCardCountThisTurn ?? 0;
    }

    public static void RecordSkillPlayed(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.RecordSkillPlayed();
    }

    public static void RecordNonShootAttackPlayed(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.RecordNonShootAttackPlayed();
    }

    public static void RecordAttackCardPlayed(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.RecordAttackCardPlayed();
    }
}
