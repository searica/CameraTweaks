// Ignore Spelling: CameraTweaks Jotunn

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using System;
using Logging;

namespace CameraTweaks
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    internal sealed class CameraTweaks : BaseUnityPlugin
    {
        internal const string Author = "Searica";
        public const string PluginName = "CameraTweaks";
        public const string PluginGUID = $"{Author}.Valheim.{PluginName}";
        public const string PluginVersion = "1.3.1";

        internal static ConfigEntry<float> MaxDistance;
        internal static ConfigEntry<float> MaxDistanceBoat;
        internal static ConfigEntry<float> CameraFoV;
        internal static ConfigEntry<bool> AlwaysFaceCamera;
        private static bool ShouldUpdateCamera;

        private static readonly string MainSection = "Global";
        private static readonly string CameraSection = "Camera";

        public void Awake()
        {
            Log.Init(Logger);

            Config.Init(PluginGUID, false);

            Log.Verbosity = Config.BindConfigInOrder(
                MainSection,
                "Verbosity",
                Log.LogLevel.Low,
                "Low will log basic information about the mod. Medium will log information that " +
                "is useful for troubleshooting. High will log a lot of information, do not set " +
                "it to this without good reason as it will slow Down your game.",
                synced: false
            );

            CameraFoV = Config.BindConfigInOrder(
                CameraSection,
                "Field of View",
                65f,
                "Camera field of view in degrees. Vanilla default is 65.",
                new AcceptableValueRange<float>(60f, 120f),
                synced: false
            );

            MaxDistance = Config.BindConfigInOrder(
                CameraSection,
                "Max Distance",
                6f,
                "Maximum distance you can zoom out to while on foot. Vanilla default is 6.",
                new AcceptableValueRange<float>(6f, 20f),
                synced: false
            );

            MaxDistanceBoat = Config.BindConfigInOrder(
                CameraSection,
                "Max Distance (Boat)",
                12f,
                "Maximum distance you can zoom out to while in a boat. Vanilla default is 6.",
                new AcceptableValueRange<float>(6f, 20f),
                synced: false
            );

            AlwaysFaceCamera = Config.BindConfigInOrder(
                CameraSection,
                "Always Face Camera",
                false,
                "Controls whether the player character will always face in the direction of the crosshairs.",
                synced: false
            );


            MaxDistance.SettingChanged += SetShouldUpdateCamera;
            MaxDistance.SettingChanged += SetShouldUpdateCamera;
            CameraFoV.SettingChanged += SetShouldUpdateCamera;

            Config.Save();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
            Game.isModded = true;

            Config.SetupWatcher();
            Config.CheckForConfigManager();
            ConfigFileManager.OnConfigFileReloaded += UpdateCameraSettings;
            ConfigFileManager.OnConfigWindowClosed += UpdateCameraSettings;
        }

        public void OnDestroy()
        {
            Config.Save();
        }

        private void SetShouldUpdateCamera(object obj, EventArgs e)
        {
            ShouldUpdateCamera |= !ShouldUpdateCamera;
        }

        private void UpdateCameraSettings()
        {
            if (ShouldUpdateCamera && GameCamera.instance != null)
            {
                GameCamera.instance.m_maxDistance = MaxDistance.Value;
                GameCamera.instance.m_maxDistanceBoat = MaxDistanceBoat.Value;
                GameCamera.instance.m_fov = CameraFoV.Value;
                GameCamera.instance.UpdateCamera(Time.unscaledDeltaTime);
                ShouldUpdateCamera = false;
                Config.Save();
            }
        }
    }


    [HarmonyPatch]
    internal static class CameraPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameCamera), nameof(GameCamera.Awake))]
        private static void GameCameraAwakePostfix(GameCamera __instance)
        {
            __instance.m_maxDistance = CameraTweaks.MaxDistance.Value;
            __instance.m_maxDistanceBoat = CameraTweaks.MaxDistanceBoat.Value;
            __instance.m_fov = CameraTweaks.CameraFoV.Value;
        }

        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)]
        [HarmonyPatch(typeof(GameCamera), nameof(GameCamera.UpdateCamera))]
        public static void GameCamera_UpdateCamera_MaxDistBoat_Prefix(GameCamera __instance, ref float __state)
        {
            if (Player.m_localPlayer && Player.m_localPlayer.IsAttachedToShip())
            {
                __state = __instance.m_maxDistance;
                __instance.m_maxDistance = __instance.m_maxDistanceBoat;
                return;
            }
            __state = -1f;
        }

        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        [HarmonyPatch(typeof(GameCamera), nameof(GameCamera.UpdateCamera))]
        public static void GameCamera_UpdateCamera_MaxDistBoat_Postfix(GameCamera __instance, float __state)
        {
            if (__state != -1f)
            {
                __instance.m_maxDistance = __state;
            } 
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), nameof(Player.AlwaysRotateCamera))]
        public static bool AlwaysRotateCameraPrefix(Player __instance, ref bool __result)
        {
            if (CameraTweaks.AlwaysFaceCamera.Value && __instance.GetCurrentWeapon() != null && !__instance.InEmote())
            {
                __result = true;
                return false;
            }

            __result = false;
            return true;
        }
    }
}