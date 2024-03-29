﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ShroudBow : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<NightmareMask>();

            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}


		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Moonglow, 1)
			.AddIngredient(ItemID.BlackThread, 1)
			.AddIngredient(ItemID.Silk, 5)
			.AddTile(TileID.Loom)
			.Register();
	}
}
