using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.Main;

namespace TRRA
{
	public class TRRAWorld : ModSystem
	{
		private static bool NoDust = false;
		private static bool ShatteredMoon = false;
        private bool justDay = false;
        private Color moonCrimson = new(40, 10, 34);
		private const string effectAssetPath = "TRRA/Effects";
		private int oldMoonType = 0;

		public static bool GetNoDust() { return NoDust; }
		public static void DustSpawned() { NoDust = true; }
		public static bool IsShatteredMoon() { return ShatteredMoon; }
		public static bool BeginShatteredMoon()
        {
			if (IsShatteredMoon() || dayTime || bloodMoon || pumpkinMoon || snowMoon || invasionType != 0 || DD2Event.Ongoing)
				return false;
			if (netMode != NetmodeID.Server)
				Main.NewText("The Shattered Moon rises...", 186, 34, 64);
			moonType = TextureAssets.Moon.Length - 1;
			invasionType = -1;
			ShatteredMoon = true;
			return true;
		}

		public override void OnWorldLoad()
        {
			oldMoonType = moonType;
			Asset<Texture2D>[] newMoons = new Asset<Texture2D>[TextureAssets.Moon.Length + 1];
			TextureAssets.Moon.CopyTo(newMoons, 0);
			newMoons[TextureAssets.Moon.Length] = ModContent.Request<Texture2D>($"{effectAssetPath}/Moon_Shattered");
			TextureAssets.Moon = newMoons;
			NoDust = Terraria.NPC.downedBoss3;
		}

		public override void OnWorldUnload()
        {
			NoDust = false;
            ShatteredMoon = false;
			Asset<Texture2D>[] oldMoons = new Asset<Texture2D>[TextureAssets.Moon.Length - 1];
			for (int i = 0; i < oldMoons.Length; i++) oldMoons[i] = TextureAssets.Moon[i];
			TextureAssets.Moon = oldMoons;
			if (invasionType == -1) invasionType = 0;
		}

		public override void PreSaveAndQuit()
        {
			moonType = oldMoonType;
		}

        public override void LoadWorldData(TagCompound tag)
        {
			tag.TryGet<bool>("WasShatteredMoon", out ShatteredMoon);
			if (ShatteredMoon)
            {
				moonType = TextureAssets.Moon.Length - 1;
				invasionType = -1;
			}
		}

		public override void SaveWorldData(TagCompound tag)
        {
			tag.Add("WasShatteredMoon", ShatteredMoon);
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
			if (ShatteredMoon && (dayTime || bloodMoon || pumpkinMoon || snowMoon || DD2Event.Ongoing || invasionType > 0))
			{
				ShatteredMoon = false;
				moonType = oldMoonType;
				if(invasionType == -1) invasionType = 0;
			}
		}

        public override void PostUpdateTime()
        {
            if (justDay && !dayTime && !fastForwardTime && !Main.ShouldNormalEventsBeAbleToStart())
            {
                justDay = false;
                if (rand.Next(9) == 0 && moonPhase != 4 && !slimeRain && !LanternNight.LanternsUp && NPC.downedPlantBoss)
					BeginShatteredMoon();
            }
            else if (!justDay && dayTime && !gameMenu) justDay = true;
        }

	}
}