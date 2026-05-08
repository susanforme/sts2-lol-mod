#nullable enable

using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using jhin.Cards;
using jhin.Extensions;
using jhin.Powers;

namespace jhin.Potions;

[Pool(typeof(PotionPools.JhinPotionPool))]
public class FlourishPotion : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.Self;

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
    ];

    public override string? CustomPackedImagePath => Placeholders.Role;

    public override string? CustomPackedOutlinePath => Placeholders.Role;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await CommonActions.Apply<ForcedFlourishPower>(choiceContext, Owner.Creature, null, 1);
        Actions.JhinCombatActionUtil.ForceNextShotFlourish(Owner);
    }
}
