#nullable enable

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.Magazine;
using jhin.Relics;
using jhin.Utils;

namespace jhin.Cards;

public abstract class AbstractShootCard(int cost, CardRarity rarity, TargetType target)
    : AbstractJhinCard(cost, CardType.Attack, rarity, target)
{
    private bool _didShootThisPlay;

    public virtual bool IsShootCard => true;

    protected override bool IsPlayable => base.IsPlayable && CanShoot();

    /// <summary>
    /// Whether this card's current execution is a flourish shot.
    /// Set by TryShoot() and valid only during OnPlay.
    /// </summary>
    protected bool IsFlourishShot { get; private set; }

    public bool CanShoot()
    {
        return JhinMagazineStateRegistry.TryGet(Owner)?.CanShoot() ?? true;
    }

    /// <summary>
    /// Consumes one bullet, updates flourish state, fires notifications, and
    /// returns whether the shot was fired at all (false = magazine empty).
    /// </summary>
    protected bool TryShoot(PlayerChoiceContext choiceContext)
    {
        ShootResult result = ShootAction.Execute(Owner);
        if (result == ShootResult.Failed)
        {
            IsFlourishShot = false;
            _didShootThisPlay = false;
            return false;
        }

        _didShootThisPlay = true;
        IsFlourishShot = result == ShootResult.Flourish;

        if (IsFlourishShot)
        {
            OnFlourish();
            FlourishContext.Begin();

            JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner);
            if (state is not null && Owner is not null)
            {
                FlourishEventBus.Notify(Owner, state);
            }
        }

        return true;
    }

    /// <summary>
    /// Call this after the flourish attack's damage has been applied to end the flourish context.
    /// </summary>
    protected void EndFlourishContext()
    {
        if (_didShootThisPlay)
        {
            ConsumeOneTimeShootBonuses();
            _didShootThisPlay = false;
        }

        if (IsFlourishShot)
        {
            FlourishContext.End();
        }

        IsFlourishShot = false;
    }

    /// <summary>
    /// Override to add extra effects that trigger only during a flourish shot.
    /// Called automatically from TryShoot after the flourish is confirmed.
    /// </summary>
    protected virtual void OnFlourish()
    {
    }

    protected virtual int GetResolvedBaseDamage(bool isFlourish) => DynamicVars.Damage.IntValue;

    protected virtual int GetBaseMarkDamagePerStack(bool isFlourish) => Powers.MarkPower.DamagePerStack;

    protected virtual int GetAdditionalDamagePerMark(bool isFlourish) => 0;

    protected virtual int GetFlatBonusDamage(Creature target, bool isFlourish) =>
        Powers.PerfectTrajectoryPower.GetBonusShootDamage(Owner?.Creature);

    protected virtual bool ShouldConsumeMarksAfterAttack() => true;

    private bool IsFlourishForDamageCalculation(Creature? dealer, CardModel? cardSource)
    {
        if (dealer == Owner?.Creature && cardSource == this && IsFlourishShot)
        {
            return true;
        }

        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner);
        return state?.WouldFlourishOnNextShot() ?? false;
    }

    private ShootCardDamageInput BuildShootDamageInput(Creature target, bool isFlourish)
    {
        bool hasWhisper = Owner?.GetRelic<Relics.Whisper>() is not null;
        bool hasLastWhisper = Owner?.GetRelic<Relics.LastWhisper>() is not null;
        bool hasFourthBullet = Owner?.GetRelic<Relics.FourthBullet>()?.HasPendingFlourishDamageBonus == true;
        bool hasFineGunOil = Owner?.GetRelic<Relics.FineGunOil>()?.HasPendingShootBonus == true;
        bool isLowHp = DamageCalculationUtil.IsLowHp(target.CurrentHp, target.MaxHp);

        return new ShootCardDamageInput(
            DisplayedBaseDamage: DynamicVars.Damage.IntValue,
            ResolvedBaseDamage: GetResolvedBaseDamage(isFlourish),
            MarkStacks: ShootAction.GetMarkAmount(target),
            BaseMarkDamagePerStack: GetBaseMarkDamagePerStack(isFlourish),
            AdditionalDamagePerMark: GetAdditionalDamagePerMark(isFlourish),
            FlatBonusDamage: GetFlatBonusDamage(target, isFlourish) + (hasFineGunOil ? 4 : 0),
            IsLowHp: isLowHp,
            DamageMultiplier: DamageCalculationUtil.GetShootDamageMultiplier(isFlourish, hasWhisper, hasLastWhisper, hasFourthBullet),
            PostMultiplierFlatBonusDamage: DamageCalculationUtil.GetShootPostMultiplierFlatBonus(isFlourish, isLowHp, hasWhisper, hasLastWhisper),
            IsFlourish: isFlourish);
    }

    protected ShootDamageCalculationResult CalculateShootDamage(Creature target, bool isFlourish)
    {
        return DamageCalculationUtil.CalculateShootDamage(BuildShootDamageInput(target, isFlourish));
    }

    public override decimal ModifyDamageAdditive(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target is null || dealer != Owner?.Creature || cardSource != this)
        {
            return 0m;
        }

        bool isFlourish = IsFlourishForDamageCalculation(dealer, cardSource);
        ShootDamageCalculationResult damageResult = CalculateShootDamage(target, isFlourish);
        return damageResult.TotalDamage - DynamicVars.Damage.IntValue;
    }

    protected async Task PerformShootAttack(
        PlayerChoiceContext choiceContext,
        Creature target)
    {
        int markAmount = ShootAction.GetMarkAmount(target);
        ShootDamageCalculationResult damageResult = CalculateShootDamage(target, IsFlourishShot);

        await MegaCrit.Sts2.Core.Commands.CreatureCmd.Damage(
            choiceContext,
            target,
            damageResult.TotalDamage,
            ValueProp.Move,
            Owner.Creature,
            null);

        if (markAmount > 0 && ShouldConsumeMarksAfterAttack())
        {
            ShootAction.ConsumeMarks(target, Owner);
        }
    }

    protected async Task DealRawBonusDamage(
        PlayerChoiceContext choiceContext,
        Creature? target,
        int damage)
    {
        if (target is null || damage <= 0 || Owner.Creature is null)
        {
            return;
        }

        await MegaCrit.Sts2.Core.Commands.CreatureCmd.Damage(
            choiceContext,
            target,
            damage,
            ValueProp.Move,
            Owner.Creature,
            null);
    }

    private void ConsumeOneTimeShootBonuses()
    {
        Owner?.GetRelic<Relics.FineGunOil>()?.ConsumeShootBonus();

        if (IsFlourishShot)
        {
            Owner?.GetRelic<Relics.FourthBullet>()?.ConsumeFlourishDamageBonus();
        }
    }
}
