using BepInEx.Logging;
using CustomizedStorage.Bepinex;
using CustomizedStorage.Config;
using System.Reflection;
using UnityEngine;

namespace CustomizedStorage.Utility
{
	internal static class Extensions
	{
		private static ModConfig Config => ModConfig.Instance;
		private static bool LogChanges => Config.LogChanges;
		private static ManualLogSource Logger => Config.Logger;

		#region Object Verifiers

		internal static bool IsSmallLocker(this StorageContainer container) => container.gameObject.name.StartsWith("SmallLocker");

		internal static bool IsLargeLocker(this StorageContainer container) => container.gameObject.name.StartsWith("Locker");

		internal static bool IsEscapePodLocker(this StorageContainer container) => !Equals(container.gameObject.GetComponent<SpawnEscapePodSupplies>(), null);

		internal static bool IsCyclopsLocker(this StorageContainer container) => container.gameObject.name.StartsWith("submarine_locker_01_door");

		internal static bool IsWaterproofLocker(this StorageContainer container) => !Equals(container.gameObject.GetComponent<SmallStorage>(), null);

		internal static bool IsCarryAll(this StorageContainer container) => !Equals(container.transform.parent, null) && container.transform.parent.gameObject.name.StartsWith("docking_luggage_01_bag4");

		#endregion

		#region Resize Extensions

		internal static void UpdateExosuitStorageSize(this Exosuit exosuit)
		{
			var width = Config.ExosuitWidth;
			var height = Config.ExosuitHeight;

			if (LogChanges)
				Logger.LogInfo($"{PluginInfo.PLUGIN_NAME} updating the size of {exosuit.name} to be {width}x{height}.");

			var storageModuleCount = exosuit.modules.GetCount(TechType.VehicleStorageModule);
			var extraRows = storageModuleCount * Config.ExosuitRowsPerModule;
			exosuit.storageContainer.Resize(width, height + extraRows);
		}

		internal static void UpdateFilterStorageSize(this FiltrationMachine machine)
		{
			var saltPercentage = Config.FiltrationMachineSaltPercentage / 100m; //Max Salt Percentage as a, well, percentage.
			var waterPercentage = Config.FiltrationMachineWaterPercentage / 100m; //Max Salt Percentage as a, well, percentage.
			var height = Config.FiltrationMachineHeight;
			var width = Config.FiltrationMachineWidth;

			var totalMaxItemsAllowed = width * height; ;
			var maxSalt = (int)decimal.Floor(totalMaxItemsAllowed * saltPercentage);
			var maxWater = (int)decimal.Floor(totalMaxItemsAllowed * waterPercentage);

			if (maxSalt + maxWater < totalMaxItemsAllowed)
			{
				maxWater = totalMaxItemsAllowed - maxSalt;
			}

			if (LogChanges)
				Logger.LogInfo($"{PluginInfo.PLUGIN_NAME} Filtration Udpated. Values: Height: {height}, Width: {width}, Max Salt {maxSalt}, Max Water {maxWater}.");

			machine.maxSalt = maxSalt;
			machine.maxWater = maxWater;
			machine.storageContainer.Resize(width, height);
		}

		internal static void UpdateStorageSize(this StorageContainer container)
		{
			int width = 0, height = 0;

			if (container.IsCarryAll()) { width = Config.CarryAllWidth; height = Config.CarryAllHeight; }
			else if (container.IsCyclopsLocker()) { width = Config.CyclopsWidth; height = Config.CyclopsHeight; }
			else if (container.IsEscapePodLocker()) { width = Config.EscapePodWidth; height = Config.EscapePodHeight; }
			else if (container.IsLargeLocker()) { width = Config.LockerWidth; height = Config.LockerHeight; }
			else if (container.IsSmallLocker()) { width = Config.SmallLockerWidth; height = Config.SmallLockerHeight; }
			else if (container.IsWaterproofLocker()) { width = Config.WaterproofLockerWidth; height = Config.WaterproofLockerHeight; }

			if (LogChanges)
				Logger.LogInfo($"{PluginInfo.PLUGIN_NAME} updating the size of {container.name} to be {width}x{height}.");

			if(width > 0 && height > 0)
				container.Resize(width, height);
		}

		internal static void UpdateStorageSizeWithFieldInfo<T>(this T itemWithContainer, int width, int height) where T : Object
		{
			if (LogChanges)
				Logger.LogInfo($"{PluginInfo.PLUGIN_NAME} updating the size of {itemWithContainer.name} to be {width}x{height}.");

			var fieldInfo = typeof(T).GetField("_container", BindingFlags.Instance | BindingFlags.NonPublic);

			if (fieldInfo == null) return;

			(fieldInfo.GetValue(itemWithContainer) as ItemsContainer)?.Resize(width, height);
		}

		#endregion
	}
}
