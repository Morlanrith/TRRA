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
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = TileType<Tiles.PlantDustCrystalTile>();
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 25);
			Item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<PlantDustExtract>(), 1)
			.AddIngredient(ItemID.CrystalShard, 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();
	}
}
