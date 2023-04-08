using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class BurningBoots : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Burning Boots");
			// Tooltip.SetDefault("'Yellow beauty burns gold'");
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
			.AddIngredient(ItemID.Fireblossom, 1)
			.AddIngredient(ItemID.Silk, 20)
			.AddTile(TileID.Loom)
			.Register();
	}
}
