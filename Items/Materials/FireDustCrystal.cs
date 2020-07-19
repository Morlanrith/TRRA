﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Materials
{
	public class FireDustCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A strange otherworldly crystal that holds the power of fire");
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
			item.createTile = TileType<Tiles.FireDustCrystalTile>();
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(silver: 25);
			item.rare = ItemRarityID.Blue;
		}
	}
}
