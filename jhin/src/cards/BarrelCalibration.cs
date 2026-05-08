using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.Actions;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class BarrelCalibration() : AbstractJhinCard(
    cost: 1,
    type: CardType.Skill,
    rarity: CardRarity.Uncommon,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Mark),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target is null)
        {
            return;
        }

        bool hadMark = ShootAction.GetMarkAmount(cardPlay.Target) > 0;
        int markAmount = IsUpgraded ? 3 : 2;
        await ApplyMarkAction.Execute(cardPlay.Target, markAmount);

        if (hadMark)
        {
            _ = MegaCrit.Sts2.Core.Commands.PlayerCmd.GainEnergy(IsUpgraded ? 2m : 1m, Owner);
        }
    }

    protected override PileType GetResultPileType() => PileType.Exhaust;

    protected override void OnUpgrade()
    {
    }
}
