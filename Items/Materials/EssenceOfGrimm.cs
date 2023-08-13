using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Materials
{
	public class EssenceOfGrimm : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = ItemRarityID.LightRed;
		}

        public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 255, 255, 50);
		}

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
			float num8 = (float)Main.rand.Next(90, 111) * 0.01f;
			num8 *= Main.essScale;
			Lighting.AddLight((int)((Item.position.X + (float)(Item.width / 2)) / 16f), (int)((Item.position.Y + (float)(Item.height / 2)) / 16f), 0.6f * num8, 0.1f * num8, 0.3f * num8);
		}

	}
}
