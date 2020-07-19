using IL.Terraria;
using Terraria.ModLoader;

namespace TRRA
{
	public class TRRAWorld : ModWorld
	{
		public static bool noDust = false;
        public override void Initialize()
        {
            if(Terraria.NPC.downedMechBoss1 && Terraria.NPC.downedMechBoss2 && Terraria.NPC.downedMechBoss3) noDust = true;
            base.Initialize();
        }
    }
}