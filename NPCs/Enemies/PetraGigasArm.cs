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
			if (!TRRAWorld.IsShatteredMoon())
				NPC.EncourageDespawn(10);

			NPC.spriteDirection = -(int)NPC.ai[0];
			if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != NPCType<PetraGigas>())
			{
				NPC.ai[2] += 10f;
				if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server)
				{
					NPC.life = -1;
					NPC.HitEffect();
					NPC.active = false;
				}
			}
			if (NPC.ai[2] == 0f || NPC.ai[2] == 3f)
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
						NPC.ai[2] += 1f;
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
			else if (NPC.ai[2] == 1f)
			{
				Vector2 vector23 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num185 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector23.X;
				float num186 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector23.Y;
				float num187 = (float)Math.Sqrt(num185 * num185 + num186 * num186);
				NPC.rotation = (float)Math.Atan2(num186, num185) + 1.57f;
				NPC.velocity.X *= 0.95f;
				NPC.velocity.Y -= 0.1f;
				if (Main.expertMode)
				{
					NPC.velocity.Y -= 0.06f;
					if (NPC.velocity.Y < -13f)
					{
						NPC.velocity.Y = -13f;
					}
				}
				else if (NPC.velocity.Y < -8f)
				{
					NPC.velocity.Y = -8f;
				}
				if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y - 200f)
				{
					NPC.TargetClosest();
					NPC.ai[2] = 2f;
					vector23 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					num185 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector23.X;
					num186 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector23.Y;
					num187 = (float)Math.Sqrt(num185 * num185 + num186 * num186);
					num187 = ((!Main.expertMode) ? (18f / num187) : (21f / num187));
					NPC.velocity.X = num185 * num187;
					NPC.velocity.Y = num186 * num187;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[2] == 2f)
			{
				if (NPC.position.Y > Main.player[NPC.target].position.Y || NPC.velocity.Y < 0f)
				{
					NPC.ai[2] = 3f;
				}
			}
			else if (NPC.ai[2] == 4f)
			{
				Vector2 vector24 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num188 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector24.X;
				float num189 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector24.Y;
				float num190 = (float)Math.Sqrt(num188 * num188 + num189 * num189);
				NPC.rotation = (float)Math.Atan2(num189, num188) + 1.57f;
				NPC.velocity.Y *= 0.95f;
				NPC.velocity.X += 0.1f * (0f - NPC.ai[0]);
				if (Main.expertMode)
				{
					NPC.velocity.X += 0.07f * (0f - NPC.ai[0]);
					if (NPC.velocity.X < -12f)
					{
						NPC.velocity.X = -12f;
					}
					else if (NPC.velocity.X > 12f)
					{
						NPC.velocity.X = 12f;
					}
				}
				else if (NPC.velocity.X < -8f)
				{
					NPC.velocity.X = -8f;
				}
				else if (NPC.velocity.X > 8f)
				{
					NPC.velocity.X = 8f;
				}
				if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 500f || NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + 500f)
				{
					NPC.TargetClosest();
					NPC.ai[2] = 5f;
					vector24 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					num188 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector24.X;
					num189 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector24.Y;
					num190 = (float)Math.Sqrt(num188 * num188 + num189 * num189);
					num190 = ((!Main.expertMode) ? (17f / num190) : (22f / num190));
					NPC.velocity.X = num188 * num190;
					NPC.velocity.Y = num189 * num190;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[2] == 5f && ((NPC.velocity.X > 0f && NPC.position.X + (float)(NPC.width / 2) > Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2)) || (NPC.velocity.X < 0f && NPC.position.X + (float)(NPC.width / 2) < Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))))
			{
				NPC.ai[2] = 0f;
			}
			return;
		}

    }
}
