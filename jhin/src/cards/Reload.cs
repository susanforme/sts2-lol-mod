using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.CardPools;
using jhin.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class Reload() : AbstractJhinCard(
    cost: 0,
    type: CardType.Skill,
    rarity: CardRarity.Uncommon,
    target: TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ReloadAction.Execute(Owner);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
    }
}
