using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Pet
{
    public class ZweiBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zwei");
            Description.SetDefault("It's art!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Zwei>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.GetSource_FromThis(), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Zwei>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
        }
    }

    public class Zwei : ModProjectile
	{
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Zwei");
            Main.projFrames[Projectile.type] = 11;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 54;
            Projectile.height = 40;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
        }

        public override void AI()
        {
			if (!Main.player[Projectile.owner].active || !Main.player[Projectile.owner].HasBuff(ModContent.BuffType<ZweiBuff>()))
			{
				Projectile.active = false;
				return;
			}
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			int num = 85;
			bool flag7 = Projectile.ai[0] == -1f || Projectile.ai[0] == -2f;
			bool num2 = Projectile.ai[0] == -1f;
			bool flag8 = Projectile.ai[0] == -2f;
			if (flag7)
			{
				Projectile.timeLeft = 2;
			}
			if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) < Projectile.position.X + (float)(Projectile.width / 2) - (float)num)
			{
				flag2 = true;
			}
			else if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) > Projectile.position.X + (float)(Projectile.width / 2) + (float)num)
			{
				flag3 = true;
			}
			if (num2)
			{
				flag2 = false;
				flag3 = true;
			}
			if (flag8)
			{
				flag2 = false;
				flag3 = false;
			}
			bool flag10 = Projectile.ai[1] == 0f;
			if (flag10)
			{
				int num77 = 500;
				if (Main.player[Projectile.owner].rocketDelay2 > 0)
				{
					Projectile.ai[0] = 1f;
				}
				Vector2 vector7 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num78 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector7.X;
				float num79 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector7.Y;
				float num80 = (float)Math.Sqrt(num78 * num78 + num79 * num79);
				if (!flag7)
				{
					if (num80 > 2000f)
					{
						Projectile.position.X = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - (float)(Projectile.width / 2);
						Projectile.position.Y = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - (float)(Projectile.height / 2);
					}
					else if (num80 > (float)num77 || (Math.Abs(num79) > 300f && (!(Projectile.localAI[0] > 0f))))
					{
						if (num79 > 0f && Projectile.velocity.Y < 0f)
						{
							Projectile.velocity.Y = 0f;
						}
						if (num79 < 0f && Projectile.velocity.Y > 0f)
						{
							Projectile.velocity.Y = 0f;
						}
						Projectile.ai[0] = 1f;
					}
				}
			}
			if (Projectile.ai[0] != 0f && !flag7)
			{
				float num82 = 0.2f;
				int num83 = 200;
				Projectile.tileCollide = false;
				Vector2 vector8 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num84 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector8.X;
				float num90 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector8.Y;
				float num91 = (float)Math.Sqrt(num84 * num84 + num90 * num90);
				float num93 = 10f;
				if (num91 < (float)num83 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					if (Projectile.velocity.Y < -6f)
					{
						Projectile.velocity.Y = -6f;
					}
				}
				if (num91 < 60f)
				{
					num84 = Projectile.velocity.X;
					num90 = Projectile.velocity.Y;
				}
				else
				{
					num91 = num93 / num91;
					num84 *= num91;
					num90 *= num91;
				}


				if (Projectile.velocity.X < num84)
				{
					Projectile.velocity.X += num82;
					if (Projectile.velocity.X < 0f)
					{
						Projectile.velocity.X += num82 * 1.5f;
					}
				}
				if (Projectile.velocity.X > num84)
				{
					Projectile.velocity.X -= num82;
					if (Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X -= num82 * 1.5f;
					}
				}
				if (Projectile.velocity.Y < num90)
				{
					Projectile.velocity.Y += num82;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y += num82 * 1.5f;
					}
				}
				if (Projectile.velocity.Y > num90)
				{
					Projectile.velocity.Y -= num82;
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y -= num82 * 1.5f;
					}
				}

				if ((double)Projectile.velocity.X > 0.5)
				{
					Projectile.spriteDirection = -1;
				}
				else if ((double)Projectile.velocity.X < -0.5)
				{
					Projectile.spriteDirection = 1;
				}

				Projectile.frameCounter++;
				if (Projectile.frameCounter > 1)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				if (Projectile.frame < 7 || Projectile.frame > 10)
				{
					Projectile.frame = 7;
				}
				Projectile.rotation = Projectile.velocity.X * 0.1f;

			}
			else
			{
                _ = Vector2.Zero;
                if (Projectile.ai[1] != 0f)
				{
					flag2 = false;
					flag3 = false;
				}
				Projectile.rotation = 0f;
				Projectile.tileCollide = true;
				float num160 = 0.08f;
				float num161 = 8f;
				if (flag7)
				{
					num161 = 6f;
				}
				if (flag2)
				{
					if ((double)Projectile.velocity.X > -3.5)
					{
						Projectile.velocity.X -= num160;
					}
					else
					{
						Projectile.velocity.X -= num160 * 0.25f;
					}
				}
				else if (flag3)
				{
					if ((double)Projectile.velocity.X < 3.5)
					{
						Projectile.velocity.X += num160;
					}
					else
					{
						Projectile.velocity.X += num160 * 0.25f;
					}
				}
				else
				{
					Projectile.velocity.X *= 0.9f;
					if (Projectile.velocity.X >= 0f - num160 && Projectile.velocity.X <= num160)
					{
						Projectile.velocity.X = 0f;
					}
				}
				if (flag2 || flag3)
				{
					int num162 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
					int j2 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
					if (flag2)
					{
						num162--;
					}
					if (flag3)
					{
						num162++;
					}
					num162 += (int)Projectile.velocity.X;
					if (WorldGen.SolidTile(num162, j2))
					{
						flag5 = true;
					}
				}
				if (Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height - 8f > Projectile.position.Y + (float)Projectile.height)
				{
					flag4 = true;
				}
				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
				if (Projectile.velocity.Y == 0f)
				{
					if (!flag4 && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
					{
						int num163 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
						int j3 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16 + 1;
						if (flag2)
						{
							num163--;
						}
						if (flag3)
						{
							num163++;
						}
						WorldGen.SolidTile(num163, j3);
					}
					if (flag5)
					{
						int num164 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
						int num165 = (int)(Projectile.position.Y + (float)Projectile.height) / 16;
						if (WorldGen.SolidTileAllowBottomSlope(num164, num165) || Main.tile[num164, num165].IsHalfBlock || Main.tile[num164, num165].Slope > 0)
						{
							try
							{
								num164 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
								num165 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
								if (flag2)
								{
									num164--;
								}
								if (flag3)
								{
									num164++;
								}
								num164 += (int)Projectile.velocity.X;
								if (!WorldGen.SolidTile(num164, num165 - 1) && !WorldGen.SolidTile(num164, num165 - 2))
								{
									Projectile.velocity.Y = -5.1f;
								}
								else if (!WorldGen.SolidTile(num164, num165 - 2))
								{
									Projectile.velocity.Y = -7.1f;
								}
								else if (WorldGen.SolidTile(num164, num165 - 5))
								{
									Projectile.velocity.Y = -11.1f;
								}
								else if (WorldGen.SolidTile(num164, num165 - 4))
								{
									Projectile.velocity.Y = -10.1f;
								}
								else
								{
									Projectile.velocity.Y = -9.1f;
								}
							}
							catch
							{
								Projectile.velocity.Y = -9.1f;
							}
						}
					}
				}
				if (Projectile.velocity.X > num161)
				{
					Projectile.velocity.X = num161;
				}
				if (Projectile.velocity.X < 0f - num161)
				{
					Projectile.velocity.X = 0f - num161;
				}
				if (Projectile.velocity.X < 0f)
				{
					Projectile.direction = -1;
					Projectile.spriteDirection = 1;
				}
				if (Projectile.velocity.X > 0f)
				{
					Projectile.direction = 1;
					Projectile.spriteDirection = -1;
				}
				if (Projectile.velocity.X > num160 && flag3)
				{
					Projectile.direction = 1;
					Projectile.spriteDirection = -1;
				}
				if (Projectile.velocity.X < 0f - num160 && flag2)
				{
					Projectile.direction = -1;
					Projectile.spriteDirection = 1;
				}
				bool flag15 = Projectile.position.X - Projectile.oldPosition.X == 0f;

				if (Projectile.velocity.Y == 0f)
				{
					if (flag15)
					{
						if (Projectile.frame > 0)
						{
							Projectile.frameCounter += 2;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame >= 7)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
					{
						Projectile.frameCounter += (int)Math.Abs((double)Projectile.velocity.X * 0.75);
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 6)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame >= 7 || Projectile.frame < 1)
						{
							Projectile.frame = 1;
						}
					}
					else if (Projectile.frame > 0)
					{
						Projectile.frameCounter += 2;
						if (Projectile.frameCounter > 6)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame >= 7)
						{
							Projectile.frame = 0;
						}
					}
					else
					{
						Projectile.frame = 0;
						Projectile.frameCounter = 0;
					}
				}
				else if (Projectile.velocity.Y < 0f)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = 2;
				}
				else if (Projectile.velocity.Y > 0f)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = 4;
				}
				Projectile.velocity.Y += 0.4f;
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
			}
		}

    }
}
