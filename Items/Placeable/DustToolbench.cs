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
			item.width = 28;
			item.height = 24;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Orange;
			item.createTile = TileType<Tiles.DustToolbenchTile>();
		}
	}
}