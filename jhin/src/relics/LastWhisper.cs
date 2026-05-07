#nullable enable

using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;
using jhin.Actions;
using jhin.Cards;
using jhin.Extensions;
using jhin.Magazine;
using System.Threading.Tasks;

namespace jhin.Relics;

[Pool(typeof(SharedRelicPool))]
public class LastWhisper : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string PackedIconPath => "last_whisper.png".ImagePath();

    protected override string PackedIconOutlinePath => "last_whisper.png".ImagePath();

    protected override string BigIconPath => "last_whisper.png".ImagePath();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
        HoverTipFactory.FromKeyword(JhinKeywords.Reload),
    ];

    public override Task BeforeCombatStart()
    {
        ReloadEventBus.OnReloadTriggered += OnReloadTriggered;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(MegaCrit.Sts2.Core.Rooms.CombatRoom room)
    {
        ReloadEventBus.OnReloadTriggered -= OnReloadTriggered;
        return Task.CompletedTask;
    }

    private void OnReloadTriggered(Player player, JhinMagazineState state, int bulletsBeforeReload)
    {
        if (player != Owner || bulletsBeforeReload >= state.MaxBullets)
        {
            return;
        }

        Flash();
        _ = JhinCombatActionUtil.Draw(null!, player, 2);
    }
}
