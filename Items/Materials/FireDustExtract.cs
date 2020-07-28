using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Materials
{
	public class FireDustExtract : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("An extract of Fire Dust\nCan be used to convert Crystal Shards to Fire Dust Crystals");
		}

		public override void SetDefaults() {
			item.width = 22;
			item.height = 22;
			item.maxStack = 999;
			item.value = Item.sellPrice(copper: 10);
			item.rare = ItemRarityID.Green;
		}
	}
}
