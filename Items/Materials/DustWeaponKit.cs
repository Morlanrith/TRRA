using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Materials
{
	public class DustWeaponKit : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("A kit containing various tools and components to contruct weapons from Dust");
		}

		public override void SetDefaults() {
			item.width = 32;
			item.height = 34;
			item.maxStack = 99;
			item.value = Item.sellPrice(gold: 20);
			item.rare = ItemRarityID.Lime;
		}
	}
}
