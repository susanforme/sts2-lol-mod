using MegaCrit.Sts2.Core.Entities.Cards;
using jhin.Actions;
using jhin.Magazine;

namespace jhin.Cards;

public abstract class AbstractShootCard(int cost, CardRarity rarity, TargetType target)
    : AbstractJhinCard(cost, CardType.Attack, rarity, target)
{
    public virtual bool IsShootCard => true;

    protected override bool IsPlayable => base.IsPlayable && CanShoot();

    public bool CanShoot()
    {
        return JhinMagazineStateRegistry.TryGet(Owner)?.CanShoot() ?? true;
    }

    protected bool TryConsumeBullet()
    {
        return ShootAction.Execute(Owner);
    }
}
