using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class BurningAviators : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Burning Aviators");
			Tooltip.SetDefault("'Fool, you shouldn't stare into these eyes of fire'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

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
			.AddIngredient(ItemID.Fireblossom, 1)
			.AddIngredient(ItemID.Sunglasses, 1)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
