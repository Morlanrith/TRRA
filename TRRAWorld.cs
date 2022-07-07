using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using static Terraria.Main;

namespace TRRA
{
	public class TRRAWorld : ModSystem
	{
		private static bool NoDust = false;
		private static bool ShatteredMoon = false;
        private bool justDay = false;
        private Color moonCrimson = new(40, 10, 34);

		public static bool GetNoDust() { return NoDust; }
		public static void DustSpawned() { NoDust = true; }
		public static bool IsShatteredMoon() { return ShatteredMoon; }
		public static void BeginShatteredMoon()
        {
			Main.NewText("The Shattered Moon rises...", 186, 34, 64);
			ShatteredMoon = true;
		}

		public override void OnWorldLoad()
        {
            NoDust = Terraria.NPC.downedPlantBoss;
            ShatteredMoon = false;
            justDay = dayTime;
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
				DrawMoon();
			}
        }

        public override void PostUpdateWorld()
        {
            if (ShatteredMoon && (dayTime || bloodMoon || pumpkinMoon || snowMoon)) ShatteredMoon = false;
        }

        public override void PostUpdateTime()
        {
            if (justDay && !dayTime && !fastForwardTime && !Main.ShouldNormalEventsBeAbleToStart())
            {
                justDay = false;
                if (!bloodMoon && rand.Next(8) == 0)
                {
					BeginShatteredMoon();
                }
            }
            else if (!justDay && dayTime) justDay = true;
        }

		private void DrawMoon()
		{
			SceneArea sceneAreaI = default;
			sceneAreaI.bgTopY = 0;
			sceneAreaI.totalHeight = screenHeight;
			sceneAreaI.totalWidth = screenWidth;
			sceneAreaI.SceneLocalScreenPositionOffset = Vector2.Zero;
			SceneArea sceneArea = sceneAreaI;
			Color moonColor = Color.White;
			int num = 0;
			if (!TextureAssets.Moon.IndexInRange(num))
			{
				num = Utils.Clamp(num, 0, 8);
			}
			Texture2D value2 = TextureAssets.Moon[num].Value;
			int num2 = sceneArea.bgTopY;
			float num5 = 1f;
			float rotation = (float)(time / 54000.0) * 2f - 7.3f;
			int num6 = (int)(time / 32400.0 * (double)(sceneArea.totalWidth + (float)(value2.Width * 2))) - value2.Width;
			int num7 = 0;
			float num8 = 1f;
			float num9 = (float)(time / 32400.0) * 2f - 7.3f;
			double num11;
			if (time < 16200.0)
			{
				num11 = Math.Pow(1.0 - time / 32400.0 * 2.0, 2.0);
				num7 = (int)((double)num2 + num11 * 250.0 + 180.0);
			}
			else
			{
				num11 = Math.Pow((time / 32400.0 - 0.5) * 2.0, 2.0);
				num7 = (int)((double)num2 + num11 * 250.0 + 180.0);
			}
			num8 = (float)(1.2 - num11 * 0.4);
			num5 *= ForcedMinimumZoom;
			num8 *= ForcedMinimumZoom;
			if (!dayTime)
			{
				float num13 = 1f - cloudAlpha * 1.5f * atmo;
				if (num13 < 0f)
				{
					num13 = 0f;
				}
				moonColor *= num13;
				Vector2 position2 = new Vector2(num6, num7 + moonModY) + sceneArea.SceneLocalScreenPositionOffset;
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Transform);
				spriteBatch.Draw(TextureAssets.Moon[num].Value, position2, new Rectangle(0, TextureAssets.Moon[num].Width() * 0, TextureAssets.Moon[num].Width(), TextureAssets.Moon[num].Width()), moonColor, num9, new Vector2(TextureAssets.Moon[num].Width() / 2, TextureAssets.Moon[num].Width() / 2), num8, SpriteEffects.None, 0f);
				spriteBatch.End();
			}
			sunModY = (short)((double)sunModY * 0.999);
			moonModY = (short)((double)moonModY * 0.999);
		}

	}
}