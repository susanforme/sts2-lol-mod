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
public class FullHouseCheer() : AbstractJhinCard(
    cost: 3,
    type: CardType.Attack,
    rarity: CardRarity.Rare,
    target: TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature?.CombatState is null) return;

        int kills = 0;
        foreach (Creature enemy in Owner.Creature.CombatState.HittableEnemies)
        {
            if (!enemy.IsAlive) continue;

            IEnumerable<DamageResult> results = await CreatureCmd.Damage(choiceContext, enemy, DynamicVars.Damage.IntValue, ValueProp.Move, Owner.Creature, this);
            DamageResult? result = results.FirstOrDefault(r => r.Receiver == enemy);
            if (result?.WasTargetKilled ?? false)
            {
                kills++;
            }
        }

        for (int i = 0; i < kills; i++)
        {
            _ = PlayerCmd.GainEnergy(1m, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
