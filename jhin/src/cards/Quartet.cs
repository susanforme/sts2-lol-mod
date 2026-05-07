using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class Quartet() : AbstractShootCard(
    cost: 2,
    rarity: CardRarity.Rare,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move)];

    protected override int GetResolvedBaseDamage(bool isFlourish) =>
        isFlourish ? (IsUpgraded ? 8 : 7) : DynamicVars.Damage.IntValue;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext) || Owner.Creature?.CombatState is null || Owner.PlayerRng is null) return;

        List<Creature> aliveEnemies = Owner.Creature.CombatState.HittableEnemies.Where(e => e.IsAlive).ToList();

        for (int i = 0; i < 4; i++)
        {
            if (aliveEnemies.Count == 0) break;

            Creature target = Owner.PlayerRng.Transformations.NextItem(aliveEnemies) ?? aliveEnemies[0];
            await PerformShootAttack(choiceContext, target);

            aliveEnemies = Owner.Creature.CombatState.HittableEnemies.Where(e => e.IsAlive).ToList();
        }

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}
