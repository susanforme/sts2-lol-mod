using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using jhin.Magazine;

namespace jhin.Powers;

public class BulletPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => Amount;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Bullet),
    ];

    public void SyncFrom(JhinMagazineState state)
    {
        SetAmount(state.Bullets);
    }
}
