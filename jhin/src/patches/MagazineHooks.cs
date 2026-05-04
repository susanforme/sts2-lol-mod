#nullable enable

using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
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

        BulletPower bulletPower = (BulletPower)ModelDb.Power<BulletPower>().ToMutable();
        bulletPower.ApplyInternal(__instance.Creature, state.Bullets, silent: true);
        state.AttachPower(bulletPower);
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
        if (preventer is not CardModel card || !JhinMagazineStateRegistry.IsJhin(card.Owner))
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
            if (!JhinMagazineStateRegistry.IsJhin(card.Owner))
            {
                MainFile.Logger.Info($"FTUE check ignored for non-Jhin card {card.Id}");
                _currentFtueCard = null;
                return;
            }

            UnplayableReason reason;
            AbstractModel? preventer;
            bool canPlay = card.CanPlay(out reason, out preventer);
            string preventerId = preventer?.Id.ToString() ?? "<null>";
            MainFile.Logger.Info($"FTUE check card={card.Id} canPlay={canPlay} reason={reason} preventer={preventerId}");
            if (!canPlay && reason == UnplayableReason.EnergyCostTooHigh)
            {
                MainFile.Logger.Info($"FTUE tracking Jhin energy prompt for {card.Id}");
                _currentFtueCard = card;
            }
            else
            {
                MainFile.Logger.Info($"FTUE not tracking card={card.Id}");
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
                    MainFile.Logger.Info("FTUE ready with no tracked Jhin card");
                    return;
                }

                LocString? prompt = LocString.GetIfExists(CardsTable, NoEnergyKey);
                if (prompt is null)
                {
                    MainFile.Logger.Info("FTUE ready but localized no-energy prompt was missing");
                    return;
                }

                string text = prompt.GetFormattedText();
                if (string.IsNullOrWhiteSpace(text))
                {
                    MainFile.Logger.Info("FTUE ready but localized no-energy prompt was empty");
                    return;
                }

                object? description = AccessTools.Field(typeof(NCannotPlayCardFtue), "_description")?.GetValue(__instance);
                if (description is not null)
                {
                    string? before = AccessTools.Property(description.GetType(), "Text")?.GetValue(description) as string;
                    MainFile.Logger.Info($"FTUE overriding description for {card.Id}: before='{before}' after='{text}'");
                    AccessTools.Property(description.GetType(), "Text")?.SetValue(description, text);
                    string? after = AccessTools.Property(description.GetType(), "Text")?.GetValue(description) as string;
                    MainFile.Logger.Info($"FTUE description after set for {card.Id}: '{after}'");
                }
                else
                {
                    MainFile.Logger.Info($"FTUE description node missing for {card.Id}");
                }
            }
            finally
            {
                _currentFtueCard = null;
            }
        }
    }
}
