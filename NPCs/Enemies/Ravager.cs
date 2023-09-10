using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Dusts;
using TRRA.Biomes;
using TRRA.Items.Placeable;
using Terraria.GameContent.ItemDropRules;
using TRRA.Items.Consumables;
using TRRA.Items.Materials;

namespace TRRA.NPCs.Enemies
{
	public class Ravager : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/Ravager";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Harpy];
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Bleeding] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Position = new Vector2(0f, 24f),
				PortraitPositionYOverride = 0f,
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 24;
			NPC.height = 34;
			NPC.aiStyle = -1;
			NPC.damage = 90;
			NPC.defense = 40;
			NPC.lifeMax = 650;
			NPC.HitSound = SoundID.NPCHit22;
			NPC.DeathSound = SoundID.NPCDeath4;
			NPC.knockBackResist = 0.5f;
			NPC.value = 1000f;
			Banner = NPC.type;
			BannerItem = ItemType<RavagerBanner>();
			AnimationType = NPCID.Harpy;
			SpawnModBiomes = new int[] { GetInstance<ShatteredMoonFakeBiome>().Type };
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			IItemDropRule rule = ItemDropRule.Common(ItemType<EssenceOfGrimm>(), 5);
			npcLoot.Add(rule);
			rule = ItemDropRule.Common(ItemType<MoonSummoner>(), 100);
			npcLoot.Add(rule);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TRRA.Bestiary.Ravager"),

			});
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 10; i++)
				{
					Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.velocity.X, NPC.velocity.Y)) * 32f;
					int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustType<GrimmParticle>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1f;
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Ravager_Head").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Ravager_Wing").Type);
			}
		}

		public override void AI()
        {
			if (!TRRAWorld.IsShatteredMoon())
				NPC.EncourageDespawn(10);

			NPC.noGravity = true;
			if (NPC.collideX)
			{
				NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
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
				NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
				if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
				{
					NPC.velocity.Y = 1f;
				}
				if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
				{
					NPC.velocity.Y = -1f;
				}
			}
			NPC.TargetClosest();
			if (NPC.direction == -1 && NPC.velocity.X > -4f)
			{
				NPC.velocity.X -= 0.1f;
				if (NPC.velocity.X > 4f)
				{
					NPC.velocity.X -= 0.1f;
				}
				else if (NPC.velocity.X > 0f)
				{
					NPC.velocity.X += 0.05f;
				}
				if (NPC.velocity.X < -4f)
				{
					NPC.velocity.X = -4f;
				}
			}
			else if (NPC.direction == 1 && NPC.velocity.X < 4f)
			{
				NPC.velocity.X += 0.1f;
				if (NPC.velocity.X < -4f)
				{
					NPC.velocity.X += 0.1f;
				}
				else if (NPC.velocity.X < 0f)
				{
					NPC.velocity.X -= 0.05f;
				}
				if (NPC.velocity.X > 4f)
				{
					NPC.velocity.X = 4f;
				}
			}
			if (NPC.directionY == -1 && (double)NPC.velocity.Y > -1.5)
			{
				NPC.velocity.Y -= 0.04f;
				if ((double)NPC.velocity.Y > 1.5)
				{
					NPC.velocity.Y -= 0.05f;
				}
				else if (NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y += 0.03f;
				}
				if ((double)NPC.velocity.Y < -1.5)
				{
					NPC.velocity.Y = -1.5f;
				}
			}
			else if (NPC.directionY == 1 && (double)NPC.velocity.Y < 1.5)
			{
				NPC.velocity.Y += 0.04f;
				if ((double)NPC.velocity.Y < -1.5)
				{
					NPC.velocity.Y += 0.05f;
				}
				else if (NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y -= 0.03f;
				}
				if ((double)NPC.velocity.Y > 1.5)
				{
					NPC.velocity.Y = 1.5f;
				}
			}
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
			if (NPC.direction == -1 && NPC.velocity.X > -4f)
			{
				NPC.velocity.X -= 0.1f;
				if (NPC.velocity.X > 4f)
				{
					NPC.velocity.X -= 0.1f;
				}
				else if (NPC.velocity.X > 0f)
				{
					NPC.velocity.X += 0.05f;
				}
				if (NPC.velocity.X < -4f)
				{
					NPC.velocity.X = -4f;
				}
			}
			else if (NPC.direction == 1 && NPC.velocity.X < 4f)
			{
				NPC.velocity.X += 0.1f;
				if (NPC.velocity.X < -4f)
				{
					NPC.velocity.X += 0.1f;
				}
				else if (NPC.velocity.X < 0f)
				{
					NPC.velocity.X -= 0.05f;
				}
				if (NPC.velocity.X > 4f)
				{
					NPC.velocity.X = 4f;
				}
			}
			if (NPC.directionY == -1 && (double)NPC.velocity.Y > -1.5)
			{
				NPC.velocity.Y -= 0.04f;
				if ((double)NPC.velocity.Y > 1.5)
				{
					NPC.velocity.Y -= 0.05f;
				}
				else if (NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y += 0.03f;
				}
				if ((double)NPC.velocity.Y < -1.5)
				{
					NPC.velocity.Y = -1.5f;
				}
			}
			else if (NPC.directionY == 1 && (double)NPC.velocity.Y < 1.5)
			{
				NPC.velocity.Y += 0.04f;
				if ((double)NPC.velocity.Y < -1.5)
				{
					NPC.velocity.Y += 0.05f;
				}
				else if (NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y -= 0.03f;
				}
				if ((double)NPC.velocity.Y > 1.5)
				{
					NPC.velocity.Y = 1.5f;
				}
			}
			NPC.ai[1] += 1f;
			if (NPC.ai[1] > 200f)
			{
				if (!Main.player[NPC.target].wet && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
				{
					NPC.ai[1] = 0f;
				}
				float num212 = 0.2f;
				float num213 = 0.1f;
				float num214 = 4f;
				float num215 = 1.5f;
				if (NPC.ai[1] > 1000f)
				{
					NPC.ai[1] = 0f;
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] > 0f)
				{
					if (NPC.velocity.Y < num215)
					{
						NPC.velocity.Y += num213;
					}
				}
				else if (NPC.velocity.Y > 0f - num215)
				{
					NPC.velocity.Y -= num213;
				}
				if (NPC.ai[2] < -150f || NPC.ai[2] > 150f)
				{
					if (NPC.velocity.X < num214)
					{
						NPC.velocity.X += num212;
					}
				}
				else if (NPC.velocity.X > 0f - num214)
				{
					NPC.velocity.X -= num212;
				}
				if (NPC.ai[2] > 300f)
				{
					NPC.ai[2] = -300f;
				}
			}

			if (NPC.spriteDirection == -1) Lighting.AddLight(NPC.Left, 0.15f, 0f, 0f);
			else Lighting.AddLight(NPC.Right, 0.15f, 0f, 0f);
		}

    }
}
