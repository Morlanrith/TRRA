﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class UnluckyPants : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            SetupDrawing();
        }

        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
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
			.AddIngredient(ItemID.Blinkroot, 1)
			.AddIngredient(ItemID.Silk, 20)
			.AddTile(TileID.Loom)
			.Register();
	}
}
