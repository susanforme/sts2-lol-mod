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

/// <summary>
/// 低声威胁 / Whispered Threat — 0 cost, non-shoot attack. Exhaust.
/// 3 dmg + 1 Mark. Upgrade: 5 dmg + 2 Mark.
/// </summary>
[Pool(typeof(JhinCardPool))]
public class WhisperedThreat() : AbstractJhinCard(
    cost: 0,
    type: CardType.Attack,
    rarity: CardRarity.Common,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move)];

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

        await CommonActions.CardAttack(this, cardPlay.Target, DynamicVars.Damage.IntValue, 1, null, null, null).Execute(choiceContext);
        await ApplyMarkAction.Execute(cardPlay.Target, IsUpgraded ? 2 : 1);
    }

    protected override PileType GetResultPileType() => PileType.Exhaust;

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
