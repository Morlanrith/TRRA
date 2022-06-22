using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Weapons
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class EmberCelicaR : ModItem
	{
		private static readonly SoundStyle RocketSingleSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/EmberCelica/RocketSingle")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle RocketTripleSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/EmberCelica/RocketTriple")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ember Celica");
			Tooltip.SetDefault("Armed and Ready\nRight Click to fire a splitting Rocket\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			Item.damage = 50;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 62;
			Item.height = 34;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.autoReuse = false;
			Item.shoot = ProjectileType<Projectiles.Item.Weapon.EmberCelica.EmberRocket>();
			Item.shootSpeed = 9f;
			Item.useAmmo = AmmoID.Rocket;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}

		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2)
			{
				Item.UseSound = RocketTripleSound;
				Item.shoot = ProjectileType<Projectiles.Item.Weapon.EmberCelica.TripEmberRocket>(); 
			}
			else
			{
				Item.UseSound = RocketSingleSound;
				Item.shoot = ProjectileType<Projectiles.Item.Weapon.EmberCelica.EmberRocket>();
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
			Projectile.NewProjectile(source, position, velocity, Item.shoot, damage + source.Item.damage, knockback, player.whoAmI);
			return false;
		}

	}
}