using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Placeable
{
    public class GeistRelic : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Geist Relic");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GeistRelic>(), 0);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.value = Item.buyPrice(0, 5);
		}
	}
}
