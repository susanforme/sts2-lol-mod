using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using jhin.CardPools;
using jhin.Extensions;

namespace jhin.Cards;

public abstract class AbstractJhinCard(int cost, CardType type, CardRarity rarity, TargetType target)
    : CustomCardModel(cost, type, rarity, target)
{
    protected virtual string PortraitResourcePath => Placeholders.Card;

    public override string CustomPortraitPath => PortraitResourcePath;
    public override string PortraitPath => PortraitResourcePath;
    public override string BetaPortraitPath => PortraitResourcePath;
}
