using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.CardPools;
using jhin.Extensions;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class WhisperBurst() : AbstractShootCard(
    cost: 0,
    rarity: CardRarity.Common,
    target: TargetType.AnyEnemy)
{
    protected override string PortraitResourcePath => "Card/JHIN-WHISPER_BURST.png".ImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
    ];

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
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}
