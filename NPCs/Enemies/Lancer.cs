using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.NPCs
{
	public class Lancer : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Enemies/Lancer";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Hornet];
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Velocity = 1f,
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 96;
			NPC.height = 122;
			NPC.aiStyle = 5;
			NPC.damage = 70;
			NPC.defense = 20;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			NPC.value = 1000f;
			NPC.noGravity = true;
			AIType = NPCID.MossHornet;
			AnimationType = NPCID.Hornet;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

				new FlavorTextBestiaryInfoElement("A dangerous hornet-like creature that is not of this world."),

			});
		}

        public override void OnKill()
        {
            base.OnKill();
			if (Main.netMode == NetmodeID.Server)
				return;

			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Lancer_Head").Type);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Lancer_Abdomen").Type);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Lancer_Wing").Type);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Lancer_Talon").Type);
		}

		public override void AI()
        {
			if (NPC.spriteDirection == -1) Lighting.AddLight(NPC.Left, 0.2f, 0f, 0f);
			else Lighting.AddLight(NPC.Right, 0.2f, 0f, 0f);
		}
	}
}
