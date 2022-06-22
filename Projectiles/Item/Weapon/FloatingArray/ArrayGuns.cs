using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.FloatingArray
{
	public class ArrayGuns : ModProjectile
	{
		private int currentProjectile = -1;

		private static readonly SoundStyle ArrayLaserSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/FloatingArray/ArrayLaser")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Array Guns");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 75;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.ignoreWater = true;
		}

        public override bool? CanDamage()
        {
			return false;
        }

        public override void AI()
        {
			AI_Prism();
        }

		private void AI_Prism()
        {
			Player player = Main.player[Projectile.owner];
			float num = (float)Math.PI / 2f;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
			int num2 = 2;
			float num3 = 0f;
			float num37 = 30f;
			if (Projectile.ai[0] > 90f)
			{
				num37 = 15f;
			}
			if (Projectile.ai[0] > 120f)
			{
				num37 = 5f;
			}
			Projectile.ai[0] += 1f;
			Projectile.ai[1] += 1f;
			bool flag8 = false;
			if (Projectile.ai[0] % num37 == 0f)
			{
				flag8 = true;
			}
			int num38 = 10;
			bool flag9 = false;
			if (Projectile.ai[0] % num37 == 0f)
			{
				flag9 = true;
			}
			if (Projectile.ai[1] >= 1f)
			{
				Projectile.ai[1] = 0f;
				flag9 = true;
				if (Main.myPlayer == Projectile.owner)
				{
					float num39 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
					Vector2 vector18 = vector;
					Vector2 value6 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector18;
					if (player.gravDir == -1f)
					{
						value6.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector18.Y;
					}
					Vector2 value7 = Vector2.Normalize(value6);
					if (float.IsNaN(value7.X) || float.IsNaN(value7.Y))
					{
						value7 = -Vector2.UnitY;
					}
					value7 = Vector2.Normalize(Vector2.Lerp(value7, Vector2.Normalize(Projectile.velocity), 0.92f));
					value7 *= num39;
					if (value7.X != Projectile.velocity.X || value7.Y != Projectile.velocity.Y)
					{
						Projectile.netUpdate = true;
					}
					Projectile.velocity = value7;
				}
			}
			Projectile.frameCounter++;
			int num40 = ((!(Projectile.ai[0] < 120f)) ? 1 : 4);
			if (Projectile.frameCounter >= num40)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
			}
			if (Projectile.soundDelay <= 0)
			{
				Projectile.soundDelay = num38;
				Projectile.soundDelay *= 2;
				if (Projectile.ai[0] != 1f)
				{
					SoundEngine.PlaySound(ArrayLaserSound, Projectile.position);
				}
			}
			if (flag9 && Main.myPlayer == Projectile.owner)
			{
				bool flag10 = false;
				flag10 = !flag8 || player.CheckMana(player.inventory[player.selectedItem].mana, pay: true);
				if (player.channel && flag10 && !player.noItems && !player.CCed)
				{
					if(currentProjectile == -1) Generate_Laser();
				}
				else
				{
					Projectile.Kill();
					Main.player[Projectile.owner].channel = false;
				}
			}
			Vector2 rotatedCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - Projectile.Size / 2f;
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 distToProj = playerCenter - Projectile.Top.RotatedBy(Projectile.rotation, Projectile.Center);
			float distance = distToProj.Length();
			float newX = distToProj.X * (20f / distance);
			float newY = distToProj.Y * (20f / distance);
			Projectile.position = new Vector2(rotatedCenter.X - newX, rotatedCenter.Y - newY);
			Projectile.rotation = Projectile.velocity.ToRotation() + num;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.timeLeft = 2;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(num2);
			player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(Projectile.velocity.Y * (float)Projectile.direction, Projectile.velocity.X * (float)Projectile.direction) + num3);
		}

		private void Generate_Laser()
        {
			Vector2 center2 = Projectile.Center;
			Vector2 vector19 = Vector2.Normalize(Projectile.velocity);
			if (float.IsNaN(vector19.X) || float.IsNaN(vector19.Y))
			{
				vector19 = -Vector2.UnitY;
			}
			int num41 = Projectile.damage;
			currentProjectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vector19, ProjectileType<ArrayLaser>(), num41, Projectile.knockBack, Projectile.owner, 2, Projectile.whoAmI);
			Projectile.netUpdate = true;
		}
	}
}