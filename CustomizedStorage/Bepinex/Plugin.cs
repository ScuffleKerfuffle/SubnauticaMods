using BepInEx;
using CustomizedStorage.Config;
using HarmonyLib;
using SMLHelper.V2.Handlers;
using SubnauticaUtils;
using System;

namespace CustomizedStorage.Bepinex
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private void Start()
		{
			try
			{
				QuickLogger.Info("Registing menu options with SML Helper...");
				ModConfig.Instance = OptionsPanelHandler.Main.RegisterModOptions<ModConfig>();

				QuickLogger.Info("Patching...");
				var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
				harmony.PatchAll();

				QuickLogger.Info("Loaded!");
			}
			catch (Exception e)
			{
				QuickLogger.Error(e);
			}
		}
	}
}