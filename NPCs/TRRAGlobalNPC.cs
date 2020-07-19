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
			if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer || npc.type == NPCID.SkeletronPrime || npc.type == NPCID.TheDestroyer)
			{
				if (Main.hardMode)
                {
					if (!TRRAWorld.noDust && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
					{
						Main.NewText("Your world has been graced with Dust!", 255, 0, 102);
						int style = 0;
						for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 100E-05); k++)
						{
							int i = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
							int j = WorldGen.genRand.Next(Main.maxTilesY - 200, Main.maxTilesY - 20);
							if (PlacementCheck(i, j, 57))
							{
								WorldGen.PlaceTile(i, j, mod.TileType("FireDustCrystalTile"));
								Main.tile[i, j].frameX = (short)style;
								if (style == 90) style = 0;
								else style += 18;
							}
						}
						for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 1000E-05); k++)
						{
							int i = WorldGen.genRand.Next(80, Main.maxTilesX - 80);
							int j = WorldGen.genRand.Next((int)Main.rockLayer + 100, Main.maxTilesY - 200);
							if (PlacementCheck(i, j, 59))
							{
								WorldGen.PlaceTile(i, j, mod.TileType("PlantDustCrystalTile"));
								Main.tile[i, j].frameX = (short)style;
								if (style == 90) style = 0;
								else style += 18;
							}
						}
						for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 1000E-05); k++)
						{
							int i = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
							int j = WorldGen.genRand.Next(10, 400);
							if (PlacementCheck(i, j, 189))
							{
								WorldGen.PlaceTile(i, j, mod.TileType("GravityDustCrystalTile"));
								Main.tile[i, j].frameX = (short)style;
								if (style == 90) style = 0;
								else style += 18;
							}
						}
						for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 1000E-05); k++)
						{
							int i = WorldGen.genRand.Next(80, Main.maxTilesX - 80);
							int j = WorldGen.genRand.Next((int)Main.rockLayer + 100, Main.maxTilesY - 200);
							if (PlacementCheck(i, j, 147))
							{
								WorldGen.PlaceTile(i, j, mod.TileType("IceDustCrystalTile"));
								Main.tile[i, j].frameX = (short)style;
								if (style == 90) style = 0;
								else style += 18;
							}
						}
						TRRAWorld.noDust = true;
					}
				}
                else 
				{
					NPC.downedMechBoss1 = false;
					NPC.downedMechBoss2 = false;
					NPC.downedMechBoss3 = false;
				}			
			}
		}

		private bool PlacementCheck(int i, int j, int attachType)
        {
			if (WorldGen.TileEmpty(i, j))
            {
				if (!WorldGen.TileEmpty(i, j + 1) && Main.tile[i, j + 1].type == attachType) return true;
				else if (!WorldGen.TileEmpty(i, j - 1) && Main.tile[i, j - 1].type == attachType) return true;
				else if (!WorldGen.TileEmpty(i + 1, j) && Main.tile[i + 1, j].type == attachType) return true;
				else if (!WorldGen.TileEmpty(i - 1, j) && Main.tile[i - 1, j].type == attachType) return true;
			}
			return false;
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
