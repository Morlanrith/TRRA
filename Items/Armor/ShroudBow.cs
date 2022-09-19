using Terraria;
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
			.AddIngredient(ItemID.Silk, 30)
			.AddTile(TileID.Loom)
			.Register();
	}
}
