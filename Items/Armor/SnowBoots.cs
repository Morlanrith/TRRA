using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class SnowBoots : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<WinterBoots>();
        }

        public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Shiverthorn, 1)
			.AddIngredient(ItemID.Silk, 20)
			.AddTile(TileID.Loom)
			.Register();
	}
}
