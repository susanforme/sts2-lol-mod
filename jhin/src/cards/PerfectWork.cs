using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.CardPools;
using jhin.Magazine;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class PerfectWork() : AbstractShootCard(
    cost: 2,
    rarity: CardRarity.Rare,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext) || cardPlay.Target is null || Owner.Creature is null) return;

        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner);
        int flourishCount = state?.FlourishCountThisCombat ?? 0;
        int bonusPerFlourish = IsUpgraded ? 5 : 4;
        int totalBonus = flourishCount * bonusPerFlourish;

        await MegaCrit.Sts2.Core.Commands.CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars.Damage.IntValue + totalBonus, ValueProp.Move, Owner.Creature, this);

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
