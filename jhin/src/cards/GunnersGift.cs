using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.CardPools;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class GunnersGift() : AbstractShootCard(
    cost: 2,
    rarity: CardRarity.Uncommon,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(13, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!TryShoot(choiceContext))
        {
            return;
        }

        if (cardPlay.Target is null || Owner.Creature is null)
        {
            EndFlourishContext();
            return;
        }

        IEnumerable<DamageResult> results = await CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars.Damage.IntValue, ValueProp.Move, Owner.Creature, this);
        DamageResult? primaryResult = results.FirstOrDefault(r => r.Receiver == cardPlay.Target);
        if (primaryResult?.WasTargetKilled ?? false)
        {
            await CreatureCmd.GainBlock(Owner.Creature, IsUpgraded ? 15 : 12, ValueProp.Move, null, false);
        }

        EndFlourishContext();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
