using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.Actions;
using jhin.CardPools;
using jhin.Powers;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class PerfectCrime() : AbstractJhinCard(
    cost: 2,
    type: CardType.Power,
    rarity: CardRarity.Rare,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        PerfectCrimePower power = (PerfectCrimePower)MegaCrit.Sts2.Core.Models.ModelDb.Power<PerfectCrimePower>().ToMutable();
        power.ApplyInternal(Owner.Creature, IsUpgraded ? 2 : 1, silent: false);

        int initialStr = IsUpgraded ? 5 : 4;
        JhinCombatActionUtil.ApplyOrStackStrength(Owner.Creature, initialStr);

        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
    }
}
