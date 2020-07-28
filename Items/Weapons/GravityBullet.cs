using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Tiles;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Weapons
{
	public class GravityBullet : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Gravity Dust Bullet");
			Tooltip.SetDefault("A special kind of bullet that uses gravity dust");
		}

		public override void SetDefaults() {
			item.damage = 17;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 8.0f;
			item.value = Item.sellPrice(copper: 50);
			item.rare = ItemRarityID.Orange;
			item.shoot = ProjectileType<Projectiles.Item.Weapon.GravityBullet>();
			item.shootSpeed = 5f;
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EmptyBullet, 50);
            recipe.AddIngredient(ItemType<Materials.GravityDustCrystal>(), 1);
			recipe.AddTile(TileType<DustToolbenchTile>());
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
