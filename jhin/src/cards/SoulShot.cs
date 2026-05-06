using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.CardPools;
using jhin.Magazine;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class SoulShot() : AbstractShootCard(
    cost: 1,
    rarity: CardRarity.Ancient,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
        HoverTipFactory.FromKeyword(JhinKeywords.Mark),
    ];

    protected override int GetAdditionalDamagePerMarkForPreview() => 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext) || cardPlay.Target is null || Owner.Creature is null)
        {
            return;
        }

        int markAmount = ShootAction.GetMarkAmount(cardPlay.Target);

        if (IsFlourishShot)
        {
            int totalDamage = 28 + (markAmount * 4);
            await CreatureCmd.Damage(choiceContext, cardPlay.Target, totalDamage, ValueProp.Move, Owner.Creature, this);
            if (markAmount > 0) ShootAction.ConsumeMarks(cardPlay.Target, Owner);
            ApplyMarkAction.Execute(cardPlay.Target, 4);
            JhinCombatActionUtil.ApplyOrStackVulnerable(cardPlay.Target, 3);
        }
        else
        {
            int totalDamage = DynamicVars.Damage.IntValue + (markAmount * 4);
            await CreatureCmd.Damage(choiceContext, cardPlay.Target, totalDamage, ValueProp.Move, Owner.Creature, this);
            if (markAmount > 0) ShootAction.ConsumeMarks(cardPlay.Target, Owner);
            ApplyMarkAction.Execute(cardPlay.Target, 2);
        }

        if (cardPlay.Target is not null && !cardPlay.Target.IsAlive)
        {
            JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner);
            state?.RestoreBullet();
        }

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
    }
}
