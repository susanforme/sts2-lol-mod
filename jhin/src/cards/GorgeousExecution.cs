using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class GorgeousExecution() : AbstractJhinCard(
    cost: 3,
    type: CardType.Attack,
    rarity: CardRarity.Uncommon,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target is null || Owner.Creature is null)
        {
            return;
        }

        IEnumerable<DamageResult> results = await CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars.Damage.IntValue, ValueProp.Move, Owner.Creature, this);
        DamageResult? primaryResult = results.FirstOrDefault(r => r.Receiver == cardPlay.Target);
        if (primaryResult?.WasTargetKilled ?? false)
        {
            _ = PlayerCmd.GainEnergy(2m, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
    }

    protected override PileType GetResultPileType() => PileType.Exhaust;
}
