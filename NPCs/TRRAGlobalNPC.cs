using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using TRRA.NPCs.Enemies;

namespace TRRA.NPCs
{
	public class TRRAGlobalNPC : GlobalNPC
	{
		public class NormalModeDropCondition : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!info.IsExpertMode) return true;
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				if(!Main.expertMode) return true;
				return false;
			}

			public string GetConditionDescription()
			{
				return "";
			}
		}

		public class BossBags : GlobalItem
		{
            [System.Obsolete]
            public override void OpenVanillaBag(string context, Player player, int arg)
			{
				if (context == "bossBag" && arg == ItemID.GolemBossBag) player.QuickSpawnItem(player.GetSource_OpenItem(ItemType<DustExtract>()), ItemType<DustExtract>(), 1);
			}
		}

		public class ShatteredMoon
		{
			private static readonly int[] enemies = {
				NPCType<Beowolf>(),
				NPCType<Creep>(),
				NPCType<Lancer>(),
				NPCType<Apathy>(),
				NPCType<Ravager>()
			};

			public static int[] GetEnemies() { return enemies; }
		}

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.Golem)
			{
				IItemDropRule conditionalRule = new LeadingConditionRule(new NormalModeDropCondition());
				IItemDropRule rule = ItemDropRule.Common(ItemType<DustExtract>(), 1);
				conditionalRule.OnSuccess(rule);
				npcLoot.Add(conditionalRule);
			}
		}

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
			if (TRRAWorld.IsShatteredMoon() && player.position.Y < Main.worldSurface * 16.0)
            {
				spawnRate = (int)(spawnRate * 0.2);
				maxSpawns = (int)(maxSpawns * 1.9f);
			}
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
			if (TRRAWorld.IsShatteredMoon() && spawnInfo.Player.position.Y < Main.worldSurface * 16.0)
			{
				pool.Clear();
				if (spawnInfo.Sky) // If the player is in the sky, spawn only Ravagers and Lancers
				{
					pool.Add(NPCType<Ravager>(), 1f);
					pool.Add(NPCType<Lancer>(), 0.3f);
				}
				else
				{
					// Otherwise, makes the enemy pool for the surface consist only of Grimm (with Apathy and Lancers having a lower spawn chance)
					foreach (int i in ShatteredMoon.GetEnemies())
					{
						float spawnChance = 1f;
						if (i == NPCType<Lancer>()) spawnChance = 0.3f;
						else if (i == NPCType<Apathy>()) spawnChance = 0.4f;
						pool.Add(i, spawnChance);
					}
					// If time is past midnight, and one has not already spawned, allow spawning of a Petra Gigas
					if(Main.time > 16200 && !TRRAWorld.GigasSpawned())
					{
						pool.Add(NPCType<PetraGigas>(), 1.0f);
                    }
				}
			}
        }

        public static void GenerateDust()
        {
			if(Main.netMode != NetmodeID.Server)
				Main.NewText(Main.worldName + " has been graced with Dust!", 255, 0, 102);
			int style = 0;
			for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 100E-05); k++)
			{
				int i = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
				int j = WorldGen.genRand.Next(Main.maxTilesY - 200, Main.maxTilesY - 20);
				if (PlacementCheck(i, j, 57))
				{
					WorldGen.PlaceTile(i, j, TileType<FireDustCrystalTile>());
					Main.tile[i, j].TileFrameX = (short)style;
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
					WorldGen.PlaceTile(i, j, TileType<PlantDustCrystalTile>());
					Main.tile[i, j].TileFrameX = (short)style;
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
					WorldGen.PlaceTile(i, j, TileType<GravityDustCrystalTile>());
					Main.tile[i, j].TileFrameX = (short)style;
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
					WorldGen.PlaceTile(i, j, TileType<IceDustCrystalTile>());
					Main.tile[i, j].TileFrameX = (short)style;
					if (style == 90) style = 0;
					else style += 18;
				}
			}
			TRRAWorld.DustSpawned();
		}

		public override void OnKill(NPC npc)
		{
			if (npc.type == NPCID.SkeletronHead && !TRRAWorld.GetNoDust()) GenerateDust();
			base.OnKill(npc);
		}

		private static bool PlacementCheck(int i, int j, int attachType)
        {
			if (WorldGen.TileEmpty(i, j))
            {
				if (!WorldGen.TileEmpty(i, j + 1) && Main.tile[i, j + 1].TileType == attachType) return true;
				else if (!WorldGen.TileEmpty(i, j - 1) && Main.tile[i, j - 1].TileType == attachType) return true;
				else if (!WorldGen.TileEmpty(i + 1, j) && Main.tile[i + 1, j].TileType == attachType) return true;
				else if (!WorldGen.TileEmpty(i - 1, j) && Main.tile[i - 1, j].TileType == attachType) return true;
			}
			return false;
        }

	}
}
