using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Pet;

namespace TRRA.Items.Pets
{
	public class ZweiSummoner : ModItem
	{
        private static readonly SoundStyle ZweiBarkSound = new($"{nameof(TRRA)}/Sounds/Item/Pet/ZweiBark")
        {
            Volume = 1.0f,
            Pitch = 0.0f,
        };

        public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Package from Patch");
            // Tooltip.SetDefault("Summons Zwei\n'He sent a dog? In the mail?'");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.width = 38;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = ZweiBarkSound;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Orange;
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