using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.Actions;
using jhin.CardPools;
using jhin.Powers;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class FinalActReload() : AbstractJhinCard(
    cost: 0,
    type: CardType.Skill,
    rarity: CardRarity.Ancient,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Reload),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ReloadAction.Execute(Owner);
        await JhinCombatActionUtil.Draw(choiceContext, Owner, 3);

        await CommonActions.ApplySelf<ForcedFlourishPower>(choiceContext, this, 1);
    }

    protected override void OnUpgrade()
    {
    }
}
