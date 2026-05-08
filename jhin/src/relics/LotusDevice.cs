#nullable enable

using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using jhin.Actions;
using jhin.Cards;
using jhin.Extensions;
using System.Threading.Tasks;

namespace jhin.Relics;

[Pool(typeof(RelicPools.JhinRelicPool))]
public class LotusDevice : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override string PackedIconPath => "last_whisper.png".ImagePath();
    protected override string PackedIconOutlinePath => "last_whisper.png".ImagePath();
    protected override string BigIconPath => "last_whisper.png".ImagePath();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.LotusTrap),
    ];

    public override Task BeforeCombatStart()
    {
        if (Owner?.Creature?.CombatState is null) return Task.CompletedTask;

        System.Collections.Generic.List<Creature> enemies = Owner.Creature.CombatState.HittableEnemies
            .Where(e => e.IsAlive)
            .ToList();

        if (enemies.Count == 0) return Task.CompletedTask;

        Creature target = Owner.PlayerRng?.Transformations.NextItem(enemies) ?? enemies[0];
        _ = ApplyLotusTrapAction.Execute(target, 2);
        Flash();
        return Task.CompletedTask;
    }
}
