using BepInEx;
using CustomizedStorage.Config;
using HarmonyLib;
using SMLHelper.V2.Handlers;
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
				Logger.LogInfo($"{PluginInfo.PLUGIN_NAME} registing menu options with SML Helper...");

				ModConfig.Instance = OptionsPanelHandler.Main.RegisterModOptions<ModConfig>();
				
				Logger.LogInfo($"{PluginInfo.PLUGIN_NAME} patching...");
				var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
				harmony.PatchAll();

				Logger.LogInfo($"{PluginInfo.PLUGIN_NAME} loaded!");
			}
			catch (Exception e)
			{
				Logger.LogError(e);
			}
		}
	}
}