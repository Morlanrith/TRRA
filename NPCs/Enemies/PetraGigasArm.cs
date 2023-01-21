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
	public class PetraGigasArm : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/PetraGigasArm";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.SkeletronHand];
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
			NPC.width = 52;
			NPC.height = 52;
			NPC.aiStyle = -1;
			NPC.damage = 110;
			NPC.defense = 40;
			NPC.lifeMax = 4800;
			NPC.HitSound = SoundID.NPCHit20;
			NPC.DeathSound = SoundID.NPCDeath25;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.value = 0f;
			//Banner = NPC.type;
			//BannerItem = ItemType<CreepBanner>();
			AnimationType = NPCID.SkeletronHand;
			//AIType = NPCID.SkeletronHand;
			SpawnModBiomes = new int[] { GetInstance<ShatteredMoonFakeBiome>().Type };
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				// DUST + GORE HERE
			}
		}

		public override void AI()
        {
			//if (!TRRAWorld.IsShatteredMoon())
			//	NPC.EncourageDespawn(10);
			float parentState = Main.npc[(int)NPC.ai[1]].ai[0];

            NPC.spriteDirection = -(int)NPC.ai[0];
			// Automatically destroy the arm if the main enemy has been slain
			if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != NPCType<PetraGigas>())
			{
				if (Main.netMode != NetmodeID.Server)
				{
					NPC.life = -1;
					NPC.HitEffect();
					NPC.active = false;
				}
			}
			if (parentState == 0f || parentState == 3f)
			{
				if (Main.npc[(int)NPC.ai[1]].ai[1] == 3f)
				{
					NPC.EncourageDespawn(10);
				}
				if (Main.npc[(int)NPC.ai[1]].ai[1] != 0f)
				{
					if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y - 100f)
					{
						if (NPC.velocity.Y > 0f)
						{
							NPC.velocity.Y *= 0.96f;
						}
						NPC.velocity.Y -= 0.07f;
						if (NPC.velocity.Y > 6f)
						{
							NPC.velocity.Y = 6f;
						}
					}
					else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y - 100f)
					{
						if (NPC.velocity.Y < 0f)
						{
							NPC.velocity.Y *= 0.96f;
						}
						NPC.velocity.Y += 0.07f;
						if (NPC.velocity.Y < -6f)
						{
							NPC.velocity.Y = -6f;
						}
					}
					if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 120f * NPC.ai[0])
					{
						if (NPC.velocity.X > 0f)
						{
							NPC.velocity.X *= 0.96f;
						}
						NPC.velocity.X -= 0.1f;
						if (NPC.velocity.X > 8f)
						{
							NPC.velocity.X = 8f;
						}
					}
					if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 120f * NPC.ai[0])
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X *= 0.96f;
						}
						NPC.velocity.X += 0.1f;
						if (NPC.velocity.X < -8f)
						{
							NPC.velocity.X = -8f;
						}
					}
				}
				else
				{
					NPC.ai[3] += 1f;
					if (Main.expertMode)
					{
						NPC.ai[3] += 0.5f;
					}
					if (NPC.ai[3] >= 300f)
					{
						NPC.ai[3] = 0f;
						NPC.netUpdate = true;
					}
					if (Main.expertMode)
					{
						if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y + 230f)
						{
							if (NPC.velocity.Y > 0f)
							{
								NPC.velocity.Y *= 0.96f;
							}
							NPC.velocity.Y -= 0.04f;
							if (NPC.velocity.Y > 3f)
							{
								NPC.velocity.Y = 3f;
							}
						}
						else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y + 230f)
						{
							if (NPC.velocity.Y < 0f)
							{
								NPC.velocity.Y *= 0.96f;
							}
							NPC.velocity.Y += 0.04f;
							if (NPC.velocity.Y < -3f)
							{
								NPC.velocity.Y = -3f;
							}
						}
						if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0])
						{
							if (NPC.velocity.X > 0f)
							{
								NPC.velocity.X *= 0.96f;
							}
							NPC.velocity.X -= 0.07f;
							if (NPC.velocity.X > 8f)
							{
								NPC.velocity.X = 8f;
							}
						}
						if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0])
						{
							if (NPC.velocity.X < 0f)
							{
								NPC.velocity.X *= 0.96f;
							}
							NPC.velocity.X += 0.07f;
							if (NPC.velocity.X < -8f)
							{
								NPC.velocity.X = -8f;
							}
						}
					}
					if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y + 230f)
					{
						if (NPC.velocity.Y > 0f)
						{
							NPC.velocity.Y *= 0.96f;
						}
						NPC.velocity.Y -= 0.04f;
						if (NPC.velocity.Y > 3f)
						{
							NPC.velocity.Y = 3f;
						}
					}
					else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y + 230f)
					{
						if (NPC.velocity.Y < 0f)
						{
							NPC.velocity.Y *= 0.96f;
						}
						NPC.velocity.Y += 0.04f;
						if (NPC.velocity.Y < -3f)
						{
							NPC.velocity.Y = -3f;
						}
					}
					if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0])
					{
						if (NPC.velocity.X > 0f)
						{
							NPC.velocity.X *= 0.96f;
						}
						NPC.velocity.X -= 0.07f;
						if (NPC.velocity.X > 8f)
						{
							NPC.velocity.X = 8f;
						}
					}
					if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0])
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X *= 0.96f;
						}
						NPC.velocity.X += 0.07f;
						if (NPC.velocity.X < -8f)
						{
							NPC.velocity.X = -8f;
						}
					}
				}
				Vector2 vector22 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num182 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector22.X;
				float num183 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector22.Y;
				float num184 = (float)Math.Sqrt(num182 * num182 + num183 * num183);
				NPC.rotation = (float)Math.Atan2(num183, num182) + 1.57f;
			}
			else if (parentState == 1f)
			{

			}
            else if (parentState == 2f) // Spin around body
            {
				NPC parent = Main.npc[(int)NPC.ai[1]];
                float timer = parent.ai[1] - 60f;
				if(timer >= 0f && timer < 360f)
				{
                    Vector2 newPos = new(parent.Center.X, parent.Center.Y + (NPC.ai[0] == 1f ? 200 : -200));
					double angle = timer*3.0;

                    double radians = (Math.PI / 180) * angle;
                    double sin = Math.Sin(radians);
                    double cos = Math.Cos(radians);

                    // Translate point back to origin
                    newPos.X -= parent.Center.X;
                    newPos.Y -= parent.Center.Y;

                    // Rotate point
                    double xnew = newPos.X * cos - newPos.Y * sin;
                    double ynew = newPos.X * sin + newPos.Y * cos;

                    // Translate point back
                    newPos = new Vector2((int)xnew + parent.Center.X, (int)ynew + parent.Center.Y);

                    NPC.Center = newPos;
				}
            }
        }

    }
}
