using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Consumables
{
	public class MoonSummoner : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Summons the Shattered Moon");
		}

		public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 20;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override bool? UseItem(Player player)
        {
            if (!TRRAWorld.ShatteredMoon && !Main.dayTime && !Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon)
            {
                Main.NewText("The Shattered Moon rises...", 186, 34, 64);
                TRRAWorld.ShatteredMoon = true;
                return true;
            }
            return false;
        }
    }
}