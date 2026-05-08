using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.CardPools;
using jhin.Powers;

namespace jhin.Cards;

/// <summary>
/// 血色舞台 / Bloody Stage — 1 cost, common power.
/// When enemy first drops below 50%: apply 2 Mark. Upgrade: 3 Mark.
/// </summary>
[Pool(typeof(JhinCardPool))]
public class BloodyStage() : AbstractJhinCard(
    cost: 1,
    type: CardType.Power,
    rarity: CardRarity.Common,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Mark),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<BloodyStageP>(choiceContext, this, IsUpgraded ? 2 : 1);
    }

    protected override void OnUpgrade()
    {
    }
}
