using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TRRA.Items
{
	// Adapted from tModLoaders Example Mod
	public class PrefixChanceGlobalItem : GlobalItem
	{
		public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand) 
		{
			// Checks if the item is from TRRA, and prevents it from having prefixes/being reforged (causes issues with weapon transforming)
			if (item.ModItem?.Mod == Mod && item.ModItem?.Item.accessory == false)
			{
				if (pre == -3 || pre == -1) return false;
			}
			return null;
		}
	}
}
