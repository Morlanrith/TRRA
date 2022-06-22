using Terraria.ModLoader;

namespace TRRA
{
	public class TRRAWorld : ModSystem
	{
		public static bool noDust = false;

        public override void OnWorldLoad()
        {
            if (Terraria.NPC.downedPlantBoss) noDust = true;
            base.OnWorldLoad();
        }

    }
}