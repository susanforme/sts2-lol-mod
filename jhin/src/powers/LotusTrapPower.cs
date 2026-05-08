#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Utils;

namespace jhin.Powers;

public class LotusTrapPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    private static readonly Dictionary<Creature, int> PendingWeakByCreature = [];

    public override string CustomPackedIconPath => "res://JhinMod/Images/captive _audience.png";
    public override string CustomBigIconPath => "res://JhinMod/Images/captive _audience.png";
    public override string CustomBigBetaIconPath => "res://JhinMod/Images/captive _audience.png";

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => Amount;

    public static int DamagePerStack => ConstantUtil.LotusTrapDamagePerStack;
    public static int WeakPerStack => ConstantUtil.LotusTrapWeakPerStack;
    public static int DeathExplosionDamage => ConstantUtil.LotusTrapDeathExplosionDamage;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("lotusTrapDamagePerStack", DamagePerStack);
        description.Add("lotusTrapWeakPerStack", WeakPerStack);
        description.Add("lotusTrapDeathExplosionDamage", DeathExplosionDamage);
    }

    public void AddStacks(int amount)
    {
        SetAmount(Amount + amount, silent: false);
    }

    public async Task TriggerAfterOwnerAttack(PlayerChoiceContext choiceContext)
    {
        Creature? target = Owner;
        CombatState? combatState = target?.CombatState;
        if (target is null || combatState is null || Amount <= 0 || !target.IsAlive)
        {
            return;
        }

        int stacks = Amount;
        Flash();
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(
            choiceContext,
            target,
            stacks * DamagePerStack,
            ValueProp.Move,
            Applier ?? target);
        DamageResult damageResult = damageResults.First(result => result.Receiver == target);

        if (damageResult.WasTargetKilled)
        {
            Creature dealer = ResolveDamageDealer(combatState, target);
            await TriggerDeathExplosion(choiceContext, combatState, dealer, stacks);
            return;
        }

        if (target.IsAlive && target.CanReceivePowers)
        {
            QueuePendingWeak(target, stacks * WeakPerStack);
        }

        target.RemovePowerInternal(this);
    }

    public static void ApplyPendingWeak(CombatSide endedSide)
    {
        if (PendingWeakByCreature.Count == 0)
        {
            return;
        }

        foreach ((Creature target, int amount) in PendingWeakByCreature.ToArray())
        {
            PendingWeakByCreature.Remove(target);
            if (target.Side != endedSide || target.CombatState is null || !target.CombatState.ContainsCreature(target) || !target.IsAlive || !target.CanReceivePowers)
            {
                continue;
            }

            _ = ApplyOrStackWeak(target, amount);
        }
    }

    public static void ClearPendingWeak()
    {
        PendingWeakByCreature.Clear();
    }

    private static void QueuePendingWeak(Creature target, int amount)
    {
        if (!PendingWeakByCreature.TryAdd(target, amount))
        {
            PendingWeakByCreature[target] += amount;
        }
    }

    private static async Task ApplyOrStackWeak(Creature target, int amount)
    {
        await CommonActions.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), target, null, amount);

        StageControlPower.TryApplyMarkOnWeak(target, target);
    }

    private static Creature ResolveDamageDealer(CombatState combatState, Creature fallback)
    {
        return combatState.PlayerCreatures.FirstOrDefault(creature => creature.IsAlive) ?? fallback;
    }

    private static async Task TriggerDeathExplosion(PlayerChoiceContext choiceContext, CombatState combatState, Creature dealer, int stacks)
    {
        List<Creature> enemies = combatState.HittableEnemies
            .Where(enemy => enemy.IsAlive)
            .ToList();

        if (enemies.Count == 0)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, enemies, stacks * DeathExplosionDamage, ValueProp.Move, dealer);
    }
}
