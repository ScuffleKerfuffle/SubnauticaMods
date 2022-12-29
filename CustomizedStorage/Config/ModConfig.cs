using CustomizedStorage.Utility;
using Newtonsoft.Json;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;
using System.Linq;
using SubnauticaUtils;

namespace CustomizedStorage.Config
{
	[Menu("Customized Storage Menu"), ConfigFile("storageconfig")]
	internal class ModConfig : ConfigFile
	{
		#region Internal Props

		[JsonIgnore]
		internal static ModConfig Instance { get; set; } = new ModConfig();
		
		#endregion

		#region Mod Helper Slider Props

		[Slider("Bio Reactor Height", 4, 10, DefaultValue = 4), OnChange(nameof(OnBioReactorSliderChanged))]
		public int BioReactorHeight { get; set; } = 4;
		[Slider("Bio Reactor Width", 4, 8, DefaultValue = 4), OnChange(nameof(OnBioReactorSliderChanged))]
		public int BioReactorWidth { get; set; } = 4;

		[Slider("Carry All Height", 3, 10, DefaultValue = 3), OnChange(nameof(OnCarryAllSliderChanged))]
		public int CarryAllHeight { get; set; } = 3;
		[Slider("Carry All Width", 3, 8, DefaultValue = 3), OnChange(nameof(OnCarryAllSliderChanged))]
		public int CarryAllWidth { get; set; } = 3;

		[Slider("Cyclops Height", 6, 10, DefaultValue = 6), OnChange(nameof(OnCyclopsSliderChanged))] 
		public int CyclopsHeight { get; set; } = 6;
		[Slider("Cyclops Width", 3, 8, DefaultValue = 3), OnChange(nameof(OnCyclopsSliderChanged))] 
		public int CyclopsWidth { get; set; } = 3;

		[Slider("Escape Pod Height", 8, 10, DefaultValue = 8), OnChange(nameof(OnEscapePodSliderChanged))] 
		public int EscapePodHeight { get; set; } = 8;
		[Slider("Escape Pod Width", 4, 8, DefaultValue = 4), OnChange(nameof(OnEscapePodSliderChanged))] 
		public int EscapePodWidth { get; set; } = 4;

		[Slider("Filtration Machine Height", 2, 10, DefaultValue = 2), OnChange(nameof(OnFiltrationMachineSliderChanged))] 
		public int FiltrationMachineHeight { get; set; } = 2;
		[Slider("Filtration Machine Width", 2, 8, DefaultValue = 2), OnChange(nameof(OnFiltrationMachineSliderChanged))] 
		public int FiltrationMachineWidth { get; set; } = 2;

		[Slider("Filtration Machine Salt Percent", 0, 100, DefaultValue = 50, Step = 25, Id = nameof(FiltrationMachineSaltPercentage)), OnChange(nameof(OnFiltrationMachineSliderChanged))] 
		public int FiltrationMachineSaltPercentage { get; set; } = 50;

		[Slider("Filtration Machine Water Percent", 0, 100, DefaultValue = 50, Step = 25, Id = nameof(FiltrationMachineWaterPercentage)), OnChange(nameof(OnFiltrationMachineSliderChanged))] 
		public int FiltrationMachineWaterPercentage { get; set; } = 50;

		[Slider("Prawn Suit Height", 8, 10, DefaultValue = 8), OnChange(nameof(OnExosuitSliderChanged))]
		public int ExosuitHeight { get; set; } = 8;
		[Slider("Prawn Suit Width", 4, 8, DefaultValue = 4), OnChange(nameof(OnExosuitSliderChanged))] 
		public int ExosuitWidth { get; set; } = 4;
		[Slider("Prawn Suit - Rows per Storage Module", 1, 3, DefaultValue = 1), OnChange(nameof(OnExosuitSliderChanged))] 
		public int ExosuitRowsPerModule { get; set; } = 1;

		[Slider("Inventory Height", 8, 10, DefaultValue = 8), OnChange(nameof(OnInventorySliderChanged))] 
		public int InventoryHeight { get; set; } = 8;
		[Slider("Inventory Width", 6, 8, DefaultValue = 6), OnChange(nameof(OnInventorySliderChanged))]
		public int InventoryWidth { get; set; } = 6;

		[Slider("Locker Height", 8, 10, DefaultValue = 8), OnChange(nameof(OnLockerSliderChanged))] 
		public int LockerHeight { get; set; } = 8;
		[Slider("Locker Width", 6, 8, DefaultValue = 6), OnChange(nameof(OnLockerSliderChanged))] 
		public int LockerWidth { get; set; } = 6;

		[Slider("Seamoth Height", 4, 10, DefaultValue = 4), OnChange(nameof(OnSeamothSliderChanged))]
		public int SeamothHeight { get; set; } = 4;
		[Slider("Seamoth Width", 4, 8, DefaultValue = 4), OnChange(nameof(OnSeamothSliderChanged))]
		public int SeamothWidth { get; set; } = 4;

		[Slider("Wall Locker Height", 6, 10, DefaultValue = 6), OnChange(nameof(OnSmallLockerSliderChanged))]
		public int SmallLockerHeight { get; set; } = 6;
		[Slider("Wall Locker Width", 5, 8, DefaultValue = 5), OnChange(nameof(OnSmallLockerSliderChanged))]
		public int SmallLockerWidth { get; set; } = 5;

		[Slider("Waterproof Locker Height", 4, 10, DefaultValue = 4), OnChange(nameof(OnWaterproofLockerSliderChanged))]
		public int WaterproofLockerHeight { get; set; } = 4;
		[Slider("Waterproof Locker Width", 4, 8, DefaultValue = 4), OnChange(nameof(OnWaterproofLockerSliderChanged))]
		public int WaterproofLockerWidth { get; set; } = 4;


		#endregion

		[Toggle("Log Value Changes")]
		public bool LogChanges { get; set; }

		#region Slider Changed Handlers

		private void OnBioReactorSliderChanged(SliderChangedEventArgs _)
		{
			var bioReactors = Object.FindObjectsOfType<BaseBioReactor>().ToList();

			bioReactors.ForEach(b => b.UpdateStorageSizeWithFieldInfo(BioReactorWidth, BioReactorHeight));
		}

		private void OnCarryAllSliderChanged(SliderChangedEventArgs _)
		{
			var storageContainers = Object.FindObjectsOfType<StorageContainer>().Where(c => c.IsCarryAll()).ToList();
			storageContainers.ForEach(c => c.UpdateStorageSize());
		}

		private void OnCyclopsSliderChanged(SliderChangedEventArgs _)
		{
			var storageContainers = Object.FindObjectsOfType<StorageContainer>().Where(c => c.IsCyclopsLocker()).ToList();
			storageContainers.ForEach(c => c.UpdateStorageSize());
		}

		private void OnEscapePodSliderChanged(SliderChangedEventArgs _)
		{
			var storageContainers = Object.FindObjectsOfType<StorageContainer>().Where(c => c.IsEscapePodLocker()).ToList();
			storageContainers.ForEach(c => c.UpdateStorageSize());
		}

		private void OnExosuitSliderChanged(SliderChangedEventArgs _)
		{
			var exosuits = Object.FindObjectsOfType<Exosuit>().ToList();

			if(LogChanges)
				QuickLogger.Info($"Found {exosuits?.Count}.");

			exosuits.ForEach(e => e.UpdateExosuitStorageSize());
		}

		private void OnFiltrationMachineSliderChanged(SliderChangedEventArgs e)
		{
			//Filtration Machines have a configurable amount of water vs salt. In order to
			//not break the game and make this configurable stuff useful, we need to
			//make sure we account for this.
			if (e.Id == nameof(FiltrationMachineSaltPercentage))
			{
				var remaining = 100 - e.IntegerValue;
				FiltrationMachineWaterPercentage = remaining;
			}
			else if (e.Id == nameof(FiltrationMachineWaterPercentage))
			{
				var remaining = 100 - e.IntegerValue;
				FiltrationMachineSaltPercentage = remaining;
			}

			//we now have the values of things. Update time!
			var machines = Object.FindObjectsOfType<FiltrationMachine>().ToList();

			foreach (var machine in machines)
			{
				machine.UpdateFilterStorageSize();
			}
		}

		private void OnInventorySliderChanged(SliderChangedEventArgs _)
		{
			var inventories = Object.FindObjectsOfType<Inventory>().ToList();

			inventories.ForEach(i => i.UpdateStorageSizeWithFieldInfo(InventoryWidth, InventoryHeight));
		}

		private void OnLockerSliderChanged(SliderChangedEventArgs _)
		{
			var storageContainers = Object.FindObjectsOfType<StorageContainer>().Where(c => c.IsLargeLocker()).ToList();
			storageContainers.ForEach(c => c.UpdateStorageSize());
		}

		private void OnSeamothSliderChanged(SliderChangedEventArgs _)
		{
			var seamoths = Object.FindObjectsOfType<SeaMoth>();

			foreach (var seamoth in seamoths)
			{
				var equippedSlots = seamoth.modules.GetEquipment();

				while (equippedSlots.MoveNext())
				{
					var current = equippedSlots.Current.Value;
					if (current.techType == TechType.VehicleStorageModule)
					{
						var pickupable = current.item;
						SeamothStorageContainer storageContainer = pickupable.GetComponent<SeamothStorageContainer>();
						storageContainer?.container?.Resize(SeamothWidth, SeamothHeight);
					}
				}
			}
		}

		private void OnSmallLockerSliderChanged(SliderChangedEventArgs _)
		{
			var storageContainers = Object.FindObjectsOfType<StorageContainer>().Where(c => c.IsSmallLocker()).ToList();
			storageContainers.ForEach(c => c.UpdateStorageSize());
		}

		private void OnWaterproofLockerSliderChanged(SliderChangedEventArgs _)
		{
			var storageContainers = Object.FindObjectsOfType<StorageContainer>().Where(c => c.IsWaterproofLocker()).ToList();
			storageContainers.ForEach(c => c.UpdateStorageSize());
		}

		#endregion		
	}
}
