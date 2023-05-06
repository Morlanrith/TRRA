using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Items.Materials;
using TRRA.Projectiles.Item.Weapon.CrescentRose;
using TRRA.Tiles;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Weapons
{
	public class SunderedRoseG : ModItem
	{
		private static readonly SoundStyle RoseShotSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseShot")
		{
			Volume = 0.6f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 240;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 56;
			Item.height = 16;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			// Plays the sound effect for a Crescent Rose gunshot
			Item.UseSound = RoseShotSound;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 16f;
			Item.crit = 36;
			Item.useAmmo = AmmoID.Bullet;
			Item.maxStack = 1;
		}

		public override void UseStyle(Player player, Rectangle r)
		{
			// Allows the player to utilise the scope function with Right Click
			player.scope = true;
		}

		public override void HoldItemFrame(Player player)
		{
			player.scope = true;
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(4, 0);
		}

		// Offsets the fire location of the bullet from the weapons muzzle
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileType<RoseBullet>(), damage, knockback, player.whoAmI);

            return true;
        }

    }
}
