#nullable enable

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using jhin.Actions;

namespace jhin.Powers;

/// <summary>
/// 枪口节奏 / Muzzle Rhythm — Every 4th Attack card played: +1 Strength.
/// Upgraded: also +1 Dexterity.
/// </summary>
public class MuzzleRhythmPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    private int _attackCounter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner?.Player || cardPlay.Card.Type != CardType.Attack)
        {
            return;
        }

        _attackCounter++;
        if (_attackCounter >= 4)
        {
            _attackCounter = 0;
            Flash();
            await JhinCombatActionUtil.ApplyOrStackStrength(Owner, 1);

            if (Amount > 1)
            {
                await JhinCombatActionUtil.ApplyOrStackDexterity(Owner, 1);
            }
        }
    }
}
