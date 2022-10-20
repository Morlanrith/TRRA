using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Pet;

namespace TRRA.Items.Pets
{
	public class ZweiSummoner : ModItem
	{
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Zwei Summoner");
            Tooltip.SetDefault("'This is the beginning of the end...'\nSummons the Shattered Moon");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.width = 22;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item2;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<Zwei>();
            Item.buffType = ModContent.BuffType<ZweiBuff>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }

    }
}