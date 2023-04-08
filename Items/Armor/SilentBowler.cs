using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class SilentBowler : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Silent Bowler");
			// Tooltip.SetDefault("'Like a candle's flame'");
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

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Feather, 1)
				.AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.Loom)
                .Register();
		}
	}
}
