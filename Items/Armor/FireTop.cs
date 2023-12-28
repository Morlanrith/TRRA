using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class FireTop : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<BurningJacket>();
        }

        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}
	}
}
