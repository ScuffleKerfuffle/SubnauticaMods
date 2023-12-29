using HarmonyLib;
using SubnauticaUtils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagicBattery.Patches
{
	internal static class ChargerPatches
	{
		[HarmonyPatch(typeof(Charger))]
		[HarmonyPatch("OnEquip")]
		internal static class ChargerPatch
		{
			private static void Postfix(Charger __instance, string slot, InventoryItem item)
			{
				Charger.SlotDefinition slotDefinition = __instance.slotDefinitions.FirstOrDefault(sd => sd.id == slot);
				QuickLogger.Info($"__instance {__instance.name} found a battery {slotDefinition.battery?.name} in slot {slotDefinition.id}");

				if (Equals(slotDefinition.battery, null)) return;

				var battery = slotDefinition.battery;
				var pickupable = item.item;

				if (Equals(battery, null) || Equals(pickupable, null)) return;

				switch (__instance)
				{
					case BatteryCharger _:
						Renderer renderer1;
						Renderer renderer2;
						var batteryGameObject = pickupable.gameObject.transform.Find("model/battery_01")?.gameObject
												?? pickupable.gameObject.transform.Find("model/battery_ion")?.gameObject;

						if (Equals(batteryGameObject, null) || !batteryGameObject.TryGetComponent(out renderer1) || !battery.TryGetComponent(out renderer2)) break;

						renderer2.material.CopyPropertiesFromMaterial(renderer1.material);
						break;
					case PowerCellCharger _:
						MeshFilter meshFilter1;
						MeshFilter meshFilter2;
						Renderer renderer3;
						Renderer renderer4;

						var powerCellGameObject = Radical.FindChild(pickupable.gameObject, "engine_power_cell_01")
												  ?? Radical.FindChild(pickupable.gameObject, "engine_power_cell_ion");

						bool mesh1Found = powerCellGameObject.TryGetComponent(out meshFilter1);
						bool mesh2Found = battery.TryGetComponent(out meshFilter2);
						bool renderer3Found = powerCellGameObject.TryGetComponent(out renderer3);
						bool renderer4Found = battery.TryGetComponent(out renderer4);
						bool foundEverything = mesh1Found && mesh2Found && renderer3Found && renderer4Found;

						if (!foundEverything) break;

						meshFilter2.mesh = meshFilter1.mesh;
						renderer4.material.CopyPropertiesFromMaterial(renderer3.material);
						break;
				}
			}
		}
	}
}
