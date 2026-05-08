using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.Actions;
using jhin.CardPools;

namespace jhin.Cards;

/// <summary>
/// 观察弱点 / Observe Weakness — 1 cost, skill.
/// Apply 2 Mark. If target below 50% HP, draw 1. Upgrade: 3 Mark.
/// </summary>
[Pool(typeof(JhinCardPool))]
public class ObserveWeakness() : AbstractJhinCard(
    cost: 1,
    type: CardType.Skill,
    rarity: CardRarity.Common,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Mark),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target is null)
        {
            return;
        }

        await ApplyMarkAction.Execute(cardPlay.Target, IsUpgraded ? 3 : 2);

        if (cardPlay.Target.MaxHp > 0 && (decimal)cardPlay.Target.CurrentHp / cardPlay.Target.MaxHp < 0.5m)
        {
            await JhinCombatActionUtil.Draw(choiceContext, Owner, 1);
        }
    }

    protected override void OnUpgrade()
    {
    }
}
