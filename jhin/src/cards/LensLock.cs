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
using jhin.Powers;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class LensLock() : AbstractShootCard(
    cost: 1,
    rarity: CardRarity.Uncommon,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Mark),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext) || cardPlay.Target is null)
        {
            return;
        }

        int savedMarkAmount = ShootAction.GetMarkAmount(cardPlay.Target);

        await PerformShootAttack(choiceContext, cardPlay.Target);

        if (savedMarkAmount > 0)
        {
            await ApplyMarkAction.Execute(cardPlay.Target, savedMarkAmount);
        }

        int extraMark = IsFlourishShot ? (IsUpgraded ? 4 : 3) : (IsUpgraded ? 3 : 2);
        await ApplyMarkAction.Execute(cardPlay.Target, extraMark);

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
