#nullable enable

using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using jhin.Cards;
using jhin.Extensions;
using System.Threading.Tasks;

namespace jhin.Relics;

[Pool(typeof(RelicPools.JhinRelicPool))]
public class ActFourScript : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override string PackedIconPath => "last_whisper.png".ImagePath();
    protected override string PackedIconOutlinePath => "last_whisper.png".ImagePath();
    protected override string BigIconPath => "last_whisper.png".ImagePath();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
    ];

    public override Task BeforeCombatStart()
    {
        Actions.FlourishEventBus.OnFlourishTriggered += OnFlourishTriggered;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(MegaCrit.Sts2.Core.Rooms.CombatRoom room)
    {
        Actions.FlourishEventBus.OnFlourishTriggered -= OnFlourishTriggered;
        return Task.CompletedTask;
    }

    private void OnFlourishTriggered(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, MegaCrit.Sts2.Core.Entities.Players.Player player, Magazine.JhinMagazineState state)
    {
        if (player != Owner) return;
        Flash();
        _ = PlayerCmd.GainEnergy(1m, player);
        _ = Actions.JhinCombatActionUtil.Draw(choiceContext, player, 1);
    }
}
