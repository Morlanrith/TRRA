using TRRA.Dusts;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace TRRA.Tiles
{
	public class GrimmWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() {
			return ModContent.Find<ModWaterfallStyle>("TRRA/GrimmWaterfallStyle").Slot;
		}

		public override int GetSplashDust() {
			return ModContent.DustType<GrimmDroplets>();
		}

		public override int GetDropletGore() {
            switch (Main.rand.Next(4))
            {
                case 4:
                    return Mod.Find<ModGore>("GrimmWater_BeoHead").Type;
				case 3:
					return Mod.Find<ModGore>("GrimmWater_BeoLeg").Type;
				case 2:
					return Mod.Find<ModGore>("GrimmWater_CreepHead").Type;
				case 1:
					return Mod.Find<ModGore>("GrimmWater_CreepTorso").Type;
				default:
					return Mod.Find<ModGore>("GrimmWater_LancerTalon").Type;
			}
        }

        public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
			r = .8f;
			g = .5f;
			b = .5f;
		}

		public override byte GetRainVariant() {
			return (byte)Main.rand.Next(3);
		}

		public override Asset<Texture2D> GetRainTexture() {
			return ModContent.Request<Texture2D>("TRRA/Tiles/GrimmRain");
		}

        public override string Texture => "TRRA/Tiles/GrimmWaterStyle";
    }
}