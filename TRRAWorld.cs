using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TRRA
{
	public class TRRAWorld : ModSystem
	{
		public static bool NoDust = false;
        public static bool ShatteredMoon = false;
        private bool justDay = false;
        private Color moonCrimson = new(40, 10, 34);

        public override void OnWorldLoad()
        {
            NoDust = Terraria.NPC.downedPlantBoss;
            ShatteredMoon = false;
            justDay = Main.dayTime;
        }

        public override void OnWorldUnload()
        {
            NoDust = false;
            ShatteredMoon = false;
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            base.ModifySunLightColor(ref tileColor, ref backgroundColor);
            if(ShatteredMoon)
            {
                tileColor = moonCrimson;
                backgroundColor = moonCrimson;
            }
        }

        public override void PostUpdateWorld()
        {
            if (ShatteredMoon && (Main.dayTime || Main.bloodMoon || Main.pumpkinMoon || Main.snowMoon)) ShatteredMoon = false;
        }

        public override void PostUpdateTime()
        {
            if (justDay && !Main.dayTime && !Main.fastForwardTime && !Main.ShouldNormalEventsBeAbleToStart())
            {
                justDay = false;
                if (!Main.bloodMoon && Main.rand.Next(8) == 0)
                {
                    Main.NewText("The Shattered Moon rises...", 186, 34, 64);
                    ShatteredMoon = true;
                }
            }
            else if (!justDay && Main.dayTime) justDay = true;
        }

    }
}