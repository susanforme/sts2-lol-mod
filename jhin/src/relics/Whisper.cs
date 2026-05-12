#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using jhin.Actions;
using jhin.Cards;
using jhin.Extensions;
using jhin.Magazine;
using System.Threading.Tasks;

namespace jhin.Relics;

[Pool(typeof(SharedRelicPool))]
public class Whisper : AbstractJhinRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<LastWhisper>();

    public override string PackedIconPath => "last_whisper.png".ImagePath();

    protected override string PackedIconOutlinePath => "last_whisper.png".ImagePath();

    protected override string BigIconPath => "last_whisper.png".ImagePath();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
        HoverTipFactory.FromKeyword(JhinKeywords.Reload),
    ];

    protected override void SubscribeEventHandlers()
    {
        ReloadEventBus.OnReloadTriggered += OnReloadTriggered;
    }

    protected override void UnsubscribeEventHandlers()
    {
        ReloadEventBus.OnReloadTriggered -= OnReloadTriggered;
    }

    private void OnReloadTriggered(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state, int bulletsBeforeReload)
    {
        if (player != Owner || bulletsBeforeReload >= state.MaxBullets)
        {
            return;
        }

        Flash();
        _ = JhinCombatActionUtil.Draw(choiceContext, player, 1);
    }
}
