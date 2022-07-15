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

namespace TRRA.NPCs.Enemies
{
	public class Beowolf : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/Beowolf";

		private static readonly SoundStyle BeoHitSound = new($"{nameof(TRRA)}/Sounds/NPCs/Beowolf/Beowolf_Hit")
		{
			Volume = 0.6f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle BeoPounceSound = new($"{nameof(TRRA)}/Sounds/NPCs/Beowolf/Beowolf_Pounce")
		{
			Volume = 0.6f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DesertBeast];
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

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 3f,
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 94;
			NPC.height = 56;
			NPC.aiStyle = -1;
			NPC.damage = 100;
			NPC.defense = 40;
			NPC.lifeMax = 800;
			NPC.HitSound = BeoHitSound;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.knockBackResist = 0.25f;
			NPC.value = 1000f;
			Banner = NPC.type;
			BannerItem = ItemType<BeowolfBanner>();
			AnimationType = NPCID.DesertBeast;
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
			base.HitEffect(hitDirection, damage);
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 15; i++)
				{
					Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.velocity.X, NPC.velocity.Y)) * 32f;
					int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustType<GrimmParticle>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1f;
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Beowolf_Head").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Beowolf_Torso").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Beowolf_Tail").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Beowolf_FrontLeg").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Beowolf_BackLeg").Type);
			}
		}

        public override void AI()
        {
			if (!TRRAWorld.IsShatteredMoon())
				NPC.EncourageDespawn(10);

			if (Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height == NPC.position.Y + (float)NPC.height)
				NPC.directionY = -1;
			
			bool flag5 = false;
			bool flag6 = false;
			
			if (NPC.velocity.X == 0f && !NPC.justHit)
				flag6 = true;

			int num55 = 60;
			bool flag7 = false;
			bool flag8 = false;

			if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
				flag7 = true;
			
			if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= (float)num55 || flag7)
				NPC.ai[3] += 1f;
			else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
				NPC.ai[3] -= 1f;

			if (NPC.ai[3] > (float)(num55 * 10) || NPC.justHit)
				NPC.ai[3] = 0f;

			if (NPC.ai[3] == (float)num55)
				NPC.netUpdate = true;

			if (Main.player[NPC.target].Hitbox.Intersects(NPC.Hitbox))
				NPC.ai[3] = 0f;

			if (NPC.ai[3] < (float)num55)
			{
				if (TRRAWorld.IsShatteredMoon()) NPC.TargetClosest();
				if (NPC.directionY > 0 && Main.player[NPC.target].Center.Y <= NPC.Bottom.Y)
					NPC.directionY = -1;
			}
			else if (!(NPC.ai[2] > 0f))
			{
				if (NPC.velocity.X == 0f && NPC.velocity.Y == 0f)
				{
					NPC.ai[0] += 1f;
					if (NPC.ai[0] >= 2f)
					{
						NPC.direction *= -1;
						NPC.spriteDirection = NPC.direction;
						NPC.ai[0] = 0f;
					}
				}
				else
					NPC.ai[0] = 0f;
				if (NPC.direction == 0)
					NPC.direction = 1;
			}

			if(NPC.spriteDirection == -1) Lighting.AddLight(NPC.Left, 0.2f, 0f, 0f);
			else Lighting.AddLight(NPC.Right, 0.2f, 0f, 0f);

			float num97 = 5f;
			float num98 = 0.15f;
			float num99 = 0.98f;

			if (NPC.velocity.X < 0f - num97 || NPC.velocity.X > num97)
			{
				if (NPC.velocity.Y == 0f)
					NPC.velocity *= num99;
			}
			else if (NPC.velocity.X < num97 && NPC.direction == 1)
			{
				NPC.velocity.X += num98;
				if (NPC.velocity.X > num97)
					NPC.velocity.X = num97;
			}
			else if (NPC.velocity.X > 0f - num97 && NPC.direction == -1)
			{
				NPC.velocity.X -= num98;
				if (NPC.velocity.X < 0f - num97)
					NPC.velocity.X = 0f - num97;
			}

			if (NPC.velocity.Y == 0f)
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
						flag5 = true;
				}
				if (!flag5 && NPC.velocity.Y < 0f)
					NPC.velocity.Y = 0f;
				
				if (flag20) return;
			}

			if (NPC.velocity.Y >= 0f)
			{
				int num174 = 0;
				if (NPC.velocity.X < 0f)
					num174 = -1;
				if (NPC.velocity.X > 0f)
					num174 = 1;
				Vector2 vector37 = NPC.position;
				vector37.X += NPC.velocity.X;
				int num175 = (int)((vector37.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 1) * num174)) / 16f);
				int num176 = (int)((vector37.Y + (float)NPC.height - 1f) / 16f);
				if (WorldGen.InWorld(num175, num176, 4))
				{
					if (Main.tile[num175, num176] == null)
						Main.tile[num175, num176].ClearTile();
					if (Main.tile[num175, num176 - 1] == null)
						Main.tile[num175, num176 - 1].ClearTile();
					if (Main.tile[num175, num176 - 2] == null)
						Main.tile[num175, num176 - 2].ClearTile();
					if (Main.tile[num175, num176 - 3] == null)
						Main.tile[num175, num176 - 3].ClearTile();
					if (Main.tile[num175, num176 + 1] == null)
						Main.tile[num175, num176 + 1].ClearTile();
					if (Main.tile[num175 - num174, num176 - 3] == null)
						Main.tile[num175 - num174, num176 - 3].ClearTile();

					if ((float)(num175 * 16) < vector37.X + (float)NPC.width && (float)(num175 * 16 + 16) > vector37.X && ((Main.tile[num175, num176].HasUnactuatedTile && !Main.tile[num175, num176].TopSlope && !Main.tile[num175, num176 - 1].TopSlope && Main.tileSolid[Main.tile[num175, num176].TileType] && !Main.tileSolidTop[Main.tile[num175, num176].TileType]) || (Main.tile[num175, num176 - 1].IsHalfBlock && Main.tile[num175, num176 - 1].HasUnactuatedTile)) && (!Main.tile[num175, num176 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 1].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 1].TileType] || (Main.tile[num175, num176 - 1].IsHalfBlock && (!Main.tile[num175, num176 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 4].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 4].TileType]))) && (!Main.tile[num175, num176 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 2].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 2].TileType]) && (!Main.tile[num175, num176 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175, num176 - 3].TileType] || Main.tileSolidTop[Main.tile[num175, num176 - 3].TileType]) && (!Main.tile[num175 - num174, num176 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num175 - num174, num176 - 3].TileType]))
					{
						float num177 = num176 * 16;
						if (Main.tile[num175, num176].IsHalfBlock)
							num177 += 8f;
						if (Main.tile[num175, num176 - 1].IsHalfBlock)
							num177 -= 8f;
						if (num177 < vector37.Y + (float)NPC.height)
						{
							float num178 = vector37.Y + (float)NPC.height - num177;
							float num179 = 16.1f;
							if (num178 <= num179)
							{
								NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num177;
								NPC.position.Y = num177 - (float)NPC.height;
								if (num178 < 9f)
									NPC.stepSpeed = 1f;
								else
									NPC.stepSpeed = 2f;
							}
						}
					}
				}
			}

			if (flag5)
			{
				int num180 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 16) * NPC.direction)) / 16f);
				int num181 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);

				if (Main.tile[num180, num181] == null)
					Main.tile[num180, num181].ClearTile();
				if (Main.tile[num180, num181 - 1] == null)
					Main.tile[num180, num181 - 1].ClearTile();
				if (Main.tile[num180, num181 - 2] == null)
					Main.tile[num180, num181 - 2].ClearTile();
				if (Main.tile[num180, num181 - 3] == null)
					Main.tile[num180, num181 - 3].ClearTile();
				if (Main.tile[num180, num181 + 1] == null)
					Main.tile[num180, num181 + 1].ClearTile();
				if (Main.tile[num180 + NPC.direction, num181 - 1] == null)
					Main.tile[num180 + NPC.direction, num181 - 1].ClearTile();
				if (Main.tile[num180 + NPC.direction, num181 + 1] == null)
					Main.tile[num180 + NPC.direction, num181 + 1].ClearTile();
				if (Main.tile[num180 - NPC.direction, num181 + 1] == null)
					Main.tile[num180 - NPC.direction, num181 + 1].ClearTile();

				if (Main.tile[num180, num181 - 1].HasUnactuatedTile && (Main.tile[num180, num181 - 1].TileType == 10 || Main.tile[num180, num181 - 1].TileType == 388) && flag8)
				{
					NPC.ai[2] += 1f;
					NPC.ai[3] = 0f;
					if (NPC.ai[2] >= 60f)
					{
						bool flag21 = false;
						bool flag22 = Main.player[NPC.target].ZoneGraveyard && Main.rand.Next(60) == 0;

						if (Main.getGoodWorld && !flag22 && flag21)
							NPC.ai[1] = 0f;

						NPC.velocity.X = 0.5f * (float)(-NPC.direction);
						int num182 = 5;

						if (Main.tile[num180, num181 - 1].TileType == 388)
							num182 = 2;

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
									NetMessage.SendData(19, -1, -1, null, 0, num180, num181 - 1, NPC.direction);
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
									NetMessage.SendData(19, -1, -1, null, 4, num180, num181 - 1);
							}

						}
					}
				}
				else
				{
					int num183 = NPC.spriteDirection;
					bool playSound = false;
					if ((NPC.velocity.X < 0f && num183 == -1) || (NPC.velocity.X > 0f && num183 == 1))
					{
						if (NPC.height >= 32 && Main.tile[num180, num181 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 2].TileType])
						{
							if (Main.tile[num180, num181 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 3].TileType])
							{
								NPC.velocity.Y = -8f;
								NPC.netUpdate = true;
							}
							else
							{
								NPC.velocity.Y = -7f;
								NPC.netUpdate = true;
							}
							playSound = true;
						}
						else if (Main.tile[num180, num181 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num180, num181 - 1].TileType])
						{
							NPC.velocity.Y = -6f;
							NPC.netUpdate = true;
							playSound = true;
						}
						else if (NPC.position.Y + (float)NPC.height - (float)(num181 * 16) > 20f && Main.tile[num180, num181].HasUnactuatedTile && !Main.tile[num180, num181].TopSlope && Main.tileSolid[Main.tile[num180, num181].TileType])
						{
							NPC.velocity.Y = -5f;
							NPC.netUpdate = true;
							playSound = true;
						}
						else if (NPC.directionY < 0 && (!Main.tile[num180, num181 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num180, num181 + 1].TileType]) && (!Main.tile[num180 + NPC.direction, num181 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num180 + NPC.direction, num181 + 1].TileType]))
						{
							NPC.velocity.Y = -8f;
							NPC.velocity.X *= 1.5f;
							NPC.netUpdate = true;
							playSound = true;
						}
						else if (flag8)
						{
							NPC.ai[1] = 0f;
							NPC.ai[2] = 0f;
						}
						if (NPC.velocity.Y == 0f && flag6 && NPC.ai[3] == 1f)
						{ 
							NPC.velocity.Y = -5f;
							playSound = true;
						}
					}
					if (playSound && Main.rand.Next(15) == 0) SoundEngine.PlaySound(BeoPounceSound, NPC.Center);
				}
			}
			else if (flag8)
			{
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
			}
		}
	}
}
