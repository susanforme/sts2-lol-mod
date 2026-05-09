using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.CardPools;
using jhin.Extensions;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class Strike() : AbstractJhinCard(
    cost: 1,
    type: CardType.Attack,
    rarity: CardRarity.Basic,
    target: TargetType.AnyEnemy)
{
    protected override string PortraitResourcePath => "Card/JHIN-STRIKE.png".ImagePath();

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target is null)
        {
            return;
        }

        await CommonActions.CardAttack(this, cardPlay.Target, DynamicVars.Damage.IntValue, 1, null, null, null).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
