using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Biomes;
using TRRA.Items.Placeable;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using TRRA.Items.Consumables;
using TRRA.Items.Materials;
using TRRA.Projectiles.NPCs.Enemies.PetraGigas;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameContent;

namespace TRRA.NPCs.Enemies
{
    public class PetraGigasBossBar : ModBossBar
    {
        private int bossHeadIndex = -1;

        public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
        {
            if (bossHeadIndex != -1)
            {
                return TextureAssets.NpcHeadBoss[bossHeadIndex];
            }
            return null;
        }

        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float lifePercent, ref float shieldPercent)
        {
            NPC npc = Main.npc[info.npcIndexToAimAt];
            if (!npc.active)
                return false;

            bossHeadIndex = npc.GetBossHeadTextureIndex();

            lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);

            return true;
        }
    }

    [AutoloadBossHead]
    public class PetraGigas : ModNPC
	{
        private int[] arms = { 0, 0 };
        private int soundTimer = 0;
        private static readonly SoundStyle GigasHandSound = new($"{nameof(TRRA)}/Sounds/NPCs/PetraGigas/Gigas_HandPortal")
        {
            Volume = 0.6f,
            Pitch = 0.0f,
        };
        public override string Texture => "TRRA/NPCs/Enemies/PetraGigas";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.MourningWood];
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;

            NPCID.Sets.BossBestiaryPriority.Add(Type);

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
                CustomTexturePath = "TRRA/NPCs/Enemies/PetraGigas_Portrait",
                Position = new Vector2(15f, 104f),
                PortraitPositionYOverride = 80f,
                PortraitPositionXOverride = 0f,
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults() {
			NPC.width = 104;
			NPC.height = 154;
			NPC.aiStyle = -1;
			NPC.damage = 140;
			NPC.defense = 60;
			NPC.lifeMax = 24000;
			NPC.HitSound = SoundID.NPCHit41;
			NPC.DeathSound = SoundID.NPCDeath43;
			NPC.knockBackResist = 0f;
			NPC.value = 10000f;
			NPC.npcSlots = 4f;
			NPC.boss = true;
			NPC.BossBar = GetInstance<PetraGigasBossBar>();
			AnimationType = NPCID.MourningWood;
			SpawnModBiomes = new int[] { GetInstance<ShatteredMoonFakeBiome>().Type };
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			IItemDropRule rule = ItemDropRule.Common(ItemType<EssenceOfGrimm>(), 1, 10, 20);
			npcLoot.Add(rule);
			rule = ItemDropRule.Common(ItemType<MoonSummoner>(), 100);
			npcLoot.Add(rule);
			rule = ItemDropRule.Common(ItemType<GeistTrophy>(), 10);
            npcLoot.Add(rule);
			rule = ItemDropRule.MasterModeCommonDrop(ItemType<GeistRelic>());
            npcLoot.Add(rule);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TRRA.Bestiary.PetraGigas"),

			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 30; i++)
				{
					Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.velocity.X, NPC.velocity.Y)) * 32f;
					int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustID.Stone);
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity *= 1f;
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigas_Body").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigas_Leg").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigas_LongRock").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigas_MidRock").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigas_MidRock2").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigas_SmallRock").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigas_SmallRock2").Type);
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
			potionType = ItemID.GreaterHealingPotion;
        }

        public override void OnSpawn(IEntitySource source)
        {
			NPC.TargetClosest();
			arms[0] = NPC.NewNPC(source, (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, NPCType<PetraGigasArm>(), NPC.whoAmI);
			Main.npc[arms[0]].ai[0] = -1f;
			Main.npc[arms[0]].ai[1] = NPC.whoAmI;
			Main.npc[arms[0]].target = NPC.target;
			Main.npc[arms[0]].netUpdate = true;
            arms[1] = NPC.NewNPC(source, (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, NPCType<PetraGigasArm>(), NPC.whoAmI);
			Main.npc[arms[1]].ai[0] = 1f;
			Main.npc[arms[1]].ai[1] = NPC.whoAmI;
			Main.npc[arms[1]].ai[3] = 150f;
			Main.npc[arms[1]].target = NPC.target;
			Main.npc[arms[1]].netUpdate = true;
			Main.npc[arms[1]].ai[2] = Main.npc[arms[0]].whoAmI;
            Main.npc[arms[0]].ai[2] = Main.npc[arms[1]].whoAmI;
		}

        public override void AI()
        {
			// Performs more dangerous/desperate attacks if below a third of health
			bool desperationMode = (double)NPC.life < (double)NPC.lifeMax * 0.5;
			if (!TRRAWorld.IsShatteredMoon())
			{
				NPC.EncourageDespawn(10);
			}
            Lighting.AddLight(NPC.Center, 0.3f, 0f, 0f);

			bool noArmsLeft = (!Main.npc[arms[0]].active || Main.npc[arms[0]].type != NPCType<PetraGigasArm>()) && (!Main.npc[arms[1]].active || Main.npc[arms[1]].type != NPCType<PetraGigasArm>());

            float speedAdjustment = desperationMode ? 4f : 2f;

			NPC.noGravity = true;
			NPC.noTileCollide = true;

			if (!Main.dayTime)
			{
				NPC.TargetClosest(); // Repeatedly target the nearest player while it is nighttime
			}

			bool stopMoving = false;

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
                    NPC.ai[0] = noArmsLeft ? 3f : Main.rand.Next(1, 4); // Selects between attacks 1-3
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

                    SoundEngine.PlaySound(GigasHandSound, spawnPosition);
                    // MAKE SURE TO ADJUST DAMAGE AND KNOCKBACK
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPosition, new(0), ProjectileType<HandSpawner>(), NPC.damage, 1.0f, 0, NPC.target, Main.rand.NextBool() ? 1 : -1);
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
                else if (soundTimer++ == 15)
                {
                    SoundEngine.PlaySound(SoundID.DeerclopsStep, NPC.Bottom);
                    soundTimer = 0;
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
                if (soundTimer++ == 15)
                {
                    SoundEngine.PlaySound(SoundID.DeerclopsStep, NPC.Bottom);
                    soundTimer = 0;
                }
            }

			// PHASE THROUGH WALLS
			int num903 = 80;
			int num904 = 20;
			Vector2 vector114 = new(NPC.Center.X - (float)(num903 / 2), NPC.position.Y + (float)NPC.height - (float)num904);
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

            if(NPC.velocity.X != 0f) NPC.spriteDirection = NPC.direction;
        }

    }
}
