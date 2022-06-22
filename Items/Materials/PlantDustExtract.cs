using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Materials
{
	public class PlantDustExtract : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("An extract of Plant Dust\nCan be used to convert Crystal Shards to Plant Dust Crystals");
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(copper: 10);
			Item.rare = ItemRarityID.Green;
		}
	}
}
