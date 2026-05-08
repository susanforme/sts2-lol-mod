using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class FourthAct() : AbstractShootCard(
    cost: 2,
    rarity: CardRarity.Uncommon,
    target: TargetType.AnyEnemy)
{
    protected override bool IsPlayable => base.IsPlayable && JhinCombatActionUtil.IsFlourishBullet(Owner);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(24, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext))
        {
            return;
        }

        await PerformShootAttack(choiceContext, cardPlay.Target);

        if (IsFlourishShot)
        {
            await JhinCombatActionUtil.ApplyOrStackVulnerable(cardPlay.Target, IsUpgraded ? 4 : 3);
        }

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
    }
}
