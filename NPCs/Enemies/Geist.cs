using Microsoft.Xna.Framework;
using Terraria;
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
using System;

namespace TRRA.NPCs.Enemies
{
	public class Geist : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/Geist";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 8;
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

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Position = new Vector2(24f, 18f),
				PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = -20f,
            };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}

		public override void SetDefaults() {
			NPC.width = 90;
			NPC.height = 60;
			NPC.aiStyle = -1;
			NPC.damage = 90;
			NPC.defense = 30;
			NPC.lifeMax = 1800;
			NPC.HitSound = SoundID.NPCHit54;
			NPC.DeathSound = SoundID.NPCDeath52;
			NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.value = 50000f;
			NPC.alpha = 50;
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

				new FlavorTextBestiaryInfoElement("Mods.TRRA.Bestiary.Geist"),

			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 10; i++)
				{
					Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.velocity.X, NPC.velocity.Y));
					int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustType<GrimmParticle>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1f;
				}
                for (int i = 0; i < 4; i++)
                {
                    Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.velocity.X, NPC.velocity.Y));
                    int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustID.Wraith, 0 , 0, 50, default, 3);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                }
            }
		}

        public override void FindFrame(int frameHeight)
        {
            int startFrame = 0;
            int finalFrame = 7;

            int frameSpeed = 5;
            NPC.frameCounter += 0.5f;
            NPC.frameCounter += NPC.velocity.Length() / 10f; // Make the counter go faster with more movement speed
            if (NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y > finalFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }

		public override void AI()
		{
			if (!TRRAWorld.IsShatteredMoon())
				NPC.EncourageDespawn(10);

			NPC.noGravity = true;
			NPC.TargetClosest();

			Vector2 vector130 = new(NPC.Center.X + (float)(NPC.direction * 20), NPC.Center.Y + 6f);
			float num990 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector130.X;
			float num992 = (float)Math.Sqrt(num990 * num990);
			float num993 = 3.5f / num992;
			num990 *= num993;
			int num994 = 60;
			NPC.velocity.X = (NPC.velocity.X * (float)(num994 - 1) - num990) / (float)num994;
			if (NPC.velocity.X > 0f)
			{
				NPC.spriteDirection = 1;
				Lighting.AddLight(NPC.Right, 0.15f, 0.15f, 0f);
			}
			if (NPC.velocity.X < 0f)
			{ 
				NPC.spriteDirection = -1;
                Lighting.AddLight(NPC.Left, 0.15f, 0.15f, 0f);
            }
		}

    }
}
