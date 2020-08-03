using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TRRA.Dusts
{
	public class Shatter : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.4f;
			dust.scale *= 1.5f;
		}

		public override bool MidUpdate(Dust dust)
		{
			dust.velocity.Y += 0.02f;
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.scale *= 0.994f;

			float light = 0.35f*dust.scale;
			Lighting.AddLight(dust.position, light, 0.8f * light, 0.9f * light);
			if (dust.scale < 0.5f)
			{
				dust.active = false;
			}
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
			=> new Color(lightColor.R, lightColor.G, lightColor.B, 25);
	}
}