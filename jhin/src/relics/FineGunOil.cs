#nullable enable

using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Cards;
using jhin.Extensions;
using jhin.Magazine;
using System.Threading.Tasks;

namespace jhin.Relics;

[Pool(typeof(RelicPools.JhinRelicPool))]
public class FineGunOil : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override string PackedIconPath => "last_whisper.png".ImagePath();
    protected override string PackedIconOutlinePath => "last_whisper.png".ImagePath();
    protected override string BigIconPath => "last_whisper.png".ImagePath();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Reload),
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
    ];

    private bool _nextShootBoosted;

    public override Task BeforeCombatStart()
    {
        _nextShootBoosted = false;
        Actions.ReloadEventBus.OnReloadTriggered += OnReloadTriggered;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(MegaCrit.Sts2.Core.Rooms.CombatRoom room)
    {
        Actions.ReloadEventBus.OnReloadTriggered -= OnReloadTriggered;
        return Task.CompletedTask;
    }

    private void OnReloadTriggered(Player player, JhinMagazineState state, int bulletsBeforeReload)
    {
        if (player == Owner)
        {
            _nextShootBoosted = true;
            Flash();
        }
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (_nextShootBoosted && dealer == Owner?.Creature && cardSource is AbstractShootCard)
        {
            _nextShootBoosted = false;
            return 4m;
        }
        return 0m;
    }
}
