using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.Actions;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class CalculatedPlan() : AbstractJhinCard(
    cost: 1,
    type: CardType.Skill,
    rarity: CardRarity.Common,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await JhinCombatActionUtil.Draw(choiceContext, Owner, 2);

        bool gainEnergy = IsUpgraded
            ? JhinCombatActionUtil.HasBulletCount(Owner, 1, 0)
            : JhinCombatActionUtil.HasBulletCount(Owner, 1);

        if (gainEnergy)
        {
            await JhinCombatActionUtil.GainEnergy(Owner, 1);
        }
    }

    protected override void OnUpgrade()
    {
    }
}
