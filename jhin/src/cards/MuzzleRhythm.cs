using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Combat;
using jhin.CardPools;
using jhin.Powers;

namespace jhin.Cards;

/// <summary>
/// 枪口节奏 / Muzzle Rhythm — 1 cost, common power.
/// Every 4th Attack card played: +1 Strength. Upgrade: also +1 Dexterity.
/// </summary>
[Pool(typeof(JhinCardPool))]
public class MuzzleRhythm() : AbstractJhinCard(
    cost: 1,
    type: CardType.Power,
    rarity: CardRarity.Common,
    target: TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<MuzzleRhythmPower>(choiceContext, this, IsUpgraded ? 2 : 1);
    }

    protected override void OnUpgrade()
    {
    }
}
