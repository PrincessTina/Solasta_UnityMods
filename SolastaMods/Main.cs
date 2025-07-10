using System;
using HarmonyLib;
using JetBrains.Annotations;
using UnityModManagerNet;

namespace SolastaMods
{
    static class Main
    {
        internal static bool Load([NotNull] UnityModManager.ModEntry modEntry)
        {
            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                FileLog.Reset();
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                modEntry.Logger.Error(ex.ToString());
                throw;
            }

            return true;
        }
    }

    /* GAMEPLAY IMPROVEMENTS: */

    /*
     * This patch allows to hide facial tattoos, that are forced by default for Sorcerer subclasses.
     */
    [HarmonyPatch(typeof(CharacterBuildingManager), nameof(CharacterBuildingManager.BuildMorphotypeOptionsList))]
    static class Patch_HideSorcererMarks
    {
        static void Prefix(ref CharacterSubclassDefinition subClass)
        {
            subClass = null;
        }
    }

    /*
     * This patch allows to always show extended information about items, spells, etc., as if "Alt" key was physically pressed,
     * while preserving the ability to view shortened information.
     */
    [HarmonyPatch(typeof(TooltipPanel), nameof(TooltipPanel.SetupFeatures))]
    static class Patch_AlwaysAlt
    {
        static void Prefix(ref TooltipDefinitions.Scope scope)
        {
            switch (scope)
            {
                case TooltipDefinitions.Scope.Simplified:
                    scope = TooltipDefinitions.Scope.Detailed;
                    break;
                case TooltipDefinitions.Scope.Detailed:
                    scope = TooltipDefinitions.Scope.Simplified;
                    break;
            }
        }
    }

    /*
     * This patch allows to hide helmets for characters in heavy armor.
     */
    [HarmonyPatch(typeof(ItemPresentation), nameof(ItemPresentation.GetBodyPartBehaviours))]
    static class Patch_HideHelmets
    {
        static void Postfix(ItemPresentation __instance, ref GraphicsCharacterDefinitions.BodyPartBehaviour[] __result)
        {
            if (__instance.HasCrownVariationMask)
            {
                return;
            }

            __result[0] = GraphicsCharacterDefinitions.BodyPartBehaviour.Shape; // Head / Helmet
            __result[1] = GraphicsCharacterDefinitions.BodyPartBehaviour.Shape; // Hair
        }
    }

    /* BYPASSING RULES: */

    /*
     * This patch allows all characters to wear metal armor (to boost your druid or bard, for example).
     * Not cleanest solution, as it basically removes "metal" property for all items, that supposed to be made of metal.
     */
    [HarmonyPatch(typeof(RulesetCharacterHero), nameof(RulesetCharacterHero.IsProficientWithItem))]
    static class Patch_AllowMetalArmor
    {
        static void Prefix(ref ItemDefinition itemDefinition)
        {
            itemDefinition.ItemTags.Remove(TagsDefinitions.ItemTagMetal);
        }
    }

    /*
     * This patch allows to preserve druid's AC and Constitution, while in wild shape form.
     */
    [HarmonyPatch(typeof(RulesetImplementationManager), nameof(RulesetImplementationManager.ApplyEffectForm))]
    static class Patch_SyncWildShapeAC
    {
        static void Prefix(EffectForm effectForm, ref RulesetImplementationDefinitions.ApplyFormsParams formsParams)
        {
            if (effectForm.FormType == EffectForm.EffectFormType.ShapeChange && formsParams.targetCharacter != null)
            {
                formsParams.targetSubstitute.ArmorClass = formsParams.targetCharacter.TryGetAttributeValue("ArmorClass");
                formsParams.targetSubstitute.AbilityScores[2] = formsParams.targetCharacter.TryGetAttributeValue("Constitution");
            }
        }
    }

    /*
     * This patch allows to max faction relationship. 
     * It will be triggered, when any faction operation affecting its relationship should happen. For example, when you sell
     * items to faction, you typically increase relationship with it. However, if this code is on, your faction relationship
     * will be set to 100 instead of normal increase.
     */
    [HarmonyPatch(typeof(GameFactionManager), nameof(GameFactionManager.ExecuteFactionOperation))]
    static class Patch_MaxFactionRelationship
    {
        static void Prefix(ref FactionDefinition.FactionOperation factionOperation, ref int value)
        {
            return; // remove this to turn on the code below

            factionOperation = FactionDefinition.FactionOperation.SetValue;
            value = 100;
        }
    }

    /*
     * This code allows to gain all achievements on game start, if turned on
     */
    [HarmonyPatch(typeof(MainMenuScreen), nameof(MainMenuScreen.Load))]
    static class Patch_GainAllAchievements
    {
        static void Prefix()
        {
            return; // remove this to turn on the code below

            // All achievement names were copied from GamingPlatformDefinitions class's const string properties
            string[] AllAchievements = new string[132]
            {
                "ACH_IMMORTAL", "ACH_IRONMAN", "ACH_REVIVE", "ACH_CRITKILL", "ACH_RANGER", "ACH_ROGUE", "ACH_WIZARD",
                "ACH_PALADIN", "ACH_CLERIC", "ACH_FIGHTER", "ACH_ONEALIVE", "ACH_FRIENDLYFIRE", "ACH_ALMOSTDEAD",
                "ACH_PLAYERFALL", "ACH_REROLLSTATS", "ACH_CLASSICPARTY", "ACH_NOCASTERPARTY", "ACH_ARWIN", "ACH_LEGENDARYQUEST",
                "ACH_ROBAR", "ACH_LISBATH", "ACH_DALIAT", "ACH_BERYL", "ACH_PRINCIPALITY", "ACH_TOWER", "ACH_ARCANEUM",
                "ACH_CIRCLE", "ACH_ANTIQUARIAN", "ACH_SCAVENGERS", "ACH_KILLAKSHA", "ACH_KILLRAZVAN", "ACH_TALKAKSHA",
                "ACH_KILLMARDRACHT", "ACH_TALKMARDRACHT", "ACH_DEPUTY", "ACH_SENIORDEPUTY", "ACH_HENRIKDEAD", "ACH_CROWN",
                "ACH_SORAKREAL", "ACH_GEMNECRO", "ACH_GEMABJU", "ACH_GEMCONJU", "ACH_GEMEVO", "ACH_INQUISITOR", "ACH_AERELAI",
                "ACH_FINISHGAME", "ACH_MONSTERFALL", "ACH_OVERWATCH", "ACH_SUPEREFFECTIVE", "ACH_TAKECRIT", "ACH_WEALTHY",
                "ACH_IDENTIFY", "ACH_ATTUNED", "ACH_POISONKIT", "ACH_HERBORISTKIT", "ACH_ENCHANTMENTKIT", "ACH_SCROLLKIT",
                "ACH_BESTIARY", "ACH_ARISTOCRAT", "ACH_PHILOSOPHER", "ACH_LOWLIFE", "ACH_ACOLYTE", "ACH_SELLSWORD",
                "ACH_ACADEMIC", "ACH_LAWKEEPER", "ACH_SPY", "ACH_FIREKILL", "ACH_ICEKILL", "ACH_THUNDERKILL",
                "ACH_LIGHTNINGKILL", "ACH_NECROTICKILL", "ACH_RADIANTKILL", "ACH_BLUDGEONKILL", "ACH_SLASHKILL",
                "ACH_PIERCEKILL", "ACH_PSYCHICKILL", "ACH_POISONKILL", "ACH_FORCEKILL", "ACH_ACIDKILL", "ACH_CRIT",
                "ACH_CREATEDUNGEON", "ACH_PLAYDUNGEON", "ACH_BARBARIAN", "ACH_DRUID", "ACH_VALLEYSTART", "ACH_REDEEMEREND",
                "ACH_ORENETISEND", "ACH_FUSIONEND", "ACH_MARINEND", "ACH_ANFARELEND", "ACH_EMPTYEND", "ACH_FORGEEND",
                "ACH_KILLSITENERO", "ACH_FOUNDMARIN", "ACH_KILLMUMMY", "ACH_DESTROYCOMPLEX", "ACH_ROCKSCREAM", "ACH_WANDERER",
                "ACH_MULTIPLAYER", "ACH_MONK", "ACH_WARLOCK", "ACH_BARD", "ACH_DEMON", "ACH_PARDON", "ACH_CONDEMN",
                "ACH_HECTOR", "ACH_EXO", "ACH_TRAITOR", "ACH_WHITECITY", "ACH_TELESCOPE", "ACH_ISRID", "ACH_MARDUK",
                "ACH_SCEPTERGIANT", "ACH_SCEPTERELF", "ACH_SAVEMOTHER", "ACH_ASHDOWN", "ACH_BLUNT", "ACH_ESTORGATH",
                "ACH_FAKEELF", "ACH_GREYBEAR", "ACH_GILMAR", "ACH_GUARDIAN", "ACH_SAVEROYAL", "ACH_SESSROTH",
                "ACH_SACRIFICEPARTY", "ACH_SACRIFICENPC", "ACH_SOUTH", "ACH_CENTRAL", "ACH_WEST", "ACH_EAST", "ACH_NORTH",
                "ACH_PARTY"
            };

            foreach (string Achievement in AllAchievements)
            {
                GamingPlatform.UnlockAchievement(Achievement);
            }
        }
    }
}