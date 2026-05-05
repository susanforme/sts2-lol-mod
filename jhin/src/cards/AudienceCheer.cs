using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.CardPools;
using jhin.Powers;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class AudienceCheer() : AbstractJhinCard(
    cost: 2,
    type: CardType.Power,
    rarity: CardRarity.Uncommon,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
    ];

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AudienceCheerPower power = (AudienceCheerPower)MegaCrit.Sts2.Core.Models.ModelDb.Power<AudienceCheerPower>().ToMutable();
        power.ApplyInternal(Owner.Creature, IsUpgraded ? 2 : 1, silent: false);
        power.SubscribeEvents();
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
    }
}
