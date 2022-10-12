using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Dusts;
using TRRA.Items.Consumables;
using TRRA.Items.Materials;
using Terraria.DataStructures;
using TRRA.Biomes;
using System;
using Terraria.Audio;

namespace TRRA.NPCs.Enemies
{
	public class ApatheticDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apathetic");
			Description.SetDefault("You feel tired...");

			Main.debuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			// Cuts speed
			player.moveSpeed /= 2f;
			if (player.velocity.Y == 0f && Math.Abs(player.velocity.X) > 1f)
			{
				player.velocity.X /= 2f;
			}
			// Cuts damage
			player.GetDamage(DamageClass.Melee) *= 0.34f;
			player.GetDamage(DamageClass.Magic) *= 0.34f;
			player.GetDamage(DamageClass.Summon) *= 0.34f;
			player.GetDamage(DamageClass.Ranged) *= 0.34f;
		}
	}

	public class Apathy : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/Apathy";

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Apathy");

			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Medusa];

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

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f,
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 40;
			NPC.damage = 14;
			NPC.defense = 6;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 1000f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;

			AnimationType = NPCID.Medusa;
			SpawnModBiomes = new int[] { GetInstance<ShatteredMoonFakeBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TRRA.Bestiary.Beowolf"),

			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 15; i++)
				{
					Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.velocity.X, NPC.velocity.Y)) * 32f;
					int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustType<GrimmParticle>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1f;
				}
				//Gore.NewGore(NPC.GetSource_Death(), NPC.NPC.position, NPC.NPC.velocity, Mod.Find<ModGore>("Beowolf_Head").Type);
				//Gore.NewGore(NPC.GetSource_Death(), NPC.NPC.position, NPC.NPC.velocity, Mod.Find<ModGore>("Beowolf_Torso").Type);
				//Gore.NewGore(NPC.GetSource_Death(), NPC.NPC.position, NPC.NPC.velocity, Mod.Find<ModGore>("Beowolf_Tail").Type);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			IItemDropRule rule = ItemDropRule.Common(ItemType<EssenceOfGrimm>(), 5);
			npcLoot.Add(rule);
			rule = ItemDropRule.Common(ItemType<MoonSummoner>(), 100);
			npcLoot.Add(rule);
		}

		// Adapated from the AI for the existing Medusa enemy
        public override void AI()
		{
			if (!TRRAWorld.IsShatteredMoon())
				NPC.EncourageDespawn(10);

			if (Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height == NPC.position.Y + (float)NPC.height)
			{
				NPC.directionY = -1;
			}
			bool flag = false;
			int num20 = 180;
			int num21 = 300;
			int num22 = 180;
			int num24 = 20;
			if (NPC.life < NPC.lifeMax / 3)
			{
				num20 = 120;
				num21 = 240;
				num22 = 240;
			}
			if (NPC.ai[2] > 0f)
			{
				NPC.ai[2] -= 1f;
			}
			else if (NPC.ai[2] == 0f)
			{
				if (((Main.player[NPC.target].Center.X < NPC.Center.X) || (Main.player[NPC.target].Center.X > NPC.Center.X)) && NPC.velocity.Y == 0f && NPC.Distance(Main.player[NPC.target].Center) < 350f)
				{
					NPC.ai[2] = -num22 - num24;
					NPC.netUpdate = true;
				}
			}
			else
			{
				if (NPC.ai[2] < 0f && NPC.ai[2] < (float)(-num22))
				{
					NPC.position += NPC.netOffset;
					NPC.velocity.X *= 0.9f;
					if (NPC.velocity.Y < -2f || NPC.velocity.Y > 4f || NPC.justHit)
					{
						NPC.ai[2] = num20;
					}
					else
					{
						NPC.ai[2] += 1f;
						if (NPC.ai[2] == 0f)
						{
							NPC.ai[2] = num21;
						}
					}
					float num25 = NPC.ai[2] + (float)num22 + (float)num24;
					if (num25 == 1f)
					{
						//SoundEngine.PlaySound(4, (int)NPC.position.X, (int)NPC.position.Y, 17);
					}
					if (num25 < (float)num24)
					{
						Vector2 vector6 = NPC.Top + new Vector2(NPC.spriteDirection * 6, 6f);
						float num26 = MathHelper.Lerp(20f, 30f, (num25 * 3f + 50f) / 182f);
						Main.rand.NextFloat();
						for (float num27 = 0f; num27 < 2f; num27 += 1f)
						{
							Vector2 vector7 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (Main.rand.NextFloat() * 0.5f + 0.5f);
							Dust obj = Main.dust[Dust.NewDust(vector6, 0, 0, DustID.GoldFlame)];
							obj.position = vector6 + vector7 * num26;
							obj.noGravity = true;
							obj.velocity = vector7 * 2f;
							obj.scale = 0.5f + Main.rand.NextFloat() * 0.5f;
						}
					}
					Lighting.AddLight(NPC.Center, 0.9f, 0.75f, 0.1f);
					NPC.position -= NPC.netOffset;
					return;
				}
				if (NPC.ai[2] < 0f && NPC.ai[2] >= (float)(-num22))
				{
					NPC.position += NPC.netOffset;
					Lighting.AddLight(NPC.Center, 0.9f, 0.75f, 0.1f);
					NPC.velocity.X *= 0.9f;
					if (NPC.velocity.Y < -2f || NPC.velocity.Y > 4f || NPC.justHit)
					{
						NPC.ai[2] = num20;
					}
					else
					{
						NPC.ai[2] += 1f;
						if (NPC.ai[2] == 0f)
						{
							NPC.ai[2] = num21;
						}
					}
					float num28 = NPC.ai[2] + (float)num22;
					if (num28 < 180f && (Main.rand.NextBool(3)|| NPC.ai[2] % 3f == 0f))
					{
						Vector2 vector8 = NPC.Top + new Vector2(NPC.spriteDirection * 10, 10f);
						float num29 = MathHelper.Lerp(20f, 30f, (num28 * 3f + 50f) / 182f);
						Main.rand.NextFloat();
						for (float num30 = 0f; num30 < 1f; num30 += 1f)
						{
							Vector2 vector9 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (Main.rand.NextFloat() * 0.5f + 0.5f);
							Dust obj2 = Main.dust[Dust.NewDust(vector8, 0, 0, DustID.GoldFlame)];
							obj2.position = vector8 + vector9 * num29;
							obj2.noGravity = true;
							obj2.velocity = vector9 * 4f;
							obj2.scale = 0.5f + Main.rand.NextFloat();
						}
					}
					NPC.position -= NPC.netOffset;
					if (Main.netMode == NetmodeID.Server)
					{
						return;
					}
					Player player2 = Main.player[Main.myPlayer];
					_ = Main.myPlayer;
					if (player2.dead || !player2.active || player2.HasBuff(BuffType<ApatheticDebuff>())) // Do nothing if player is dead, inactive, or already has the debuff
					{
						return;
					}
					Vector2 vector10 = player2.Center - NPC.Center;
					if (!(vector10.Length() < 300f))
					{
						return;
					}
					if (!player2.creativeGodMode)
					{
						player2.AddBuff(BuffType<ApatheticDebuff>(), (Main.expertMode ? 1380 : 600) + (int)NPC.ai[2] * -1);
					}
					return;
				}
			}


			bool flag5 = false;
			bool flag6 = false;
			if (NPC.velocity.X == 0f)
			{
				flag6 = true;
			}
			if (NPC.justHit)
			{
				flag6 = false;
			}
			int num55 = 60;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = true;
			if (!flag9 && flag10)
			{
				if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
				{
					flag7 = true;
				}
				if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= (float)num55 || flag7)
				{
					NPC.ai[3] += 1f;
				}
				else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
				{
					NPC.ai[3] -= 1f;
				}
				if (NPC.ai[3] > (float)(num55 * 10))
				{
					NPC.ai[3] = 0f;
				}
				if (NPC.justHit)
				{
					NPC.ai[3] = 0f;
				}
				if (NPC.ai[3] == (float)num55)
				{
					NPC.netUpdate = true;
				}
				if (Main.player[NPC.target].Hitbox.Intersects(NPC.Hitbox))
				{
					NPC.ai[3] = 0f;
				}
			}
			if (NPC.ai[3] < (float)num55 && NPC.DespawnEncouragement_AIStyle3_Fighters_NotDiscouraged(NPC.type, NPC.position, NPC))
			{
				NPC.TargetClosest();
				if (NPC.directionY > 0 && Main.player[NPC.target].Center.Y <= NPC.Bottom.Y)
				{
					NPC.directionY = -1;
				}
			}
			else if (!(NPC.ai[2] > 0f) || !NPC.DespawnEncouragement_AIStyle3_Fighters_CanBeBusyWithAction(NPC.type))
			{
				if (Main.dayTime && (double)(NPC.position.Y / 16f) < Main.worldSurface)
				{
					NPC.EncourageDespawn(10);
				}
				if (NPC.velocity.X == 0f)
				{
					if (NPC.velocity.Y == 0f)
					{
						NPC.ai[0] += 1f;
						if (NPC.ai[0] >= 2f)
						{
							NPC.direction *= -1;
							NPC.spriteDirection = NPC.direction;
							NPC.ai[0] = 0f;
						}
					}
				}
				else
				{
					NPC.ai[0] = 0f;
				}
				if (NPC.direction == 0)
				{
					NPC.direction = 1;
				}
			}
			float num79 = 0.75f;
			if (NPC.velocity.X < 0f - num79 || NPC.velocity.X > num79)
			{
				if (NPC.velocity.Y == 0f)
				{
					NPC.velocity *= 0.8f;
				}
			}
			else if (NPC.velocity.X < num79 && NPC.direction == 1)
			{
				NPC.velocity.X += 0.07f;
				if (NPC.velocity.X > num79)
				{
					NPC.velocity.X = num79;
				}
			}
			else if (NPC.velocity.X > 0f - num79 && NPC.direction == -1)
			{
				NPC.velocity.X -= 0.07f;
				if (NPC.velocity.X < 0f - num79)
				{
					NPC.velocity.X = 0f - num79;
				}
			}
			float num102 = 1f;
			if (NPC.velocity.X < 0f - num102 || NPC.velocity.X > num102)
			{
				if (NPC.velocity.Y == 0f)
				{
					NPC.velocity *= 0.8f;
				}
			}
			else if (NPC.velocity.X < num102 && NPC.direction == 1)
			{
				NPC.velocity.X += 0.07f;
				if (NPC.velocity.X > num102)
				{
					NPC.velocity.X = num102;
				}
			}
			else if (NPC.velocity.X > 0f - num102 && NPC.direction == -1)
			{
				NPC.velocity.X -= 0.07f;
				if (NPC.velocity.X < 0f - num102)
				{
					NPC.velocity.X = 0f - num102;
				}
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (NPC.velocity.Y == 0f)
				{
					int num117 = -1;
					if (num117 != -1 && NPC.NPCCanStickToWalls())
					{
						NPC.Transform(num117);
					}
				}
			}
			if (NPC.velocity.Y == 0f || flag)
			{
				int num167 = (int)(NPC.position.Y + (float)NPC.height + 7f) / 16;
				int num168 = (int)(NPC.position.Y - 9f) / 16;
				int num169 = (int)NPC.position.X / 16;
				int num170 = (int)(NPC.position.X + (float)NPC.width) / 16;
				int num171 = (int)(NPC.position.X + 8f) / 16;
				int num172 = (int)(NPC.position.X + (float)NPC.width - 8f) / 16;
				bool flag20 = false;
				for (int num173 = num171; num173 <= num172; num173++)
				{
					if (num173 >= num169 && num173 <= num170 && Main.tile[num173, num167] == null)
					{
						flag20 = true;
						continue;
					}
					if (Main.tile[num173, num168] != null && Main.tile[num173, num168].HasUnactuatedTile && Main.tileSolid[Main.tile[num173, num168].TileType])
					{
						flag5 = false;
						break;
					}
					if (!flag20 && num173 >= num169 && num173 <= num170 && Main.tile[num173, num167].HasUnactuatedTile && Main.tileSolid[Main.tile[num173, num167].TileType])
					{
						flag5 = true;
					}
				}
				if (!flag5 && NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y = 0f;
				}
				if (flag20)
				{
					return;
				}
			}
			if (NPC.velocity.Y >= 0f && NPC.directionY != 1)
			{
				int num174 = 0;
				if (NPC.velocity.X < 0f)
				{
					num174 = -1;
				}
				if (NPC.velocity.X > 0f)
				{
					num174 = 1;
				}
				Vector2 vector37 = NPC.position;
				vector37.X += NPC.velocity.X;
				int num175 = (int)((vector37.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 1) * num174)) / 16f);
				int num176 = (int)((vector37.Y + (float)NPC.height - 1f) / 16f);
				if (WorldGen.InWorld(num175, num176, 4))
				{
					if (Main.tile[num175, num176] == null)
					{
						Main.tile[num175, num176].ClearTile();
					}
					if (Main.tile[num175, num176 - 1] == null)
					{
						Main.tile[num175, num176 - 1].ClearTile();
					}
					if (Main.tile[num175, num176 - 2] == null)
					{
						Main.tile[num175, num176 - 2].ClearTile();
					}
					if (Main.tile[num175, num176 - 3] == null)
					{
						Main.tile[num175, num176 - 3].ClearTile();
					}
					if (Main.tile[num175, num176 + 1] == null)
					{
						Main.tile[num175, num176 + 1].ClearTile();
					}
					if (Main.tile[num175 - num174, num176 - 3] == null)
					{
						Main.tile[num175 - num174, num176 - 3].ClearTile();
					}
					if ((float)(num175 * 16) < vector37.X + (float)NPC.width && (float)(num175 * 16 + 16) > vector37.X && ((Main.tile[num175, num176].HasUnactuatedTile && !Main.tile[num175, num176].TopSlope && !Main.tile[num175, num176 - 1].TopSlope && Main.tileSolid[Main.tile[num175, num176].TileType] && !Main.tileSolidTop[Main.tile[num175, num176].TileType]) || (Main.tile[num175, num176 - 1].IsHalfBlock && Main.tile[num175, num176 - 1].HasUnactuatedTile)) && (!Main.tile[num175, num176 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 1].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 1].TileType] || (Main.tile[num175, num176 - 1].IsHalfBlock && (!Main.tile[num175, num176 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 4].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 4].TileType]))) && (!Main.tile[num175, num176 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 2].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 2].TileType]) && (!Main.tile[num175, num176 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 3].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 3].TileType]) && (!Main.tile[num175 - num174, num176 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175 - num174, num176 - 3].TileType]))
					{
						float num177 = num176 * 16;
						if (Main.tile[num175, num176].IsHalfBlock)
						{
							num177 += 8f;
						}
						if (Main.tile[num175, num176 - 1].IsHalfBlock)
						{
							num177 -= 8f;
						}
						if (num177 < vector37.Y + (float)NPC.height)
						{
							float num178 = vector37.Y + (float)NPC.height - num177;
							float num179 = 16.1f;
							if (num178 <= num179)
							{
								NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num177;
								NPC.position.Y = num177 - (float)NPC.height;
								if (num178 < 9f)
								{
									NPC.stepSpeed = 1f;
								}
								else
								{
									NPC.stepSpeed = 2f;
								}
							}
						}
					}
				}
			}
			if (flag5)
			{
				int num180 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
				int num181 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);
				if (Main.tile[num180, num181] == null)
				{
					Main.tile[num180, num181].ClearTile();
				}
				if (Main.tile[num180, num181 - 1] == null)
				{
					Main.tile[num180, num181 - 1].ClearTile();
				}
				if (Main.tile[num180, num181 - 2] == null)
				{
					Main.tile[num180, num181 - 2].ClearTile();
				}
				if (Main.tile[num180, num181 - 3] == null)
				{
					Main.tile[num180, num181 - 3].ClearTile();
				}
				if (Main.tile[num180, num181 + 1] == null)
				{
					Main.tile[num180, num181 + 1].ClearTile();
				}
				if (Main.tile[num180 + NPC.direction, num181 - 1] == null)
				{
					Main.tile[num180 + NPC.direction, num181 - 1].ClearTile();
				}
				if (Main.tile[num180 + NPC.direction, num181 + 1] == null)
				{
					Main.tile[num180 + NPC.direction, num181 + 1].ClearTile();
				}
				if (Main.tile[num180 - NPC.direction, num181 + 1] == null)
				{
					Main.tile[num180 - NPC.direction, num181 + 1].ClearTile();
				}
				if (Main.tile[num180, num181 - 1].HasUnactuatedTile && (Main.tile[num180, num181 - 1].TileType == 10 || Main.tile[num180, num181 - 1].TileType == 388) && flag8)
				{
					NPC.ai[2] += 1f;
					NPC.ai[3] = 0f;
					if (NPC.ai[2] >= 60f)
					{
						bool flag21 = false;
						bool flag22 = Main.player[NPC.target].ZoneGraveyard && Main.rand.NextBool(60);
						if ((!Main.bloodMoon || Main.getGoodWorld) && !flag22 && flag21)
						{
							NPC.ai[1] = 0f;
						}
						NPC.velocity.X = 0.5f * (float)(-NPC.direction);
						int num182 = 5;
						if (Main.tile[num180, num181 - 1].TileType == 388)
						{
							num182 = 2;
						}
						NPC.ai[1] += num182;
						NPC.ai[2] = 0f;
						bool flag23 = false;
						if (NPC.ai[1] >= 10f)
						{
							flag23 = true;
							NPC.ai[1] = 10f;
						}
						WorldGen.KillTile(num180, num181 - 1, fail: true);
						if ((Main.netMode != NetmodeID.MultiplayerClient || !flag23) && flag23 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (Main.tile[num180, num181 - 1].TileType == 10)
							{
								bool flag24 = WorldGen.OpenDoor(num180, num181 - 1, NPC.direction);
								if (!flag24)
								{
									NPC.ai[3] = num55;
									NPC.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag24)
								{
									NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num180, num181 - 1, NPC.direction);
								}
							}
							if (Main.tile[num180, num181 - 1].TileType == 388)
							{
								bool flag25 = WorldGen.ShiftTallGate(num180, num181 - 1, closing: false);
								if (!flag25)
								{
									NPC.ai[3] = num55;
									NPC.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag25)
								{
									NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num180, num181 - 1);
								}
							}
						}
					}
				}
				else
				{
					int num183 = NPC.spriteDirection;
					if ((NPC.velocity.X < 0f && num183 == -1) || (NPC.velocity.X > 0f && num183 == 1))
					{
						if (NPC.height >= 32 && Main.tile[num180, num181 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 2].TileType])
						{
							if (Main.tile[num180, num181 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 3].TileType])
							{
								NPC.velocity.Y = -6f;
								NPC.netUpdate = true;
							}
							else
							{
								NPC.velocity.Y = -5.25f;
								NPC.netUpdate = true;
							}
						}
						else if (Main.tile[num180, num181 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 1].TileType])
						{
							NPC.velocity.Y = -4.5f;
							NPC.netUpdate = true;
						}
						else if (NPC.position.Y + (float)NPC.height - (float)(num181 * 16) > 20f && Main.tile[num180, num181].HasUnactuatedTile && !Main.tile[num180, num181].TopSlope && Main.tileSolid[Main.tile[num180, num181].TileType])
						{
							NPC.velocity.Y = -3.75f;
							NPC.netUpdate = true;
						}
						else if (NPC.directionY < 0 && (!Main.tile[num180, num181 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num180, num181 + 1].TileType]) && (!Main.tile[num180 + NPC.direction, num181 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num180 + NPC.direction, num181 + 1].TileType]))
						{
							NPC.velocity.Y = -6f;
							NPC.velocity.X *= 1.5f;
							NPC.netUpdate = true;
						}
						else if (flag8)
						{
							NPC.ai[1] = 0f;
							NPC.ai[2] = 0f;
						}
						if (NPC.velocity.Y == 0f && flag6 && NPC.ai[3] == 1f)
						{
							NPC.velocity.Y = -3.75f;
						}
						if (NPC.velocity.Y == 0f && (Main.expertMode) && Main.player[NPC.target].Bottom.Y < NPC.Top.Y && Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < (float)(Main.player[NPC.target].width * 3) && Collision.CanHit(NPC, Main.player[NPC.target]))
						{
							if (NPC.velocity.Y == 0f)
							{
								int num186 = 6;
								if (Main.player[NPC.target].Bottom.Y > NPC.Top.Y - (float)(num186 * 16))
								{
									NPC.velocity.Y = -5.175f;
								}
								else
								{
									int num187 = (int)(NPC.Center.X / 16f);
									int num188 = (int)(NPC.Bottom.Y / 16f) - 1;
									for (int num189 = num188; num189 > num188 - num186; num189--)
									{
										if (Main.tile[num187, num189].HasUnactuatedTile && TileID.Sets.Platforms[Main.tile[num187, num189].TileType])
										{
											NPC.velocity.Y = -5.175f;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			else if (flag8)
			{
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient || !(NPC.ai[3] >= (float)num55))
			{
				return;
			}
		}

	}
}