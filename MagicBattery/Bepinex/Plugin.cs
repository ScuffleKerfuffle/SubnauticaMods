using BepInEx;
using HarmonyLib;
using MagicBattery.Items;
using SMLHelper.V2.Utility;
using SubnauticaUtils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Policy;

namespace MagicBattery.Bepinex
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private void Start()
		{
			try
			{
				var magicBattery = new MagicBatteryItem();
				magicBattery.Patch();

				QuickLogger.Info($"Magic Battery Tech type is: {Enum.GetName(typeof(TechType), magicBattery.TechType)} with a numeric value of {(int)magicBattery.TechType}");

				QuickLogger.Info("Patching...");
				var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
				harmony.PatchAll();

				UpdateChargers(magicBattery);

				QuickLogger.Info("Loaded!");
			}
			catch (Exception e)
			{
				QuickLogger.Error(e);
			}
		}

		private void UpdateChargers(MagicBatteryItem battery)
		{
			var type = typeof(BatteryCharger);

			var fieldInfo = type.GetField("compatibleTech", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);

			QuickLogger.Info($"Field Info for compatibleTech was named:{fieldInfo?.Name}");

			if (fieldInfo != null)
			{
				var compatibleTech = (fieldInfo.GetValue(null) as HashSet<TechType>);

				foreach (var tt in compatibleTech)
				{
					QuickLogger.Info($"Existing Tech type is: {Enum.GetName(typeof(TechType), tt)} with a numeric value of {(int)tt}");
				}

				var newHash = new HashSet<TechType>(compatibleTech) { battery.TechType };

				fieldInfo.SetValue(null, newHash);

				QuickLogger.Info($"compatibleTech has [{compatibleTech?.Count}] entries after change.");
			}
		}
	}
}