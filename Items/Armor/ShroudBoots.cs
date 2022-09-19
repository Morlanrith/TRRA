using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ShroudBoots : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Shroud Boots");
			Tooltip.SetDefault("'Be seen for who you are'");
			SetupDrawing();
		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
			ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Silk, 30)
			.AddTile(TileID.Loom)
			.Register();
	}
}
