using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.GambolShroud
{
	public class GambolBlade : ModProjectile
	{
		private static readonly SoundStyle GambolSwingSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/GambolShroud/GambolSwing")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 14;
		}

		public override void SetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 60;
			Projectile.scale = 1f;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
		}
		
		public override void AI()
        {
			if (Main.player[Projectile.owner].HeldItem.type != ItemType<Items.Weapons.GambolShroudS>()) Projectile.Kill();
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 21;
				SoundEngine.PlaySound(GambolSwingSound, Projectile.position);
			}
			if (Main.rand.NextBool(5))
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
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
				if (++Projectile.frame >= 14)
				{
					Projectile.frame = 0;
				}
			}

			Main.player[Projectile.owner].itemTime = 21;
			Main.player[Projectile.owner].itemAnimation = 21;
		}
	}
}