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
			.AddIngredient(ItemID.Silk, 30)
			.AddTile(TileID.Loom)
			.Register();
	}
}
