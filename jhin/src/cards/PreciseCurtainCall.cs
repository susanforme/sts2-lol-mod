using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.Actions;
using jhin.CardPools;
using jhin.Magazine;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class PreciseCurtainCall() : AbstractJhinCard(
    cost: 0,
    type: CardType.Skill,
    rarity: CardRarity.Rare,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner);
        if (state is not null)
        {
            state.SetBulletsToZero();
        }

        int energyAmount = IsUpgraded ? 3 : 2;
        _ = PlayerCmd.GainEnergy(energyAmount, Owner);

        if (IsUpgraded)
        {
            await JhinCombatActionUtil.Draw(choiceContext, Owner, 1);
        }
    }

    protected override void OnUpgrade()
    {
    }

    protected override PileType GetResultPileType() => PileType.Exhaust;
}
