using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TRRA.Items.Consumables;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items
{
	// Adapted from tModLoaders Example Mod
	public class PrefixChanceGlobalItem : GlobalItem
	{

		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[ItemID.BloodMoonStarter] = ItemType<MoonSummoner>();
        }


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
