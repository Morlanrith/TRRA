using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Materials
{
	public class FireDustExtract : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
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
