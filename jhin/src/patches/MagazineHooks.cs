#nullable enable

using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Entities.Powers;
using jhin.Actions;
using jhin.Cards;
using jhin.Magazine;
using jhin.Powers;

namespace jhin.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.PopulateCombatState))]
public static class PlayerPopulateCombatStatePatch
{
    public static void Postfix(Player __instance)
    {
        if (!JhinMagazineStateRegistry.IsJhin(__instance))
        {
            return;
        }

        JhinMagazineState state = JhinMagazineStateRegistry.GetOrCreate(__instance);
        state.InitializeCombat();

        _ = ApplyBulletPowerAsync(__instance, state);
    }

    private static async Task ApplyBulletPowerAsync(Player player, JhinMagazineState state)
    {
        BulletPower? bulletPower = await CommonActions.Apply<BulletPower>(
            new ThrowingPlayerChoiceContext(), player.Creature, null, state.Bullets, silent: true);
        if (bulletPower is not null)
        {
            state.AttachPower(bulletPower);
        }
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.AfterCombatEnd))]
public static class PlayerAfterCombatEndPatch
{
    public static void Postfix(Player __instance)
    {
        if (!JhinMagazineStateRegistry.IsJhin(__instance))
        {
            return;
        }

        JhinMagazineStateRegistry.TryGet(__instance)?.DetachPower();
        JhinMagazineStateRegistry.Clear(__instance);
        FlourishContext.End();
        FlourishEventBus.ClearListeners();
        BulletEmptyEventBus.ClearListeners();
        ReloadEventBus.ClearListeners();
        LotusTrapPower.ClearPendingWeak();
    }
}

[HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.ResetEnergy))]
public static class PlayerCombatStateResetEnergyPatch
{
    public static void Postfix(PlayerCombatState __instance)
    {
        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(__instance);
        if (state is null)
        {
            return;
        }

        state.StartTurn();

        // Check for powers that trigger at turn start
        // Use the registry to find the Player associated with this combat state
        MegaCrit.Sts2.Core.Entities.Players.Player? player = JhinMagazineStateRegistry.GetPlayerFromCombatState(__instance);
        if (player?.Creature is null || !player.Creature.IsAlive)
        {
            return;
        }

        ActorsInstinctPower? instinctPower = player.Creature.GetPower<ActorsInstinctPower>();
        instinctPower?.OnTurnStart();

        AestheticOfFourPower? fourPower = player.Creature.GetPower<AestheticOfFourPower>();
        fourPower?.OnTurnStart(player);

        CarefulArrangementPower? arrangePower = player.Creature.GetPower<CarefulArrangementPower>();
        arrangePower?.OnTurnStart(player);

        FinalActForeshadowingPower? foreshadowPower = player.Creature.GetPower<FinalActForeshadowingPower>();
        foreshadowPower?.OnTurnStart(player);

        FatemakerPower? fatemakerPower = player.Creature.GetPower<FatemakerPower>();
        fatemakerPower?.OnTurnStart(player);

        PerfectCrimePower? perfectCrimePower = player.Creature.GetPower<PerfectCrimePower>();
        perfectCrimePower?.OnTurnStart();

        FinalActArtPower? finalActArtPower = player.Creature.GetPower<FinalActArtPower>();
        finalActArtPower?.OnTurnStart(player);
    }
}

[HarmonyPatch]
public static class ShootCardCanPlayReasonPatch
{
    private static System.Reflection.MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(CardModel), nameof(CardModel.CanPlay), [typeof(UnplayableReason).MakeByRefType(), typeof(AbstractModel).MakeByRefType()]);
    }

    public static void Postfix(CardModel __instance, ref bool __result, ref UnplayableReason reason, ref AbstractModel preventer)
    {
        if (!__instance.IsMutable)
        {
            return;
        }

        if (!__result && reason == UnplayableReason.EnergyCostTooHigh && JhinMagazineStateRegistry.IsJhin(__instance.Owner))
        {
            preventer = __instance;
            return;
        }

        if (__instance is not AbstractShootCard shootCard || shootCard.CanShoot())
        {
            return;
        }

        if (!__result && reason == UnplayableReason.EnergyCostTooHigh)
        {
            return;
        }

        __result = false;
        reason = UnplayableReason.BlockedByCardLogic;
        preventer = __instance;
    }
}

[HarmonyPatch]
public static class ShootCardUnplayableDialoguePatch
{
    private const string CardsTable = "cards";
    private const string NoEnergyKey = "JHIN-SHOOT_DISABLED_NO_ENERGY";
    private const string NoBulletsKey = "JHIN-SHOOT_DISABLED_NO_BULLETS";

    private static System.Reflection.MethodBase TargetMethod()
    {
        return AccessTools.Method("MegaCrit.Sts2.Core.Entities.Cards.UnplayableReasonExtensions:GetPlayerDialogueLine");
    }

    public static void Postfix(UnplayableReason reason, AbstractModel preventer, ref LocString __result)
    {
        if (preventer is not CardModel card || !card.IsMutable || !JhinMagazineStateRegistry.IsJhin(card.Owner))
        {
            return;
        }

        if (reason == UnplayableReason.EnergyCostTooHigh)
        {
            LocString? noEnergy = LocString.GetIfExists(CardsTable, NoEnergyKey);
            if (noEnergy is not null)
            {
                __result = noEnergy;
            }

            return;
        }

        if (reason == UnplayableReason.BlockedByCardLogic && card is AbstractShootCard shootCard && !shootCard.CanShoot())
        {
            LocString? noBullets = LocString.GetIfExists(CardsTable, NoBulletsKey);
            if (noBullets is not null)
            {
                __result = noBullets;
            }
        }
    }
}

public static class JhinCardDisabledPromptPatch
{
    private const string CardsTable = "cards";
    private const string NoEnergyKey = "JHIN-SHOOT_DISABLED_NO_ENERGY";
    private const string NoBulletsKey = "JHIN-SHOOT_DISABLED_NO_BULLETS";

    private static LocString? GetDisabledPrompt(CardModel card)
    {
        if (!card.IsMutable)
        {
            return null;
        }

        if (!JhinMagazineStateRegistry.IsJhin(card.Owner))
        {
            return null;
        }

        UnplayableReason reason;
        AbstractModel? preventer;
        if (card.CanPlay(out reason, out preventer))
        {
            return null;
        }

        if (reason == UnplayableReason.EnergyCostTooHigh)
        {
            return LocString.GetIfExists(CardsTable, NoEnergyKey);
        }

        if (card is AbstractShootCard shootCard && !shootCard.CanShoot())
        {
            return LocString.GetIfExists(CardsTable, NoBulletsKey);
        }

        return null;
    }

    [HarmonyPatch]
    public static class SelectionScreenPromptGetterPatch
    {
        private static System.Reflection.MethodBase TargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(CardModel), "SelectionScreenPrompt");
        }

        public static void Postfix(CardModel __instance, ref LocString __result)
        {
            LocString? prompt = GetDisabledPrompt(__instance);
            if (prompt is not null)
            {
                __result = prompt;
            }
        }
    }

    [HarmonyPatch]
    public static class EnergyHoverTipGetterPatch
    {
        private static System.Reflection.MethodBase TargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(CardModel), "EnergyHoverTip");
        }

        public static void Postfix(CardModel __instance, ref IHoverTip __result)
        {
            LocString? prompt = GetDisabledPrompt(__instance);
            if (prompt is not null)
            {
                __result = new HoverTip(__instance.TitleLocString, prompt, __instance.EnergyIcon);
            }
        }
    }
}

public static class JhinCannotPlayFtuePatch
{
    private const string CardsTable = "cards";
    private const string NoEnergyKey = "JHIN-SHOOT_DISABLED_NO_ENERGY";
    private static CardModel? _currentFtueCard;

    [HarmonyPatch]
    public static class CannotPlayThisCardFtueCheckPatch
    {
        private static System.Reflection.MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(NCardPlay), "CannotPlayThisCardFtueCheck", [typeof(CardModel)]);
        }

        public static void Prefix(CardModel card)
        {
            if (!card.IsMutable)
            {
                _currentFtueCard = null;
                return;
            }

            if (!JhinMagazineStateRegistry.IsJhin(card.Owner))
            {
                _currentFtueCard = null;
                return;
            }

            UnplayableReason reason;
            AbstractModel? preventer;
            bool canPlay = card.CanPlay(out reason, out preventer);
            if (!canPlay && reason == UnplayableReason.EnergyCostTooHigh)
            {
                _currentFtueCard = card;
            }
            else
            {
                _currentFtueCard = null;
            }
        }

    }

    [HarmonyPatch(typeof(NCannotPlayCardFtue), nameof(NCannotPlayCardFtue._Ready))]
    public static class CannotPlayFtueReadyPatch
    {
        public static void Postfix(NCannotPlayCardFtue __instance)
        {
            try
            {
                CardModel? card = _currentFtueCard;
                if (card is null)
                {
                    return;
                }

                LocString? prompt = LocString.GetIfExists(CardsTable, NoEnergyKey);
                if (prompt is null)
                {
                    return;
                }

                string text = prompt.GetFormattedText();
                if (string.IsNullOrWhiteSpace(text))
                {
                    return;
                }

                object? description = AccessTools.Field(typeof(NCannotPlayCardFtue), "_description")?.GetValue(__instance);
                if (description is not null)
                {
                    AccessTools.Property(description.GetType(), "Text")?.SetValue(description, text);
                }
            }
            finally
            {
                _currentFtueCard = null;
            }
        }
    }
}
