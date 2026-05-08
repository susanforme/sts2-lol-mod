using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class DeathMonologue() : AbstractJhinCard(
    cost: 3,
    type: CardType.Attack,
    rarity: CardRarity.Rare,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target is null || Owner.Creature is null) return;

        IEnumerable<DamageResult> results = await CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars.Damage.IntValue, ValueProp.Move, Owner.Creature, this);

        if (cardPlay.Target.IsAlive && cardPlay.Target.MaxHp > 0 && (decimal)cardPlay.Target.CurrentHp / cardPlay.Target.MaxHp < 0.3m)
        {
            await CreatureCmd.Damage(choiceContext, cardPlay.Target, IsUpgraded ? 50 : 40, ValueProp.Move, Owner.Creature, this);
        }

        DamageResult? primaryResult = results.FirstOrDefault(r => r.Receiver == cardPlay.Target);
        if (primaryResult?.WasTargetKilled ?? false)
        {
            await JhinCombatActionUtil.ApplyOrStackStrength(Owner.Creature, 2);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
}
