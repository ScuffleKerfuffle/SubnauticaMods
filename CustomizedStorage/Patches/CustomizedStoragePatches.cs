using CustomizedStorage.Config;
using CustomizedStorage.Utility;
using HarmonyLib;
using UnityEngine;

namespace CustomizedStorage.Patches
{
	internal class CustomizedStoragePatches
	{
		[HarmonyPatch(typeof(BaseBioReactor))]
		[HarmonyPatch("get_container")]
		internal class BaseBioReactor_get_container_Patch
		{
			private static void Postfix(BaseBioReactor __instance) => __instance.UpdateStorageSizeWithFieldInfo(ModConfig.Instance.BioReactorWidth, ModConfig.Instance.BioReactorHeight);
		}

		[HarmonyPatch(typeof(Exosuit))]
		[HarmonyPatch("UpdateStorageSize")]
		internal class Exosuit_UpdateStorageSize_Patch
		{
			private static void Postfix(Exosuit __instance) => __instance.UpdateExosuitStorageSize();
		}

		[HarmonyPatch(typeof(FiltrationMachine))]
		[HarmonyPatch("Start")]
		internal class FiltrationMachine_Start_Patch
		{
			private static void Postfix(FiltrationMachine __instance) => __instance.UpdateFilterStorageSize();
		}

		[HarmonyPatch(typeof(Inventory))]
		[HarmonyPatch("Awake")]
		internal class Inventory_Awake_Patch
		{
			private static void Postfix(Inventory __instance) => __instance.UpdateStorageSizeWithFieldInfo(ModConfig.Instance.InventoryWidth, ModConfig.Instance.InventoryHeight);
		}

		[HarmonyPatch(typeof(SeamothStorageContainer))]
		[HarmonyPatch("Init")]
		internal class SeamothStorageContainer_Init_Patch
		{
			private static bool Prefix(SeamothStorageContainer __instance)
			{
				__instance.width = ModConfig.Instance.SeamothWidth;
				__instance.height = ModConfig.Instance.SeamothHeight;
				return true;
			}
		}

		[HarmonyPatch(typeof(uGUI_ItemsContainer))]
		[HarmonyPatch("OnResize")]
		internal class uGUI_ItemsContainer_OnResize_Patch
		{
			private static void Postfix(uGUI_ItemsContainer __instance, int width, int height)
			{
				float x = __instance.rectTransform.anchoredPosition.x;
				switch (height)
				{
					case 9:
						__instance.rectTransform.anchoredPosition = new Vector2(x, -39f);
						break;
					case 10:
						__instance.rectTransform.anchoredPosition = new Vector2(x, -75f);
						break;
					default:
						__instance.rectTransform.anchoredPosition = new Vector2(x, -4f);
						break;
				}
				float y = __instance.rectTransform.anchoredPosition.y;
				float num = Mathf.Sign(x);
				if (width == 8)
					__instance.rectTransform.anchoredPosition = new Vector2(num * 292f, y);
				else
					__instance.rectTransform.anchoredPosition = new Vector2(num * 284f, y);
			}
		}

		[HarmonyPatch(typeof(StorageContainer))]
		[HarmonyPatch("Awake")]
		internal class StorageContainer_Awake_Patch
		{
			private static bool Prefix(StorageContainer __instance)
			{
				__instance.UpdateStorageSize();

				return true;
			}
		}
	}
}
