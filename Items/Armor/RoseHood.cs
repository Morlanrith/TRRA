using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class RoseHood : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Rose Hood");
			Tooltip.SetDefault("'Just like the heroes in the books'");
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}


		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Daybloom, 1)
			.AddIngredient(ItemID.Silk, 10)
			.AddTile(TileID.Loom)
			.Register();
	}
}
