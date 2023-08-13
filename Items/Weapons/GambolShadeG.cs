using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Weapon.GambolShroud;

namespace TRRA.Items.Weapons
{
	public class GambolShadeG : ModItem
	{

		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 80;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 34;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.Pink;
			Item.noUseGraphic = true;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 7f;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<ShadeRibbonEnd>();
			Item.shootSpeed = 10f;
			Item.crit = 0;
			Item.useAmmo = AmmoID.None;
			Item.channel = true;
			Item.autoReuse = false;
			Item.maxStack = 1;
		}

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
			// Prevents the player from utilising the scope function with Right Click
			player.scope = false;
        }

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(-15, 3);
		}

	}
}
