#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using jhin.Actions;
using jhin.Cards;
using jhin.Extensions;

namespace jhin.Relics;

[Pool(typeof(RelicPools.JhinRelicPool))]
public class JhinMask : AbstractJhinRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public override string PackedIconPath => "last_whisper.png".ImagePath();

    protected override string PackedIconOutlinePath => "last_whisper.png".ImagePath();

    protected override string BigIconPath => "last_whisper.png".ImagePath();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Mark),
    ];

    protected override Task OnBeforeCombatStart()
    {
        var combatState = Owner.Creature.CombatState;
        if (combatState is null)
        {
            return Task.CompletedTask;
        }

        foreach (var enemy in combatState.HittableEnemies.Where(enemy => enemy.IsAlive))
        {
            _ = ApplyMarkAction.Execute(enemy, 1);
        }

        Flash();
        return Task.CompletedTask;
    }
}
