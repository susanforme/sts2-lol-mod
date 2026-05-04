using MegaCrit.Sts2.Core.Entities.Cards;

namespace jhin.Cards;

public abstract class AbstractShootCard(int cost, CardRarity rarity, TargetType target)
    : AbstractJhinCard(cost, CardType.Attack, rarity, target)
{
    public virtual bool IsShootCard => true;

    public virtual void OnShoot()
    {
    }
}
