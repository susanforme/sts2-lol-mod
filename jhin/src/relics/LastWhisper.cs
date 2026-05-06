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
using jhin.Utils;
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

    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        return DamageCalculationUtil.GetLastWhisperFlourishMultiplier(
            hasLastWhisper: true,
            isFlourish: IsWouldFlourish(dealer, cardSource));
    }

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

        return DamageCalculationUtil.GetLastWhisperLowHpBonusDamage(
            hasLastWhisper: true,
            isFlourish: true,
            isLowHp: DamageCalculationUtil.IsLowHp(target.CurrentHp, target.MaxHp));
    }

    private bool IsWouldFlourish(Creature? dealer, CardModel? cardSource)
    {
        if (FlourishContext.IsActive && dealer == Owner.Creature)
        {
            return true;
        }

        if (cardSource is AbstractShootCard && cardSource.Owner == Owner)
        {
            JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner);
            if (state is not null && state.WouldFlourishOnNextShot())
            {
                return true;
            }
        }

        return false;
    }
}
