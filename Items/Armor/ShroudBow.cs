﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ShroudBow : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Shroud Bow");
			Tooltip.SetDefault("'It'll go great with your pajamas!'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

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
