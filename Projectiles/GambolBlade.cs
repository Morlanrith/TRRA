using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TRRA.Projectiles
{
	public class GambolBlade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambol Blade");
			Main.projFrames[projectile.type] = 14;
		}

		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 60;
			projectile.scale = 1f;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.penetrate = -1;
		}
		
		public override void AI()
        {
			if (projectile.soundDelay == 0)
			{
				projectile.soundDelay = 21;
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/GambolSwing"), projectile.position);
			}
			if (Main.rand.Next(5) == 0)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 27, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 150, default, 0.7f);
			}
			//Settings for updating on net
			Vector2 vector22 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
			if (Main.myPlayer == projectile.owner)
			{
				if (Main.player[projectile.owner].channel)
				{
					float num263 = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].shootSpeed * projectile.scale;
					Vector2 vector23 = vector22;
					float num264 = Main.mouseX + Main.screenPosition.X - vector23.X;
					float num265 = Main.mouseY + Main.screenPosition.Y - vector23.Y;
					if (Main.player[projectile.owner].gravDir == -1f)
					{
						num265 = Main.screenHeight - Main.mouseY + Main.screenPosition.Y - vector23.Y;
					}

                    _ = (float)Math.Sqrt(num264 * num264 + num265 * num265);
                    float num266 = (float)Math.Sqrt(num264 * num264 + num265 * num265);
                    num266 = num263 / num266;
					num264 *= num266;
					num265 *= num266;
					if (num264 != projectile.velocity.X || num265 != projectile.velocity.Y)
					{
						projectile.netUpdate = true;
					}
					projectile.velocity.X = num264;
					projectile.velocity.Y = num265;
				}
				else
				{
					projectile.Kill();
				}
			}
			
			//Setting the position and direction
			projectile.spriteDirection = projectile.direction;
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 distToProj = playerCenter - Main.MouseWorld;
			float distance = distToProj.Length();
			float newX = distToProj.X * (35f / distance);
			float newY = distToProj.Y * (35f / distance);
			projectile.position = new Vector2(playerCenter.X - newX - 32, playerCenter.Y - newY - 28);
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
			
			//Rotation of Projectile and Item Use
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= 3f;
			}
			if (Main.player[projectile.owner].MountedCenter.X - Main.MouseWorld.X > 0)
			{
				Main.player[projectile.owner].direction = -1;
				Main.player[projectile.owner].itemRotation = (Main.player[projectile.owner].MountedCenter - Main.MouseWorld).ToRotation();
			}
			else
			{
				Main.player[projectile.owner].direction = 1;
				Main.player[projectile.owner].itemRotation = (Main.MouseWorld - Main.player[projectile.owner].MountedCenter).ToRotation();
			}

			//Make velocity really low towards mouse
			projectile.velocity.X = projectile.velocity.X * (1f + Main.rand.Next(-3, 4) * 0.01f);

			//Animation and firing in terms of frameCounter and first counter
			if (++projectile.frameCounter >= 3)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 14)
				{
					projectile.frame = 0;
				}
			}

			Main.player[projectile.owner].itemTime = 21;
			Main.player[projectile.owner].itemAnimation = 21;
		}
	}
}