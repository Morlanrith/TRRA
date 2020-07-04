using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.NPCs
{
	public class TRRAGlobalNPC : GlobalNPC
	{
		public override void NPCLoot(NPC npc) 
		{
			if (npc.type == NPCID.Golem && !Main.expertMode) Item.NewItem(npc.getRect(), ItemType<DustExtract>(), 1);
			
		}

		public class BossBags : GlobalItem
		{
			public override void OpenVanillaBag(string context, Player player, int arg)
			{
				if (context == "bossBag" && arg == ItemID.GolemBossBag) player.QuickSpawnItem(ItemType<DustExtract>(), 1);
			}
		}
	}
}
