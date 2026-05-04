#nullable enable

using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.Cards;
using jhin.Extensions;
using jhin.Magazine;
using System.Threading.Tasks;

namespace jhin.Relics;

[Pool(typeof(SharedRelicPool))]
public class Whisper : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string PackedIconPath => Placeholders.Role;

    protected override string PackedIconOutlinePath => Placeholders.Role;

    protected override string BigIconPath => Placeholders.Role;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
    ];

    public override Task BeforeCombatStart()
    {
        FlourishEventBus.OnFlourishTriggered += OnFlourishTriggered;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(MegaCrit.Sts2.Core.Rooms.CombatRoom room)
    {
        FlourishEventBus.OnFlourishTriggered -= OnFlourishTriggered;
        return Task.CompletedTask;
    }

    private void OnFlourishTriggered(Player player, JhinMagazineState state)
    {
        if (player == Owner)
        {
            Flash();
        }
    }

    /// <summary>
    /// Multiply flourish attack damage by 1.5.
    /// Applies both during real play (FlourishContext.IsActive) and during drag preview
    /// (card is a shoot card and magazine has exactly 1 bullet remaining).
    /// </summary>
    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!IsWouldFlourish(dealer, cardSource))
        {
            return 1m;
        }

        return 1.5m;
    }

    /// <summary>
    /// Add +6 damage when the target's HP is below 25% during a flourish.
    /// This hook is called during both drag preview and real damage calculation,
    /// so the displayed number and actual damage are always consistent.
    /// Note: additive hooks run before block reduction, which matches the design intent
    /// of a flat bonus on low-HP targets.
    /// </summary>
    public override decimal ModifyDamageAdditive(
        Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target is null || !IsWouldFlourish(dealer, cardSource))
        {
            return 0m;
        }

        if (target.MaxHp <= 0)
        {
            return 0m;
        }

        decimal hpRatio = (decimal)target.CurrentHp / (decimal)target.MaxHp;
        return hpRatio < 0.25m ? 6m : 0m;
    }

    private bool IsWouldFlourish(Creature? dealer, CardModel? cardSource)
    {
        // Real play: flourish context is active
        if (FlourishContext.IsActive && dealer == Owner.Creature)
        {
            return true;
        }

        // Preview: card is a shoot card and magazine has exactly 1 bullet remaining
        if (cardSource is Cards.AbstractShootCard && cardSource.Owner == Owner)
        {
            JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner);
            if (state is not null && state.Bullets == 1)
            {
                return true;
            }
        }

        return false;
    }
}
