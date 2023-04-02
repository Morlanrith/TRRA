using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Placeable
{
    public class Worthy : ModItem
	{
		public override void SetStaticDefaults() {
            Tooltip.SetDefault("'L. Bromley'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Worthy>());

			Item.width = 50;
			Item.height = 34;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.White;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
