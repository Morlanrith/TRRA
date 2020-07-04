using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Accessories
{
	public class HuntressEmblem : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Huntress Emblem");
			Tooltip.SetDefault("For the best and brightest warriors of Remnant\n14% increased melee damage\n14% increased ranged damage");
		}

		public override void SetDefaults() {
			item.width = 28;
			item.height = 28;
			item.accessory = true;
			item.value = Item.sellPrice(gold: 4);
			item.rare = ItemRarityID.Pink;
			item.maxStack = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.meleeDamageMult += 0.14f;
			player.rangedDamageMult += 0.14f;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WarriorEmblem, 1);
			recipe.AddIngredient(ItemID.RangerEmblem, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
