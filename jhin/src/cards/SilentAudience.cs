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
public class SilentAudience() : AbstractJhinCard(
    cost: 2,
    type: CardType.Skill,
    rarity: CardRarity.Uncommon,
    target: TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        if (Owner.Creature?.CombatState is null)
        {
            return;
        }

        int weakAmount = IsUpgraded ? 2 : 1;
        foreach (Creature enemy in Owner.Creature.CombatState.HittableEnemies)
        {
            if (enemy.IsAlive)
            {
                await JhinCombatActionUtil.ApplyOrStackWeak(enemy, weakAmount);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
