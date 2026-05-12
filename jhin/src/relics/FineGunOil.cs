#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using jhin.Cards;
using jhin.Extensions;
using jhin.Magazine;
using System.Threading.Tasks;

namespace jhin.Relics;

[Pool(typeof(RelicPools.JhinRelicPool))]
public class FineGunOil : AbstractJhinRelic
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

    protected override Task OnBeforeCombatStart()
    {
        _nextShootBoosted = false;
        return Task.CompletedTask;
    }

    protected override void SubscribeEventHandlers()
    {
        Actions.ReloadEventBus.OnReloadTriggered += OnReloadTriggered;
    }

    protected override void UnsubscribeEventHandlers()
    {
        Actions.ReloadEventBus.OnReloadTriggered -= OnReloadTriggered;
    }

    private void OnReloadTriggered(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state, int bulletsBeforeReload)
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
