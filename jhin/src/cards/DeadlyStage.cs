using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.Actions;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class DeadlyStage() : AbstractJhinCard(
    cost: 1,
    type: CardType.Skill,
    rarity: CardRarity.Rare,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Mark),
        HoverTipFactory.FromKeyword(JhinKeywords.LotusTrap),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature?.CombatState is null) return;

        int amount = IsUpgraded ? 3 : 2;
        foreach (Creature enemy in Owner.Creature.CombatState.HittableEnemies)
        {
            if (enemy.IsAlive)
            {
                await ApplyMarkAction.Execute(enemy, amount);
                await ApplyLotusTrapAction.Execute(enemy, amount);
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}
