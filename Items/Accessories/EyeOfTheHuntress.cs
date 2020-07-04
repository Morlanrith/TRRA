using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace TRRA.Items.Accessories
{
	public class EyeOfTheHuntress : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Eye of the Huntress");
			Tooltip.SetDefault("Strike a Grimm down with but a mere sight\n12% increased melee damage\n12% increased ranged damage\n8% increased critical strike chance");
		}

		public override void SetDefaults() {
			item.width = 28;
			item.height = 28;
			item.accessory = true;
			item.value = Item.sellPrice(gold: 9);
			item.rare = ItemRarityID.Pink;
			item.maxStack = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.meleeDamageMult += 0.12f;
			player.rangedDamageMult += 0.12f;
			player.meleeCrit += 8;
			player.rangedCrit += 8;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<HuntressEmblem>(), 1);
			recipe.AddIngredient(ItemID.EyeoftheGolem, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
