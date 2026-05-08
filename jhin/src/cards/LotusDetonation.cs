using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.CardPools;
using jhin.Powers;

namespace jhin.Cards;

[Pool(typeof(JhinCardPool))]
public class LotusDetonation() : AbstractJhinCard(
    cost: 1,
    type: CardType.Attack,
    rarity: CardRarity.Uncommon,
    target: TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target is null || Owner.Creature?.CombatState is null)
        {
            return;
        }

        LotusTrapPower? trapPower = cardPlay.Target.GetPower<LotusTrapPower>();
        if (trapPower is null)
        {
            return;
        }

        int stacks = trapPower.Amount;
        int perStackDamage = IsUpgraded ? 8 : 6;
        int totalDamage = stacks * perStackDamage;

        await PowerCmd.Remove(trapPower);
        await CreatureCmd.Damage(choiceContext, cardPlay.Target, totalDamage, ValueProp.Move, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
