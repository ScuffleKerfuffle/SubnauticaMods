using BepInEx;
using HarmonyLib;
using SMLHelper.V2.Handlers;
using SubnauticaUtils;
using System;

namespace MagicBattery.Bepinex
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private void Start()
		{
			try
			{
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