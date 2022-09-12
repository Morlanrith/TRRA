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
			Tooltip.SetDefault("This is a modded body armor.");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 14;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            base.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
			drawPlayer.back = Item.backSlot;
		}

        public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Silk, 1)
			.AddTile(TileID.Loom)
			.Register();
	}
}
