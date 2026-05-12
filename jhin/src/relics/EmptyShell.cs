#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.Cards;
using jhin.Extensions;
using jhin.Magazine;

namespace jhin.Relics;

[Pool(typeof(RelicPools.JhinRelicPool))]
public class EmptyShell : AbstractJhinRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public override string PackedIconPath => "last_whisper.png".ImagePath();

    protected override string PackedIconOutlinePath => "last_whisper.png".ImagePath();

    protected override string BigIconPath => "last_whisper.png".ImagePath();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
    ];

    protected override void SubscribeEventHandlers()
    {
        BulletEmptyEventBus.OnBulletEmptied += OnBulletEmptied;
    }

    protected override void UnsubscribeEventHandlers()
    {
        BulletEmptyEventBus.OnBulletEmptied -= OnBulletEmptied;
    }

    private void OnBulletEmptied(MegaCrit.Sts2.Core.Entities.Players.Player player, JhinMagazineState state)
    {
        if (player != Owner || !player.Creature.IsAlive)
        {
            return;
        }

        Flash();
        TaskHelper.RunSafely(CreatureCmd.GainBlock(player.Creature, 5m, ValueProp.Unpowered, null));
    }
}
