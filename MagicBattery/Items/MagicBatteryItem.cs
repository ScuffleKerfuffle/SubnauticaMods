using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using SubnauticaUtils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MagicBattery.Items
{
	internal class MagicBatteryItem : Craftable
	{
		public MagicBatteryItem() : base("MagicBattery", "Magic Battery", "A battery with a seemingly-magical storage capacity.")
		{
			//OnStartedPatching += () =>
			//{
			//	TechType = TechTypeHandler.AddTechType(ClassID, FriendlyName, Description, UnlockedAtStart);
			//};
		}

		#region SML Helper overrides

		protected override TechData GetBlueprintRecipe()
		{
			return new TechData
			{
				craftAmount = 1,
				Ingredients = new List<Ingredient>
				{
					new Ingredient(TechType.AcidMushroom, 1)
				}
			};
		}
		
		public override float CraftingTime => 1f;
		public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
		public override string[] StepsToFabricatorTab => new[] { "Resources", "Electronics" };
		public override TechType RequiredForUnlock => TechType.AcidMushroom;
		public override TechGroup GroupForPDA => TechGroup.Resources;
		public override TechCategory CategoryForPDA => TechCategory.Electronics;
		public sealed override string AssetsFolder => "Assets";
		public override string IconFileName => "MagicBattery.png";
		public override PDAEncyclopedia.EntryData EncyclopediaEntryData => new PDAEncyclopedia.EntryData { key = "magicBattery", kind = PDAEncyclopedia.EntryData.Kind.Encyclopedia, nodes = new[] { "Resources", "Electronics" }};
		
		public override GameObject GetGameObject()
		{
			GameObject prefab = CraftData.GetPrefabForTechTypeAsync(TechType.Battery).GetResult();
			var gameObject = Object.Instantiate(prefab);

			var component = gameObject.GetComponent<Battery>();

			component._capacity = this.PowerCapacity;
			component.name = "MagicBattery";

			var skyApplier = Radical.EnsureComponent<SkyApplier>(gameObject);
			skyApplier.renderers = gameObject.GetComponentsInChildren<Renderer>(true);
			skyApplier.anchorSky = Skies.Auto;

			return gameObject;
		}

		#endregion

		#region Battery-related Properties

		public float PowerCapacity { get; set; }

		#endregion
	}
}
