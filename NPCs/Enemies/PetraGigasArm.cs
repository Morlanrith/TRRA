using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Biomes;
using Terraria.DataStructures;

namespace TRRA.NPCs.Enemies
{
	public class PetraGigasArm : ModNPC
	{
        private Vector2 targetPos;
        private bool singleArm = false;
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
			NPC.width = 40;
			NPC.height = 156;
			NPC.aiStyle = -1;
			NPC.damage = 110;
			NPC.defense = 40;
			NPC.lifeMax = 4800;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.Item70;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.value = 0f;
			AnimationType = NPCID.SkeletronHand;
			SpawnModBiomes = new int[] { GetInstance<ShatteredMoonFakeBiome>().Type };
		}

        public override bool CheckActive() => false;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
                for (int i = 0; i < 10; i++)
                {
                    Vector2 dustOffset = Vector2.Normalize(new Vector2(NPC.velocity.X, NPC.velocity.Y)) * 32f;
                    int dust = Dust.NewDust(NPC.position + dustOffset, NPC.width, NPC.height, DustID.Stone);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity *= 1f;
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigasArm_Shoulder").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PetraGigasArm_Wrist").Type);
            }
		}

		private void NormalBehaviour()
		{
            NPC parent = Main.npc[(int)NPC.ai[1]];
            float elevation = 0f; // Height above the center of the body
            if (NPC.position.Y > parent.position.Y - elevation)
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
            else if (NPC.position.Y < parent.position.Y - elevation)
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
            float distance = 120f; // Distance from the center of the body
            if (NPC.position.X + (float)(NPC.width / 2) > parent.position.X + (float)(parent.width / 2) - distance * NPC.ai[0])
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
            if (NPC.position.X + (float)(NPC.width / 2) < parent.position.X + (float)(parent.width / 2) - distance * NPC.ai[0])
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

            Vector2 vector22 = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num182 = parent.position.X + (float)(parent.width / 2) - 200f * NPC.ai[0] - vector22.X;
            float num183 = parent.position.Y + 230f - vector22.Y;
            NPC.rotation = (float)Math.Atan2(num183, num182) - 1.57f;
        }

		public override void AI()
        {
            NPC parent = Main.npc[(int)NPC.ai[1]];
            float parentState = parent.ai[0];
            Lighting.AddLight(NPC.Center, 0.15f, 0f, 0f);

            NPC.spriteDirection = -(int)NPC.ai[0];
			// Automatically destroy the arm if the main enemy has been slain
			if (!parent.active || parent.type != NPCType<PetraGigas>())
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
                if(!Main.npc[(int)NPC.ai[2]].active)
                {
                    singleArm= true;
                }
                NormalBehaviour();
            }
            else if (parentState == 1f) // Fly at player
            {
                float timer = parent.ai[1];
                if (parent.ai[2] == NPC.ai[0] && !singleArm)
                {
                    NormalBehaviour();
                    return;
                }
                if (timer < 50f)
                {
                    if (timer == 0f)
                    {
                        Vector2 targetDiff = Main.player[(int)parent.ai[3]].position - NPC.Center;
                        targetDiff.Normalize();
                        targetDiff *= 600;
                        targetPos = NPC.Center + targetDiff;
                    }
                    float xDiff = (targetPos.X - NPC.Center.X) / (50f - timer);
                    float yDiff = (targetPos.Y - NPC.Center.Y) / (50f - timer);

                    NPC.Center = new(NPC.Center.X + xDiff, NPC.Center.Y + yDiff);
                    NPC.rotation += 0.3f * (parent.ai[2]*-1);
                }
                else
                {
                    NormalBehaviour();
                }
            }
            else if (parentState == 2f) // Spin around body
            {
                float timer = parent.ai[1] - 60f;
                if (timer >= 0f && timer < 360f)
                {
                    Vector2 newPos = new(parent.Center.X + (NPC.ai[0] * -(200 + (timer / 2f))), parent.Center.Y);
                    double angle = timer * 3.0 * parent.ai[2];

                    double radians = (Math.PI / 180) * angle;
                    double sin = Math.Sin(radians);
                    double cos = Math.Cos(radians);

                    // Translate position back to origin
                    newPos.X -= parent.Center.X;
                    newPos.Y -= parent.Center.Y;

                    // Rotate position
                    double xnew = newPos.X * cos - newPos.Y * sin;
                    double ynew = newPos.X * sin + newPos.Y * cos;

                    // Translate position back
                    newPos = new Vector2((int)xnew + parent.Center.X, (int)ynew + parent.Center.Y);
                    NPC.rotation = parent.rotation + (float)((Math.PI / 180) * (angle + (NPC.ai[0] * 90)));
                    NPC.Center = newPos;
                }
                else if (timer < 0f)
                {
                    timer += 60f;
                    Vector2 intendedPos = new(parent.Center.X + (NPC.ai[0] * -200), parent.Center.Y);
                    float xDiff = (intendedPos.X - NPC.Center.X) / (60f - timer);
                    float yDiff = (intendedPos.Y - NPC.Center.Y) / (60f - timer);

                    NPC.Center = new(NPC.Center.X + xDiff, NPC.Center.Y + yDiff);

                    float intendedRot = parent.rotation + (float)((Math.PI / 180) * (NPC.ai[0] * 90));

                    while (intendedRot * NPC.ai[0] < NPC.rotation * NPC.ai[0])
                    {
                        intendedRot += (float)(Math.PI * 2) * NPC.ai[0];
                    }

                    NPC.rotation += (intendedRot - NPC.rotation) / (60f - timer);
                }
            }
        }

    }
}
