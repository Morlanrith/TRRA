using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Body, EquipType.Back)]
	public class RoseCloak : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Rose Cloak");
			Tooltip.SetDefault("'Just like the heroes in the books'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            base.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
			drawPlayer.back = Item.backSlot;
		}

        public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Daybloom, 1)
			.AddIngredient(ItemID.Silk, 20)
			.AddTile(TileID.Loom)
			.Register();
	}
}
