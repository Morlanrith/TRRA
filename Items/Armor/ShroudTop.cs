using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class ShroudTop : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Shroud Top");
			Tooltip.SetDefault("'Be seen for who you are'");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
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
