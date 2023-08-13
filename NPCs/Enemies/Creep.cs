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
	public class Creep : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/Creep";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Derpling];
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
			NPC.width = 64;
			NPC.height = 54;
			NPC.aiStyle = -1;
			NPC.damage = 100;
			NPC.defense = 40;
			NPC.lifeMax = 700;
			NPC.HitSound = SoundID.NPCHit20;
			NPC.DeathSound = SoundID.NPCDeath25;
			NPC.knockBackResist = 0.5f;
			NPC.value = 1000f;
			Banner = NPC.type;
			BannerItem = ItemType<CreepBanner>();
			AnimationType = NPCID.Derpling;
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

				new FlavorTextBestiaryInfoElement("Mods.TRRA.Bestiary.Creep"),

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
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Creep_Head").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Creep_Torso").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Creep_Tail").Type);
			}
		}

		public override void AI()
        {
			if (!TRRAWorld.IsShatteredMoon())
				NPC.EncourageDespawn(10);

			if (NPC.ai[2] > 1f)
			{
				NPC.ai[2] -= 1f;
			}
			if (NPC.ai[2] == 0f)
			{
				NPC.ai[0] = -100f;
				NPC.ai[2] = 1f;
				if (TRRAWorld.IsShatteredMoon()) NPC.TargetClosest();
				NPC.spriteDirection = NPC.direction;
			}
			if (NPC.velocity.Y == 0f)
			{
				if (NPC.ai[3] == NPC.position.X)
				{
					NPC.direction *= -1;
					NPC.ai[2] = 300f;
				}
				NPC.ai[3] = 0f;
				NPC.velocity.X *= 0.8f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
				{
					NPC.velocity.X = 0f;
				}
				NPC.ai[0] += 2f;
				Vector2 vector74 = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num608 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector74.X;
				float num609 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector74.Y;
				float num610 = (float)Math.Sqrt(num608 * num608 + num609 * num609);
				float num611 = (400f / num610) * 5f;
				if (num611 > 30f)
				{
					num611 = 30f;
				}
				NPC.ai[0] += (int)num611;
				if (NPC.ai[0] >= 0f)
				{
					NPC.netUpdate = true;
					if (NPC.ai[2] == 1f)
					{
						if (TRRAWorld.IsShatteredMoon()) NPC.TargetClosest();
					}
					if (NPC.ai[1] == 2f)
					{
						NPC.velocity.Y = -11.5f;
						NPC.velocity.X += 2f * (float)NPC.direction;
						if (num610 < 350f && num610 > 200f)
						{
							NPC.velocity.X += NPC.direction;
						}
						NPC.ai[0] = -200f;
						NPC.ai[1] = 0f;
						NPC.ai[3] = NPC.position.X;
					}
					else
					{
						NPC.velocity.Y = -7.5f;
						NPC.velocity.X += 4 * NPC.direction;
						if (num610 < 350f && num610 > 200f)
						{
							NPC.velocity.X += NPC.direction;
						}
						NPC.ai[0] = -120f;
						NPC.ai[1] += 1f;
					}
					if (Main.rand.NextBool(15)) SoundEngine.PlaySound(SoundID.NPCHit21, NPC.position);
				}
				else if (NPC.ai[0] >= -30f)
				{
					NPC.aiAction = 1;
				}
				NPC.spriteDirection = NPC.direction;
			}
			else
			{
				if (NPC.target >= 255)
				{
					return;
				}
				bool flag34 = false;
				if (NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X && NPC.position.X < Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width)
				{
					flag34 = true;
					NPC.velocity.X *= 0.92f;
					if (NPC.velocity.Y < 0f)
					{
						NPC.velocity.Y *= 0.9f;
						NPC.velocity.Y += 0.1f;
					}
				}
				if (!flag34 && ((NPC.direction == 1 && NPC.velocity.X < 4f) || (NPC.direction == -1 && NPC.velocity.X > -4f)))
				{
					if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.1) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.1))
					{
						NPC.velocity.X += 0.2f * (float)NPC.direction;
					}
					else
					{
						NPC.velocity.X *= 0.93f;
					}
				}
			}
			if (NPC.spriteDirection == -1) Lighting.AddLight(NPC.Left, 0.15f, 0f, 0f);
			else Lighting.AddLight(NPC.Right, 0.15f, 0f, 0f);
		}

    }
}
