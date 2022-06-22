using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Placeable
{
	public class DustToolbench : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Converts Dust Crystals into various Weapons and Ammo");
		}

		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Orange;
			Item.createTile = TileType<Tiles.DustToolbenchTile>();
		}
	}
}