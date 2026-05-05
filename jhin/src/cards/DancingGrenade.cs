#nullable enable

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
public class DancingGrenade() : AbstractJhinCard(
    cost: 1,
    type: CardType.Attack,
    rarity: CardRarity.Common,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target is null || Owner.Creature?.CombatState is null)
        {
            return;
        }

        IEnumerable<DamageResult> results = await CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars.Damage.IntValue, ValueProp.Move, Owner.Creature, this);
        DamageResult? primaryResult = results.FirstOrDefault(result => result.Receiver == cardPlay.Target);
        if (primaryResult is null || !primaryResult.WasTargetKilled)
        {
            return;
        }

        List<Creature> livingOtherEnemies = Owner.Creature.CombatState.HittableEnemies
            .Where(enemy => enemy.IsAlive && enemy != cardPlay.Target)
            .ToList();

        Creature? bounceTarget = Owner.PlayerRng.Transformations.NextItem(livingOtherEnemies);
        if (bounceTarget is null)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, bounceTarget, DynamicVars.Damage.IntValue + (IsUpgraded ? 4 : 3), ValueProp.Move, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
