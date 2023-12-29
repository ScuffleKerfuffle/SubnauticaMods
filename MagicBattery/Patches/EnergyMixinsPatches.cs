using HarmonyLib;
using MagicBattery.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EnergyMixin;
using static SMLHelper.V2.Assets.CustomFabricator;

namespace MagicBattery.Patches
{
	internal class EnergyMixinsPatches
	{
		[HarmonyPatch(typeof(EnergyMixin))]
		[HarmonyPatch("NotifyHasBattery")]
		internal static class EnergyMixinNotifyBatteryPatches
		{
			internal static void Postfix(EnergyMixin __instance, InventoryItem item)
			{
				//Note: This is for Powercells that are visible, like the Cyclops or whatever.
				var itemInSlot = item?.item?.GetTechType();

				if (!itemInSlot.HasValue || itemInSlot.Value == TechType.None) return;

				__instance.batteryModels[0].model.SetActive(true);
			}
		}

		[HarmonyPatch(typeof(EnergyMixin))]
		[HarmonyPatch("Awake")]
		internal static class EnergyMixinAwakePatches
		{
			internal static void Prefix(EnergyMixin __instance) 
			{
				// This is necessary to allow the new batteries to be compatible with tools and vehicles

				// Battery replacement not allowed - No need to make changes
				if (!__instance.allowBatteryReplacement) return; 

				var compatibleBatteries = __instance.compatibleBatteries;

				var batteryModels = new List<BatteryModels>(__instance.batteryModels ?? new BatteryModels[0]);

				GameObject batteryModel = null;
				GameObject powerCellModel = null;
				GameObject ionBatteryModel = null;
				GameObject ionPowerCellModel = null;

				List<TechType> existingTechtypes = new List<TechType>();
				List<GameObject> existingModels = new List<GameObject>();

				//First check for models already setup
				for (int i=0; i < batteryModels.Count; i++)
				{
					BatteryModels model = batteryModels[i];

					switch (model.techType) 
					{
						case TechType.Battery:
							batteryModel = model.model;
							break;
						case TechType.PowerCell:
							powerCellModel = model.model;
							break;
						case TechType.PrecursorIonBattery:
							ionBatteryModel = model.model;
							break;
						case TechType.PrecursorIonPowerCell:
							ionPowerCellModel = model.model;
							break;
					}
					existingTechtypes.Add(batteryModels[i].techType);
					existingModels.Add(batteryModels[i].model);
				}

				//Then check for models not already setup.
				foreach (Renderer renderer in __instance.gameObject.GetComponentsInChildren<Renderer>(true))
				{
					if (renderer.gameObject.GetComponentInParent<Battery>(true) != null)
						continue;

					switch (renderer?.material?.mainTexture?.name)
					{
						case "battery_01":
							batteryModel = batteryModel ?? (batteryModel = renderer.gameObject);
							break;
						case "battery_ion":
							ionBatteryModel = ionBatteryModel ?? (ionBatteryModel = renderer.gameObject);
							break;
						case "power_cell_01":
							powerCellModel = powerCellModel ?? (powerCellModel = renderer.gameObject);
							break;
						case "engine_power_cell_ion":
							ionPowerCellModel = ionPowerCellModel ?? (ionPowerCellModel = renderer.gameObject);
							break;
					}
				}

				//Add missing models that were found or create new ones if possible.
				if (batteryModel != null && !existingTechtypes.Contains(TechType.Battery))
				{
					batteryModels.Add(new BatteryModels() { model = batteryModel, techType = TechType.Battery });
					existingTechtypes.Add(TechType.Battery);
					existingModels.Add(batteryModel);
				}

				//Add missing models that were found or create new ones if possible.
				if (!existingTechtypes.Contains(TechType.PrecursorIonBattery))
				{
					if (ionBatteryModel != null)
					{
						batteryModels.Add(new BatteryModels() { model = ionBatteryModel, techType = TechType.PrecursorIonBattery });
						existingTechtypes.Add(TechType.PrecursorIonBattery);
						existingModels.Add(ionBatteryModel);
					}
					else if (batteryModel != null)
					{

						GameObject ionBatteryPrefab = Resources.Load<GameObject>("worldentities/tools/precursorionbattery");
						ionBatteryModel = GameObject.Instantiate(batteryModel, batteryModel.transform.parent);
						ionBatteryModel.name = "precursorIonBatteryModel";


						Material ionBatteryMaterial = ionBatteryPrefab?.GetComponentInChildren<Renderer>()?.material;
						if (ionBatteryMaterial != null)
						{
							ionBatteryModel.GetComponentInChildren<Renderer>().material = new Material(ionBatteryMaterial);
							batteryModels.Add(new BatteryModels() { model = ionBatteryModel, techType = TechType.PrecursorIonBattery });
							existingTechtypes.Add(TechType.PrecursorIonBattery);
							existingModels.Add(ionBatteryModel);
						}
					}
				}

				//Add missing models that were found or create new ones if possible.
				if (powerCellModel != null && !existingTechtypes.Contains(TechType.PowerCell))
				{
					batteryModels.Add(new BatteryModels() { model = powerCellModel, techType = TechType.PowerCell });
					existingTechtypes.Add(TechType.PowerCell);
					existingModels.Add(powerCellModel);
				}

				//Add missing models that were found or create new ones if possible.
				if (!existingTechtypes.Contains(TechType.PrecursorIonPowerCell))
				{
					if (ionPowerCellModel != null)
					{
						batteryModels.Add(new BatteryModels() { model = ionPowerCellModel, techType = TechType.PrecursorIonPowerCell });
						existingTechtypes.Add(TechType.PrecursorIonPowerCell);
						existingModels.Add(ionPowerCellModel);
					}
					else if (powerCellModel != null)
					{

						GameObject ionPowerCellPrefab = Resources.Load<GameObject>("worldentities/tools/precursorionpowercell");
						ionPowerCellModel = GameObject.Instantiate(powerCellModel, powerCellModel.transform.parent);
						ionPowerCellModel.name = "PrecursorIonPowerCellModel";


						Material precursorIonPowerCellMaterial = ionPowerCellPrefab?.GetComponentInChildren<Renderer>()?.material;
						if (precursorIonPowerCellMaterial != null)
						{
							ionPowerCellModel.GetComponentInChildren<Renderer>().material = new Material(precursorIonPowerCellMaterial);
							batteryModels.Add(new BatteryModels() { model = ionPowerCellModel, techType = TechType.PrecursorIonPowerCell });
							existingTechtypes.Add(TechType.PrecursorIonPowerCell);
							existingModels.Add(ionPowerCellModel);
						}
					}
				}

				//Remove models from the controlled objects list after we have added them as controlled models instead.
				List<GameObject> controlledObjects = new List<GameObject>(__instance.controlledObjects ?? new GameObject[0]);

				foreach (GameObject gameObject in __instance.controlledObjects ?? new GameObject[0])
				{
					if (!existingModels.Contains(gameObject))
						controlledObjects.Add(gameObject);
				}
				__instance.controlledObjects = controlledObjects.ToArray();

				if (compatibleBatteries.Contains(TechType.Battery) || compatibleBatteries.Contains(TechType.PrecursorIonBattery))
				{
					// If the regular Battery or Ion Battery is compatible with this item, then modded batteries should also be compatible
					var magicBatteryTechType = new MagicBatteryItem().TechType;

					if (!compatibleBatteries.Contains(magicBatteryTechType))
						compatibleBatteries.Add(magicBatteryTechType);

					if (batteryModel != null && ionBatteryModel != null)
					{
						//If we have enough information to make custom models for this tool or vehicle then create them.
						batteryModels.Add(new BatteryModels() {model = batteryModel, techType = magicBatteryTechType });
					}
				}

				//if (compatibleBatteries.Contains(TechType.PowerCell) || compatibleBatteries.Contains(TechType.PrecursorIonPowerCell))
				//{
				//	// If the regular Power Cell or Ion Power Cell is compatible with this item, then modded power cells should also be compatible
				//	AddMissingTechTypesToList(compatibleBatteries, CbDatabase.PowerCellItems);

				//	if (powerCellModel != null && ionPowerCellModel != null)
				//	{
				//		//If we have enough information to make custom models for this tool or vehicle then create them.
				//		AddCustomModels(powerCellModel, ionPowerCellModel, ref Models, CbDatabase.PowerCellModels, existingTechtypes);
				//	}
				//}

				__instance.batteryModels = batteryModels.ToArray();
			}
		}
	}
}
