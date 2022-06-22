using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Terraria.GameContent.ItemDropRules;

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
			public override void OpenVanillaBag(string context, Player player, int arg)
			{
				if (context == "bossBag" && arg == ItemID.GolemBossBag) player.QuickSpawnItem(player.GetSource_OpenItem(ItemType<DustExtract>()), ItemType<DustExtract>(), 1);
			}
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

		public void GenerateDust()
        {
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
			TRRAWorld.noDust = true;
		}

		public override void OnKill(NPC npc)
		{
			if (npc.type == NPCID.Plantera && !TRRAWorld.noDust) GenerateDust();
			base.OnKill(npc);
		}

		private bool PlacementCheck(int i, int j, int attachType)
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
