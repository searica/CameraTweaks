// Ignore Spelling: CameraTweaks Jotunn

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using CameraTweaks.Configs;
using CameraTweaks.Extensions;
using System.Reflection;
using UnityEngine;

namespace CameraTweaks
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    internal class CameraTweaks : BaseUnityPlugin
    {
        internal const string Author = "Searica";
        public const string PluginName = "CameraTweaks";
        public const string PluginGUID = $"{Author}.Valheim.{PluginName}";
        public const string PluginVersion = "1.0.3";

        internal static ConfigEntry<float> MaxDistance;
        internal static ConfigEntry<float> MaxDistanceBoat;
        private static bool UpdateCameraDistanceValues;

        private static readonly string MainSection = ConfigManager.SetStringPriority("Global", 1);
        private static readonly string CameraSection = "Camera";

        public void Awake()
        {
            Log.Init(Logger);

            ConfigManager.Init(PluginGUID, Config, false);

            Log.Verbosity = ConfigManager.BindConfig(
                MainSection,
                "Verbosity",
                LogLevel.Low,
                "Low will log basic information about the mod. Medium will log information that " +
                "is useful for troubleshooting. High will log a lot of information, do not set " +
                "it to this without good reason as it will slow Down your game."
            );

            MaxDistance = ConfigManager.BindConfig(
                CameraSection,
                "MaxDistance",
                6f,
                "Maximum distance you can zoom out to. Vanilla default is 6.",
                new AcceptableValueRange<float>(6f, 20f)
            );

            MaxDistanceBoat = ConfigManager.BindConfig(
                CameraSection,
                "MaxDistanceBoat",
                12f,
                "Maximum distance you can zoom out to while in a boat. Vanilla default is 6.",
                new AcceptableValueRange<float>(6f, 20f)
            );

            MaxDistance.SettingChanged += delegate
            {
                if (!UpdateCameraDistanceValues)
                {
                    UpdateCameraDistanceValues = true;
                }
            };

            MaxDistance.SettingChanged += delegate
            {
                if (!UpdateCameraDistanceValues)
                {
                    UpdateCameraDistanceValues = true;
                }
            };

            ConfigManager.Save();
            ConfigManager.SaveOnConfigSet(true);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
            Game.isModded = true;

            ConfigManager.SetupWatcher();
            ConfigManager.CheckForConfigManager();
            ConfigManager.OnConfigFileReloaded += UpdateCameraDistance;
            ConfigManager.OnConfigWindowClosed += UpdateCameraDistance;
        }

        public void OnDestroy()
        {
            ConfigManager.Save();
        }

        private static void UpdateCameraDistance()
        {
            if (GameCamera.instance != null && UpdateCameraDistanceValues)
            {
                GameCamera.instance.m_maxDistance = MaxDistance.Value;
                GameCamera.instance.m_maxDistanceBoat = MaxDistanceBoat.Value;
                GameCamera.instance.UpdateCamera(Time.unscaledDeltaTime);
                UpdateCameraDistanceValues = false;
            }
        }
    }

    [HarmonyPatch(typeof(GameCamera))]
    internal class GameCameraPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameCamera.Awake))]
        private static void GameCameraAwakePostfix(GameCamera __instance)
        {
            __instance.m_maxDistance = CameraTweaks.MaxDistance.Value;
            __instance.m_maxDistanceBoat = CameraTweaks.MaxDistanceBoat.Value;
        }
    }

    /// <summary>
    ///     Log level to control output to BepInEx log
    /// </summary>
    internal enum LogLevel
    {
        Low = 0,
        Medium = 1,
        High = 2,
    }

    /// <summary>
    ///     Helper class for properly logging from static contexts.
    /// </summary>
    internal static class Log
    {
        #region Verbosity

        internal static ConfigEntry<LogLevel> Verbosity { get; set; }
        internal static LogLevel VerbosityLevel => Verbosity.Value;

        #endregion Verbosity

        internal static ManualLogSource _logSource;

        internal static void Init(ManualLogSource logSource)
        {
            _logSource = logSource;
        }

        internal static void LogDebug(object data) => _logSource.LogDebug(data);

        internal static void LogError(object data) => _logSource.LogError(data);

        internal static void LogFatal(object data) => _logSource.LogFatal(data);

        internal static void LogInfo(object data, LogLevel level = LogLevel.Low)
        {
            if (VerbosityLevel >= level)
            {
                _logSource.LogInfo(data);
            }
        }

        internal static void LogMessage(object data) => _logSource.LogMessage(data);

        internal static void LogWarning(object data) => _logSource.LogWarning(data);

        internal static void LogGameObject(GameObject prefab, bool includeChildren = false)
        {
            LogInfo("***** " + prefab.name + " *****");
            foreach (Component compo in prefab.GetComponents<Component>())
            {
                LogComponent(compo);
            }

            if (!includeChildren) { return; }

            LogInfo("***** " + prefab.name + " (children) *****");
            foreach (Transform child in prefab.transform)
            {
                LogInfo($" - {child.gameObject.name}");
                foreach (Component compo in child.gameObject.GetComponents<Component>())
                {
                    LogComponent(compo);
                }
            }
        }

        internal static void LogComponent(Component compo)
        {
            LogInfo($"--- {compo.GetType().Name}: {compo.name} ---");

            PropertyInfo[] properties = compo.GetType().GetProperties(ReflectionUtils.AllBindings);
            foreach (var property in properties)
            {
                LogInfo($" - {property.Name} = {property.GetValue(compo)}");
            }

            FieldInfo[] fields = compo.GetType().GetFields(ReflectionUtils.AllBindings);
            foreach (var field in fields)
            {
                LogInfo($" - {field.Name} = {field.GetValue(compo)}");
            }
        }
    }
}