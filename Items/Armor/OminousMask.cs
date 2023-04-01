using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class OminousMask : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ominous Mask");
			Tooltip.SetDefault("'Close your eyes now time for dreams'");
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
			.AddIngredient(ItemID.Deathweed, 1)
			.AddIngredient(ItemID.Bone, 5)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
