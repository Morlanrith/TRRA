using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.Omen
{
	public class OmenBlade : ModProjectile
	{
		private static readonly SoundStyle OmenSlashSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Omen/OmenSlash")
		{
			Volume = 0.5f,
			Pitch = 0.7f,
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omen Blade");
			Main.projFrames[Projectile.type] = 35;
		}

		public override void SetDefaults()
		{
			Projectile.width = 68;
			Projectile.height = 64;
			Projectile.scale = 1f;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.alpha = 100;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
		}

		private void DisplayVFX(int dustID)
        {
			if (Main.rand.NextBool(5))
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustID, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			switch (Projectile.frame / 7)
			{
				case 0:
					target.AddBuff(BuffID.OnFire, 300);
					break;
				case 1:
					target.AddBuff(BuffID.Frostburn, 300);
					break;
				case 2:
					target.AddBuff(BuffID.CursedInferno, 300);
					break;
				case 3:
					target.AddBuff(BuffID.ShadowFlame, 300);
					break;
				case 4:
					target.AddBuff(BuffID.Ichor, 600);
					break;
			}
		}

        public override void AI()
        {
			int itemType = Main.player[Projectile.owner].HeldItem.type;
			if (itemType != ItemType<Items.Weapons.Omen>()) Projectile.Kill();
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 21;
				SoundEngine.PlaySound(OmenSlashSound, Projectile.position);
			}
			//Settings for updating on net
			Vector2 vector22 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
			if (Main.myPlayer == Projectile.owner)
			{
				if (Main.player[Projectile.owner].channel)
				{
					float num263 = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].shootSpeed * Projectile.scale;
					Vector2 vector23 = vector22;
					float num264 = Main.mouseX + Main.screenPosition.X - vector23.X;
					float num265 = Main.mouseY + Main.screenPosition.Y - vector23.Y;
					if (Main.player[Projectile.owner].gravDir == -1f)
					{
						num265 = Main.screenHeight - Main.mouseY + Main.screenPosition.Y - vector23.Y;
					}

                    _ = (float)Math.Sqrt(num264 * num264 + num265 * num265);
                    float num266 = (float)Math.Sqrt(num264 * num264 + num265 * num265);
                    num266 = num263 / num266;
					num264 *= num266;
					num265 *= num266;
					if (num264 != Projectile.velocity.X || num265 != Projectile.velocity.Y)
					{
						Projectile.netUpdate = true;
					}
					Projectile.velocity.X = num264;
					Projectile.velocity.Y = num265;
				}
				else
				{
					Projectile.Kill();
				}
			}
			
			//Setting the position and direction
			Projectile.spriteDirection = Projectile.direction;
			Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 distToProj = playerCenter - Main.MouseWorld;
			float distance = distToProj.Length();
			float newX = distToProj.X * (35f / distance);
			float newY = distToProj.Y * (35f / distance);
			Projectile.position = new Vector2(playerCenter.X - newX - 32, playerCenter.Y - newY - 28);
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			
			//Rotation of Projectile and Item Use
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= 3f;
			}
			if (Main.player[Projectile.owner].MountedCenter.X - Main.MouseWorld.X > 0)
			{
				Main.player[Projectile.owner].direction = -1;
				Main.player[Projectile.owner].itemRotation = (Main.player[Projectile.owner].MountedCenter - Main.MouseWorld).ToRotation();
			}
			else
			{
				Main.player[Projectile.owner].direction = 1;
				Main.player[Projectile.owner].itemRotation = (Main.MouseWorld - Main.player[Projectile.owner].MountedCenter).ToRotation();
			}

			//Make velocity really low towards mouse
			Projectile.velocity.X = Projectile.velocity.X * (1f + Main.rand.Next(-3, 4) * 0.01f);

			//Animation and firing in terms of frameCounter and first counter
			if (++Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 35)
				{
					Projectile.frame = 0;
				}
			}

			switch (Projectile.frame / 7)
			{
				case 0:
					DisplayVFX(DustID.RedTorch);
					break;
				case 1:
					DisplayVFX(DustID.IceTorch);
					break;
				case 2:
					DisplayVFX(DustID.GreenTorch);
					break;
				case 3:
					DisplayVFX(DustID.PurpleTorch);
					break;
				case 4:
					DisplayVFX(DustID.YellowTorch);
					break;
			}

			Main.player[Projectile.owner].itemTime = 21;
			Main.player[Projectile.owner].itemAnimation = 21;
		}
	}
}