using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.Actions;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class ShowPause() : AbstractJhinCard(
    cost: 0,
    type: CardType.Skill,
    rarity: CardRarity.Uncommon,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature?.CombatState is null)
        {
            return;
        }

        int weakAmount = IsUpgraded ? 3 : 2;
        foreach (Creature enemy in Owner.Creature.CombatState.HittableEnemies)
        {
            if (enemy.IsAlive)
            {
                await JhinCombatActionUtil.ApplyOrStackWeak(enemy, weakAmount, Owner.Creature);
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}
