using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.CardPools;

namespace jhin.Cards;

/// <summary>
/// 枪焰擦伤 / Gunflame Graze — 1 cost, shoot attack.
/// 6 dmg + 1 Weak. Flourish: 2 Weak instead. Upgrade: 8 dmg + 1 Weak. Flourish: 3 Weak.
/// </summary>
[Pool(typeof(JhinCardPool))]
public class GunflameGraze() : AbstractShootCard(
    cost: 1,
    rarity: CardRarity.Common,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext))
        {
            return;
        }

        await PerformShootAttack(choiceContext, cardPlay.Target);

        int weakAmount = IsFlourishShot ? (IsUpgraded ? 3 : 2) : 1;
        await JhinCombatActionUtil.ApplyOrStackWeak(cardPlay.Target, weakAmount, Owner.Creature);

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
