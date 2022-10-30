using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ShroudBoots : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Shroud Boots");
			Tooltip.SetDefault("'Nevermore will I run away'");
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
			.AddIngredient(ItemID.Moonglow, 1)
			.AddIngredient(ItemID.Silk, 20)
			.Register();
	}
}
