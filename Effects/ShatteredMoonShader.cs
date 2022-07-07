using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;

namespace TRRA.Effects
{
	public class ShatteredMoonShader : ScreenShaderData
	{
		public ShatteredMoonShader(string passName)
            : base(passName)
        {
        }

		public override void Update(GameTime gameTime)
		{
			float num = 1f - Utils.SmoothStep((float)Main.worldSurface + 50f, (float)Main.rockLayer + 100f, (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f);
			UseOpacity(num * 0.75f);
		}
	}
}