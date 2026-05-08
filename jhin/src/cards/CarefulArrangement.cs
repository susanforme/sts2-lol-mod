using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.CardPools;
using jhin.Powers;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class CarefulArrangement() : AbstractJhinCard(
    cost: 1,
    type: CardType.Power,
    rarity: CardRarity.Uncommon,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<CarefulArrangementPower>(choiceContext, this, IsUpgraded ? 2 : 1);
    }

    protected override void OnUpgrade()
    {
    }
}
