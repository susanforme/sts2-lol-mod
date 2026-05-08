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
    private bool _suppressUnifiedDamageModifier;

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
            ShootAction.TriggerFlourish(choiceContext, Owner);
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
        return JhinCombatActionUtil.BuildGenericShootDamageInput(
            Owner,
            target,
            displayedBaseDamage: DynamicVars.Damage.IntValue,
            resolvedBaseDamage: GetResolvedBaseDamage(isFlourish),
            baseMarkDamagePerStack: GetBaseMarkDamagePerStack(isFlourish),
            additionalDamagePerMark: GetAdditionalDamagePerMark(isFlourish),
            flatBonusDamage: GetFlatBonusDamage(target, isFlourish),
            isFlourish: isFlourish);
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
        if (_suppressUnifiedDamageModifier || target is null || dealer != Owner?.Creature || cardSource != this)
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

        _suppressUnifiedDamageModifier = true;
        try
        {
            await CommonActions.CardAttack(this, target, damageResult.TotalDamage, 1, null, null, null).Execute(choiceContext);
        }
        finally
        {
            _suppressUnifiedDamageModifier = false;
        }

        if (markAmount > 0 && ShouldConsumeMarksAfterAttack())
        {
            ShootAction.ConsumeMarks(choiceContext, target, Owner);
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

        _suppressUnifiedDamageModifier = true;
        try
        {
            await CommonActions.CardAttack(this, target, damage, 1, null, null, null).Execute(choiceContext);
        }
        finally
        {
            _suppressUnifiedDamageModifier = false;
        }
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
