#nullable enable

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Commands.Builders;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using jhin.Actions;
using jhin.Magazine;
using jhin.Relics;
using jhin.Utils;

namespace jhin.Cards;

public abstract class AbstractShootCard(int cost, CardRarity rarity, TargetType target)
    : AbstractJhinCard(cost, CardType.Attack, rarity, target)
{
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
            return false;
        }

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
        if (IsFlourishShot)
        {
            FlourishContext.End();
        }
    }

    /// <summary>
    /// Override to add extra effects that trigger only during a flourish shot.
    /// Called automatically from TryShoot after the flourish is confirmed.
    /// </summary>
    protected virtual void OnFlourish()
    {
    }

    protected async Task<AttackCommand> PerformShootAttack(
        PlayerChoiceContext choiceContext,
        Creature target,
        int extraCardDamagePerMark = 0)
    {
        int markAmount = ShootAction.GetMarkAmount(target);
        bool isLowHp = DamageCalculationUtil.IsLowHp(target.CurrentHp, target.MaxHp);
        ShootDamageCalculationResult damageResult = DamageCalculationUtil.CalculateShootDamage(
            baseDamage: DynamicVars.Damage.IntValue,
            isFlourish: IsFlourishShot,
            markStacks: markAmount,
            isLowHp: isLowHp,
            hasWhisper: false,
            extraDamagePerMark: extraCardDamagePerMark);

        int bonusDamage = jhin.Powers.PerfectTrajectoryPower.GetBonusShootDamage(Owner?.Creature);

        AttackCommand command = await CommonActions
            .CardAttack(this, target, damageResult.TotalDamage + bonusDamage, 1, null, null, null)
            .Execute(choiceContext);

        if (markAmount > 0)
        {
            ShootAction.ConsumeMarks(target, Owner);
        }

        return command;
    }
}
