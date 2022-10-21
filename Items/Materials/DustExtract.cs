using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Materials
{
	public class DustExtract : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("A strange otherworldly extract, obtained from the Golems power cell");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Yellow;
		}
	}
}
