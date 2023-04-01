using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class SilentBoots : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Silent Boots");
			Tooltip.SetDefault("'Now you'll pay the price'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
			.AddIngredient(ItemID.PinkThread, 1)
			.AddIngredient(ItemID.Silk, 20)
			.AddTile(TileID.Loom)
            .Register();
	}
}
