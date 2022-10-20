using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class RoseBoots : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Rose Boots");
			Tooltip.SetDefault("'Just like the heroes in the books'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Daybloom, 1)
			.AddIngredient(ItemID.Silk, 20)
			.AddTile(TileID.Loom)
			.Register();
	}
}
