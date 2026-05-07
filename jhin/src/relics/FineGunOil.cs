#nullable enable

using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
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

    public bool HasPendingShootBonus => _nextShootBoosted;

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

    public void ConsumeShootBonus()
    {
        _nextShootBoosted = false;
    }
}
