using Terraria;
using Terraria.ModLoader;

namespace TRRA.Biomes
{
	// This is NOT a real biome, it's merely a fake class to hold Bestiary information for the Shattered Moon event
	public class ShatteredMoonFakeBiome : ModBiome
	{

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shattered Moon");
		}

		public override bool IsBiomeActive(Player player) {
			return false; // Not a real biome, so it's never active
		}
	}
}
