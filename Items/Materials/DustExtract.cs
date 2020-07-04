using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Materials
{
	public class DustExtract : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("A strange otherworldly extract, obtained from the Golems power cell");
		}

		public override void SetDefaults() {
			item.width = 22;
			item.height = 22;
			item.maxStack = 99;
			item.value = Item.sellPrice(silver: 50);
			item.rare = ItemRarityID.Yellow;
		}
	}
}
