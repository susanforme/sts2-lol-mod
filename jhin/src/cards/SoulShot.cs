using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.CardPools;
using jhin.Magazine;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class SoulShot() : AbstractShootCard(
    cost: 1,
    rarity: CardRarity.Ancient,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(JhinKeywords.Flourish),
        HoverTipFactory.FromKeyword(JhinKeywords.Mark),
    ];

    protected override int GetResolvedBaseDamage(bool isFlourish) => isFlourish ? 28 : DynamicVars.Damage.IntValue;

    protected override int GetBaseMarkDamagePerStack(bool isFlourish) => 0;

    protected override int GetAdditionalDamagePerMark(bool isFlourish) => 4;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext) || cardPlay.Target is null)
        {
            return;
        }

        await PerformShootAttack(choiceContext, cardPlay.Target);

        await ApplyMarkAction.Execute(cardPlay.Target, IsFlourishShot ? 4 : 2);

        if (IsFlourishShot)
        {
            await JhinCombatActionUtil.ApplyOrStackVulnerable(cardPlay.Target, 3);
        }

        if (cardPlay.Target is not null && !cardPlay.Target.IsAlive)
        {
            JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner);
            state?.RestoreBullet();
        }

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
    }
}
