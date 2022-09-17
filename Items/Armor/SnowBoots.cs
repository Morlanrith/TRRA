using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class SnowBoots : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Snow Boots");
			Tooltip.SetDefault("'Carve out your own way'");
		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Silk, 30)
			.AddTile(TileID.Loom)
			.Register();
	}
}
