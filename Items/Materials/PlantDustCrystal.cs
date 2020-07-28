using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Tiles;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Materials
{
	public class PlantDustCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A strange otherworldly crystal that holds the power of plantlife");
		}
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
			item.createTile = TileType<Tiles.PlantDustCrystalTile>();
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(silver: 25);
			item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<PlantDustExtract>(), 1);
			recipe.AddIngredient(ItemID.CrystalShard, 1);
			recipe.AddTile(TileType<DustToolbenchTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
