using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.CardPools;
using jhin.Extensions;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class CommonShot() : AbstractShootCard(
    cost: 1,
    rarity: CardRarity.Basic,
    target: TargetType.AnyEnemy), ITranscendenceCard
{
    protected override string PortraitResourcePath => "Card/JHIN-COMMON_SHOT.png".ImagePath();

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
    ];

    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<SoulShot>();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext))
        {
            return;
        }

        await PerformShootAttack(choiceContext, cardPlay.Target);
        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
