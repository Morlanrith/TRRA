using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Dusts;
using TRRA.Biomes;
using TRRA.Items.Placeable;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using TRRA.Items.Consumables;
using TRRA.Items.Materials;

namespace TRRA.NPCs.Enemies
{
	public class PetraGigas : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/PetraGigas";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.MourningWood];
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;

			NPCDebuffImmunityData debuffData = new()
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
					BuffID.Bleeding,
					BuffID.Venom,
					BuffID.Ichor,
					BuffID.Confused
				}
			};

			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}

		public override void SetDefaults() {
			NPC.width = 164;
			NPC.height = 154;
			NPC.aiStyle = -1;
			NPC.damage = 140;
			NPC.defense = 60;
			NPC.lifeMax = 24000;
			NPC.HitSound = SoundID.NPCHit20;
			NPC.DeathSound = SoundID.NPCDeath25;
			NPC.knockBackResist = 0f;
			NPC.value = 1000f;
			NPC.npcSlots = 4f;
			//Banner = NPC.type;
			//BannerItem = ItemType<CreepBanner>();
			AnimationType = NPCID.MourningWood;
			//AIType = NPCID.MourningWood;
			SpawnModBiomes = new int[] { GetInstance<ShatteredMoonFakeBiome>().Type };
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			IItemDropRule rule = ItemDropRule.Common(ItemType<EssenceOfGrimm>(), 1);
			npcLoot.Add(rule);
			rule = ItemDropRule.Common(ItemType<MoonSummoner>(), 100);
			npcLoot.Add(rule);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TRRA.Bestiary.Creep"), // CHANGE LATER

			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				/*for (int i = 0; i < 10; i++)
				{
					Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.NPC.velocity.X, NPC.NPC.velocity.Y)) * 32f;
					int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustType<GrimmParticle>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].NPC.velocity *= 1f;
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.NPC.velocity, Mod.Find<ModGore>("Creep_Head").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.NPC.velocity, Mod.Find<ModGore>("Creep_Torso").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.NPC.velocity, Mod.Find<ModGore>("Creep_Tail").Type);*/
			}
		}

        public override void OnSpawn(IEntitySource source)
        {
			NPC.TargetClosest();
			int num156 = NPC.NewNPC(source, (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, NPCType<PetraGigasArm>(), NPC.whoAmI);
			Main.npc[num156].ai[0] = -1f;
			Main.npc[num156].ai[1] = NPC.whoAmI;
			Main.npc[num156].target = NPC.target;
			Main.npc[num156].netUpdate = true;
			num156 = NPC.NewNPC(source, (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, NPCType<PetraGigasArm>(), NPC.whoAmI);
			Main.npc[num156].ai[0] = 1f;
			Main.npc[num156].ai[1] = NPC.whoAmI;
			Main.npc[num156].ai[3] = 150f;
			Main.npc[num156].target = NPC.target;
			Main.npc[num156].netUpdate = true;
		}

        public override void AI()
        {
			//if (!TRRAWorld.IsShatteredMoon())
			//	NPC.EncourageDespawn(10);

			float speedAdjustment = 2f;

			NPC.noGravity = true;
			NPC.noTileCollide = true;

			if (!Main.dayTime)
			{
				NPC.TargetClosest(); // Repeatedly target the nearest player while it is nighttime
			}

			bool stopMoving = false;

			if ((double)NPC.life < (double)NPC.lifeMax * 0.75)
			{
				speedAdjustment = 3f;
			}
			if ((double)NPC.life < (double)NPC.lifeMax * 0.5)
			{
				speedAdjustment = 4f;
			}

			//Lighting.AddLight(NPC.Bottom + new Vector2(0f, -30f), 0.3f, 0.125f, 0.06f); // Mourning wood ambient light

			// ATTACKING
			if (Main.dayTime)
			{
				NPC.EncourageDespawn(10);
				speedAdjustment = 8f;
			}
			else if (NPC.ai[0] == 0f)
			{
				NPC.ai[1] += 1f; // AI[1] is a timer that controls when the next attack will occur (when it reaches 300)
				if ((double)NPC.life < (double)NPC.lifeMax * 0.5)
				{
					NPC.ai[1] += 1f; // If below half health, add an extra tick to the timer (double the standard rate)
				}
				if ((double)NPC.life < (double)NPC.lifeMax * 0.25)
				{
					NPC.ai[1] += 1f; // If below a quarter health, add another extra tick to the timer (triple the standard rate)
				}
				// If the timer is at 300
				if (NPC.ai[1] >= 300f && Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[1] = 0f; // Reset the timer
					// Choose a random attack, with different variants selected if below a quarter health
					if ((double)NPC.life < (double)NPC.lifeMax * 0.25)
					{
						NPC.ai[0] = Main.rand.Next(3, 5);
					}
					else
					{
						NPC.ai[0] = Main.rand.Next(1, 3);
					}
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 1f) // Attack 1 - Regular Version
			{
				stopMoving = true; // Don't move when attacking (regular only)
				NPC.ai[1] += 1f; // AI[1] is a timer that controls when the attack will stop (when it reaches 120)
				if (NPC.ai[1] % 15f == 0f) // Creates a projectile every 15 ticks of the timer
				{
					Vector2 vector109 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f + 30f);
					float num878 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector109.X;
					float num879 = Main.player[NPC.target].position.Y - vector109.Y;
					float num880 = (float)Math.Sqrt(num878 * num878 + num879 * num879);
					float num881 = 10f;
					num880 = num881 / num880;
					num878 *= num880;
					num879 *= num880;
					num878 *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
					num879 *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
					int num882 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector109.X, vector109.Y, num878, num879, ProjectileID.FlamingWood, 50, 0f, Main.myPlayer);
				}
				if (NPC.ai[1] >= 120f) // Stop attacking and reset to netural state
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
				}
			}
			else if (NPC.ai[0] == 2f) // Attack 2 - Regular Version
			{
				stopMoving = true; // Don't move when attacking (regular only)
				NPC.ai[1] += 1f; // AI[1] is a timer that controls when the attack will stop (when it reaches 300)
				if (NPC.ai[1] > 60f && NPC.ai[1] < 240f && NPC.ai[1] % 8f == 0f) // Creates a projectile every 8 ticks of the timer, between the values of 60 and 240
				{
					float num888 = 10f;
					Vector2 vector111 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f + 30f);
					float num889 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector111.X;
					float num890 = Main.player[NPC.target].position.Y - vector111.Y;
					num890 -= Math.Abs(num889) * 0.3f;
					num888 += Math.Abs(num889) * 0.004f;
					if (num888 > 14f)
					{
						num888 = 14f;
					}
					num889 += (float)Main.rand.Next(-50, 51);
					num890 -= (float)Main.rand.Next(50, 201);
					float num891 = (float)Math.Sqrt(num889 * num889 + num890 * num890);
					num891 = num888 / num891;
					num889 *= num891;
					num890 *= num891;
					num889 *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
					num890 *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
					int num892 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector111.X, vector111.Y, num889, num890, Main.rand.Next(326, 329), 40, 0f, Main.myPlayer); // Greek Fire
				}
				if (NPC.ai[1] >= 300f) // Stop attacking and reset to netural state
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
				}
			}
			else if (NPC.ai[0] == 3f) // Attack 1 - Low health Version
			{
				speedAdjustment = 4f;
				NPC.ai[1] += 1f; // AI[1] is a timer that controls when the attack will stop (when it reaches 120)
				if (NPC.ai[1] % 30f == 0f) // Creates a projectile every 30 ticks of the timer
				{
					Vector2 vector112 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f + 30f);
					float num893 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector112.X;
					float num894 = Main.player[NPC.target].position.Y - vector112.Y;
					float num895 = (float)Math.Sqrt(num893 * num893 + num894 * num894);
					float num896 = 16f;
					num895 = num896 / num895;
					num893 *= num895;
					num894 *= num895;
					num893 *= 1f + (float)Main.rand.Next(-20, 21) * 0.001f;
					num894 *= 1f + (float)Main.rand.Next(-20, 21) * 0.001f;
					int num897 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector112.X, vector112.Y, num893, num894, ProjectileID.FlamingWood, 75, 0f, Main.myPlayer);
				}
				if (NPC.ai[1] >= 120f) // Stop attacking and reset to netural state
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
				}
			}
			else if (NPC.ai[0] == 4f) // Attack 2 - Low health Version
			{
				speedAdjustment = 4f;
				NPC.ai[1] += 1f; // AI[1] is a timer that controls when the attack will stop (when it reaches 240)
				if (NPC.ai[1] % 10f == 0f) // Creates a projectile every 10 ticks of the timer
				{
					float num898 = 12f;
					Vector2 vector113 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f + 30f);
					float num899 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector113.X;
					float num900 = Main.player[NPC.target].position.Y - vector113.Y;
					num900 -= Math.Abs(num899) * 0.2f;
					num898 += Math.Abs(num899) * 0.002f;
					if (num898 > 16f)
					{
						num898 = 16f;
					}
					num899 += (float)Main.rand.Next(-50, 51);
					num900 -= (float)Main.rand.Next(50, 201);
					float num901 = (float)Math.Sqrt(num899 * num899 + num900 * num900);
					num901 = num898 / num901;
					num899 *= num901;
					num900 *= num901;
					num899 *= 1f + (float)Main.rand.Next(-30, 31) * 0.005f;
					num900 *= 1f + (float)Main.rand.Next(-30, 31) * 0.005f;
					int num902 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector113.X, vector113.Y, num899, num900, Main.rand.Next(326, 329), 50, 0f, Main.myPlayer); // Greek Fire
				}
				if (NPC.ai[1] >= 240f) // Stop attacking and reset to netural state
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
				}
			}

			// HORIZONTAL MOVEMENT
			if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < 50f)
			{
				stopMoving = true;
			}
			if (stopMoving)
			{
				NPC.velocity.X *= 0.9f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
				{
					NPC.velocity.X = 0f;
				}
			}
			else
			{
				if (NPC.direction > 0)
				{
					NPC.velocity.X = (NPC.velocity.X * 20f + speedAdjustment) / 21f;
				}
				if (NPC.direction < 0)
				{
					NPC.velocity.X = (NPC.velocity.X * 20f - speedAdjustment) / 21f;
				}
			}

			// PHASE THROUGH WALLS
			int num903 = 80;
			int num904 = 20;
			Vector2 vector114 = new Vector2(NPC.Center.X - (float)(num903 / 2), NPC.position.Y + (float)NPC.height - (float)num904);
			bool flag51 = false;
			if (NPC.position.X < Main.player[NPC.target].position.X && NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height - 16f)
			{
				flag51 = true;
			}
			if (flag51)
			{
				NPC.velocity.Y += 0.5f;
			}
			else if (Collision.SolidCollision(vector114, num903, num904))
			{
				if (NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y = 0f;
				}
				if ((double)NPC.velocity.Y > -0.2)
				{
					NPC.velocity.Y -= 0.025f;
				}
				else
				{
					NPC.velocity.Y -= 0.2f;
				}
				if (NPC.velocity.Y < -4f)
				{
					NPC.velocity.Y = -4f;
				}
			}
			else
			{
				if (NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y = 0f;
				}
				if ((double)NPC.velocity.Y < 0.1)
				{
					NPC.velocity.Y += 0.025f;
				}
				else
				{
					NPC.velocity.Y += 0.5f;
				}
			}
			if (NPC.velocity.Y > 10f)
			{
				NPC.velocity.Y = 10f;
			}
		}

    }
}
