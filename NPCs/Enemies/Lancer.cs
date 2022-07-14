using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Projectiles.NPCs.Enemies.Lancer;
using TRRA.Dusts;
using TRRA.Biomes;
using TRRA.Items.Placeable;

namespace TRRA.NPCs.Enemies
{
	public class Lancer : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/Lancer";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Hornet];
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f,
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 96;
			NPC.height = 122;
			NPC.aiStyle = -1;
			NPC.damage = 60;
			NPC.defense = 20;
			NPC.lifeMax = 400;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath35;
			NPC.knockBackResist = 0.5f;
			NPC.value = 1000f;
			NPC.noGravity = true;
			Banner = NPC.type;
			BannerItem = ItemType<LancerBanner>();
			AnimationType = NPCID.Hornet;
			SpawnModBiomes = new int[] { GetInstance<ShatteredMoonFakeBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TRRA.Bestiary.Lancer"),

			});
		}

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, damage);
			if(NPC.life <= 0)
            {
				for (int i = 0; i < 15; i++)
				{
					Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.velocity.X, NPC.velocity.Y)) * 32f;
					int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustType<GrimmParticle>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1f;
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Lancer_Head").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Lancer_Abdomen").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Lancer_Wing").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Lancer_Talon").Type);
			}
        }

		private void Flee()
        {
			Vector2 vector130 = new(NPC.Center.X + (float)(NPC.direction * 20), NPC.Center.Y + 6f);
			float num990 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector130.X;
			float num991 = Main.player[NPC.target].position.Y - vector130.Y;
			float num992 = (float)Math.Sqrt(num990 * num990 + num991 * num991);
			float num993 = 7f / num992;
			num990 *= num993;
			num991 *= num993;
			int num994 = 60;
			NPC.velocity.X = (NPC.velocity.X * (float)(num994 - 1) - num990) / (float)num994;
			NPC.velocity.Y = (NPC.velocity.Y * (float)(num994 - 1) - num991) / (float)num994;
			NPC.EncourageDespawn(10);
			if (NPC.velocity.X > 0f)
				NPC.spriteDirection = 1;
			if (NPC.velocity.X < 0f)
				NPC.spriteDirection = -1;
		}

        public override void AI()
        {
			if (!TRRAWorld.IsShatteredMoon())
			{
				Flee();
				return;
			}
			if (NPC.target < 0 || NPC.target <= 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest();
			}
			NPCAimedTarget targetData = NPC.GetTargetData();
			bool flag = false;
			if (targetData.Type == Terraria.Enums.NPCTargetType.Player)
			{
				flag = Main.player[NPC.target].dead;
			}
			float num = 4f;
			float num2 = 0.017f;
			Vector2 vector = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num4 = targetData.Position.X + (float)(targetData.Width / 2);
			float num5 = targetData.Position.Y + (float)(targetData.Height / 2);
			num4 = (int)(num4 / 8f) * 8;
			num5 = (int)(num5 / 8f) * 8;
			vector.X = (int)(vector.X / 8f) * 8;
			vector.Y = (int)(vector.Y / 8f) * 8;
			num4 -= vector.X;
			num5 -= vector.Y;
			float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
			float num7 = num6;
			if (num6 == 0f)
			{
				num4 = NPC.velocity.X;
				num5 = NPC.velocity.Y;
			}
			else
			{
				num6 = num / num6;
				num4 *= num6;
				num5 *= num6;
			}
			if (num7 > 100f)
			{
				NPC.ai[0] += 1f;
				if (NPC.ai[0] > 0f)
				{
					NPC.velocity.Y += 0.023f;
				}
				else
				{
					NPC.velocity.Y -= 0.023f;
				}
				if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
				{
					NPC.velocity.X += 0.023f;
				}
				else
				{
					NPC.velocity.X -= 0.023f;
				}
				if (NPC.ai[0] > 200f)
				{
					NPC.ai[0] = -200f;
				}
			}
			if (flag)
			{
				num4 = (float)NPC.direction * num / 2f;
				num5 = (0f - num) / 2f;
			}
			if (NPC.velocity.X < num4)
			{
				NPC.velocity.X += num2;
				if (NPC.velocity.X < 0f && num4 > 0f)
				{
					NPC.velocity.X += num2;
				}
			}
			else if (NPC.velocity.X > num4)
			{
				NPC.velocity.X -= num2;
				if (NPC.velocity.X > 0f && num4 < 0f)
				{
					NPC.velocity.X -= num2;
				}
			}
			if (NPC.velocity.Y < num5)
			{
				NPC.velocity.Y += num2;
				if (NPC.velocity.Y < 0f && num5 > 0f)
				{
					NPC.velocity.Y += num2;
				}
			}
			else if (NPC.velocity.Y > num5)
			{
				NPC.velocity.Y -= num2;
				if (NPC.velocity.Y > 0f && num5 < 0f)
				{
					NPC.velocity.Y -= num2;
				}
			}

			if (NPC.velocity.X > 0f)
			{
				NPC.spriteDirection = 1;
			}
			if (NPC.velocity.X < 0f)
			{
				NPC.spriteDirection = -1;
			}
			NPC.rotation = NPC.velocity.X * 0.1f;

			if (NPC.spriteDirection == -1) Lighting.AddLight(NPC.Left, 0.2f, 0f, 0f);
			else Lighting.AddLight(NPC.Right, 0.2f, 0f, 0f);

			float num12 = 0.7f;
			if (NPC.collideX)
			{
				NPC.netUpdate = true;
				NPC.velocity.X = NPC.oldVelocity.X * (0f - num12);
				if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
				{
					NPC.velocity.X = 2f;
				}
				if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
				{
					NPC.velocity.X = -2f;
				}
			}
			if (NPC.collideY)
			{
				NPC.netUpdate = true;
				NPC.velocity.Y = NPC.oldVelocity.Y * (0f - num12);
				if (NPC.velocity.Y > 0f && (double)NPC.velocity.Y < 1.5)
				{
					NPC.velocity.Y = 2f;
				}
				if (NPC.velocity.Y < 0f && (double)NPC.velocity.Y > -1.5)
				{
					NPC.velocity.Y = -2f;
				}
			}
			NPC.position += NPC.netOffset;
			NPC.position -= NPC.netOffset;
			if (NPC.wet)
			{
				if (NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y *= 0.95f;
				}
				NPC.velocity.Y -= 0.5f;
				if (NPC.velocity.Y < -4f)
				{
					NPC.velocity.Y = -4f;
				}
				NPC.TargetClosest();
			}
			if (NPC.ai[1] == 101f)
			{
				SoundEngine.PlaySound(SoundID.Item17, NPC.position);
				NPC.ai[1] = 0f;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.ai[1] += (float)Main.rand.Next(5, 20) * 0.1f * NPC.scale;
				NPC.ai[1] += (float)Main.rand.Next(5, 20) * 0.1f * NPC.scale;
				if (targetData.Type == NPCTargetType.Player)
				{
					Player player = Main.player[NPC.target];
					if (player != null && player.stealth == 0f && player.itemAnimation == 0)
					{
						NPC.ai[1] = 0f;
					}
				}
				if (NPC.ai[1] >= 130f)
				{
					if (targetData.Type != 0 && Collision.CanHit(NPC, targetData))
					{
						float num17 = 8f;
						Vector2 vector2 = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)(NPC.height / 2));
						float num18 = targetData.Center.X - vector2.X + (float)Main.rand.Next(-20, 21);
						float num19 = targetData.Center.Y - vector2.Y + (float)Main.rand.Next(-20, 21);
						if ((num18 < 0f && NPC.velocity.X < 0f) || (num18 > 0f && NPC.velocity.X > 0f))
						{
							float num20 = (float)Math.Sqrt(num18 * num18 + num19 * num19);
							num20 = num17 / num20;
							num18 *= num20;
							num19 *= num20;
							int num23 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector2.X, vector2.Y, num18, num19, ProjectileType<LancerStinger>(), NPC.damage, 0f, Main.myPlayer);
							Main.projectile[num23].timeLeft = 300;
							NPC.ai[1] = 101f;
							NPC.netUpdate = true;
						}
						else
						{
							NPC.ai[1] = 0f;
						}
					}
					else
					{
						NPC.ai[1] = 0f;
					}
				}
			}
			if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
			{
				NPC.netUpdate = true;
			}
		}
    }
}
