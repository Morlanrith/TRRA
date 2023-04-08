using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Materials
{
	public class DustWeaponKit : ModItem
	{
		public override void SetStaticDefaults() {
			// Tooltip.SetDefault("A kit containing various tools and components to contruct weapons from Dust");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 34;
			Item.maxStack = 99;
			Item.value = Item.buyPrice(gold: 25);
			Item.rare = ItemRarityID.Green;
		}
	}
}
