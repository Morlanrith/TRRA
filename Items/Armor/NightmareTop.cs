using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class NightmareTop : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<ShroudTop>();
        }

        public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}
	}
}
