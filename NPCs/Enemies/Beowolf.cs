using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Weapon;
using static Terraria.ModLoader.ModContent;

namespace TRRA.NPCs
{
	public class Beowolf : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/Beowolf";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Lihzahrd];
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 42;
			NPC.aiStyle = 3;
			NPC.damage = 80;
			NPC.defense = 40;
			NPC.lifeMax = 400;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			AIType = NPCID.LihzahrdCrawler;
			AnimationType = NPCID.Lihzahrd;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

				new FlavorTextBestiaryInfoElement("A dangerous wolf-like creature that is not of this world."),

			});
		}
	}
}
