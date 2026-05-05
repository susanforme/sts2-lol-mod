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
public class WhisperVolley() : AbstractShootCard(
    cost: 2,
    rarity: CardRarity.Uncommon,
    target: TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext) || Owner.Creature?.CombatState is null)
        {
            return;
        }

        List<Creature> enemies = Owner.Creature.CombatState.HittableEnemies.Where(e => e.IsAlive).ToList();
        foreach (Creature enemy in enemies)
        {
            await CommonActions.CardAttack(this, enemy, DynamicVars.Damage.IntValue, 1, null, null, null).Execute(choiceContext);
        }

        if (IsFlourishShot)
        {
            int bonusDmg = IsUpgraded ? 4 : 3;
            foreach (Creature enemy in enemies.Where(e => e.IsAlive))
            {
                await CommonActions.CardAttack(this, enemy, bonusDmg, 1, null, null, null).Execute(choiceContext);
            }
        }

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
