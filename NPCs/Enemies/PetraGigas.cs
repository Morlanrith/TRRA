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
using TRRA.Projectiles.NPCs.Enemies.PetraGigas;

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
            int num157 = NPC.NewNPC(source, (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, NPCType<PetraGigasArm>(), NPC.whoAmI);
			Main.npc[num157].ai[0] = 1f;
			Main.npc[num157].ai[1] = NPC.whoAmI;
			Main.npc[num157].ai[3] = 150f;
			Main.npc[num157].target = NPC.target;
			Main.npc[num157].netUpdate = true;
			Main.npc[num157].ai[2] = Main.npc[num156].whoAmI;
            Main.npc[num156].ai[2] = Main.npc[num157].whoAmI;
		}

        public override void AI()
        {
			// Performs more dangerous/desperate attacks if below a third of health
			bool desperationMode = (double)NPC.life < (double)NPC.lifeMax * 0.5;
			//if (!TRRAWorld.IsShatteredMoon())
			//	NPC.EncourageDespawn(10);

			float speedAdjustment = desperationMode ? 4f : 2f;

			NPC.noGravity = true;
			NPC.noTileCollide = true;

			if (!Main.dayTime)
			{
				NPC.TargetClosest(); // Repeatedly target the nearest player while it is nighttime
			}

			bool stopMoving = false;

			//Lighting.AddLight(NPC.Bottom + new Vector2(0f, -30f), 0.3f, 0.125f, 0.06f); // Mourning wood ambient light

			// ATTACKING
			if (Main.dayTime)
			{
				NPC.EncourageDespawn(10);
				speedAdjustment = 8f;
			}
			else if (NPC.ai[0] == 0f)
			{
				// AI[1] is a timer that controls when the next attack will occur (when it reaches 300)
				NPC.ai[1] += desperationMode ? 3f : 1f; // If below half health, add three tick to the timer, otherwise just add one
				// If the timer is at 300
				if (NPC.ai[1] >= 300f && Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[1] = 0f; // Reset the timer
					NPC.ai[0] = Main.rand.Next(1, 4); // Selects between attacks 1-3
					NPC.ai[2] = NPC.direction;
					NPC.ai[3] = NPC.target;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[0] == 1f) // Attack 1 - Throw arms at player
			{
				stopMoving = true; // Don't move when attacking
				NPC.ai[1] += 1f; // AI[1] is a timer that controls when the attack will stop (when it reaches 180)
				if (NPC.ai[1] >= 180f) // Stop attacking and reset to netural state
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
				}
			}
			else if (NPC.ai[0] == 2f) // Attack 2 - Spin arms around body
			{
				if (!desperationMode)
				{
					stopMoving = true; // Don't move when attacking (regular only)
				}
				NPC.ai[1] += 1f; // AI[1] is a timer that controls when the attack will stop (when it reaches 420)
				if (NPC.ai[1] >= 420f) // Stop attacking and reset to netural state
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = 0f;
				}
			}
			else if (NPC.ai[0] == 3f) // Attack 3 - Create hand projectiles that move towards the player
			{
				stopMoving = true; // Don't move when attacking
				NPC.ai[1] += 1f; // AI[1] is a timer that controls when the attack will stop (when it reaches 240)
				if (NPC.ai[1] % 30f == 0f) // Creates a projectile every 30 ticks of the timer
				{
					Vector2 spawnPosition = Main.player[NPC.target].position;
					int xOffset = Main.rand.Next(75, 151);
					int yOffset = Main.rand.Next(75, 151);
					spawnPosition.X += Main.rand.NextBool() ? xOffset : -xOffset;
					spawnPosition.Y += Main.rand.NextBool() ? yOffset : -yOffset;

                    // MAKE SURE TO ADJUST DAMAGE AND KNOCKBACK
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPosition, new(0), ProjectileType<HandSpawner>(), NPC.damage, 1.0f, 0, NPC.target);
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
