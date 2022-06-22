using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Weapon.Myrtenaster;

namespace TRRA.Items.Weapons
{
	public class MyrtenasterF : ModItem
	{
		private bool resetTime = false;

		private static readonly SoundStyle FireStabSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Myrtenaster/FireStab")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle FireWaveSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Myrtenaster/FireWave")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Myrtenaster");
			Tooltip.SetDefault("For those who are more than just a name\nRight Click to shoot a wave of fire\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			Item.damage = 25;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useAnimation = 12;
			Item.useTime = 4;
			Item.knockBack = 4.5f;
			Item.width = 46;
			Item.height = 46;
			Item.scale = 0.9f;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(gold: 25);
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
			Item.UseSound = FireStabSound;
			Item.shoot = ProjectileType<MyrtenasterFR>();
			Item.shootSpeed = 6f;
			Item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (player.altFunctionUse != 2)
			{
				target.AddBuff(BuffID.OnFire, 180);
			}
		}


		public override void HoldItem(Player player)
		{
			if (player.altFunctionUse == 2) player.itemRotation = 0f;
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (player.altFunctionUse == 2)
			{
				if (PlayerInput.Triggers.JustReleased.MouseRight) //Stops the animation manually
				{
					resetTime = true;
				}
				if (player.itemAnimation == 1) //Resets the animation so it doesn't let the hand return to resting position
				{
					if (!resetTime)
					{
						player.itemAnimation = Item.useAnimation;
						SoundEngine.PlaySound(FireWaveSound, player.Center);
					}
					else resetTime = false;
				}
			}
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.DamageType = DamageClass.Ranged;
				Item.useTime = 45;
				Item.useAnimation = 45;
				Item.damage = 135;
				Item.knockBack = 7.5f;
				Item.shoot = ProjectileType<MyrtenasterFS>();
				Item.UseSound = FireWaveSound;
			}
			else {
				Item.useStyle = ItemUseStyleID.Thrust;
				Item.DamageType = DamageClass.Melee;
				Item.noMelee = false;
				Item.useTime = 2;
				Item.useAnimation = 12;
				Item.damage = 25;
				Item.knockBack = 4.5f;
				Item.shoot = ProjectileType<MyrtenasterFR>();
				Item.UseSound = FireStabSound;
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -11);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				float posY = player.position.Y + 25;
				float posX = player.position.X - 33;
				if (player.direction == 1) posX += 83;
				Random r = new Random();
				posY += r.Next(-20, 20);
				velocity.X = new Vector2(velocity.X, velocity.Y).Length() * (velocity.X > 0 ? 1 : -1);
				Projectile.NewProjectile(source, posX, posY, velocity.X, 0, type, (int)(18 * player.GetDamage(DamageClass.Melee).Multiplicative), Item.knockBack, player.whoAmI);
			}			
			else
			{
				Projectile.NewProjectile(source, position, velocity, type, (int)(135 * player.GetDamage(DamageClass.Ranged).Multiplicative), Item.knockBack, player.whoAmI);
			}
			return false;
		}
	}
}