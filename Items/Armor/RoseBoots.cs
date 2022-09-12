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
			Tooltip.SetDefault("This is a modded leg armor.");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Silk, 1)
			.AddTile(TileID.Loom)
			.Register();
	}
}
