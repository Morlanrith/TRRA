using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Weapons
{
	public class FireBullet : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Fire Dust Bullet");
			Tooltip.SetDefault("A special kind of bullet that uses fire dust");
		}

		public override void SetDefaults() {
			item.damage = 13;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 2.0f;
			item.value = Item.sellPrice(copper: 50);
			item.rare = ItemRarityID.Orange;
			item.shoot = ProjectileType<Projectiles.Item.Weapon.FireBullet>();
			item.shootSpeed = 5f;
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EmptyBullet, 50);
            recipe.AddIngredient(ItemType<Materials.FireDustCrystal>(), 1);
			recipe.AddTile(TileID.MythrilAnvil); // To be replaced with a custom workbench for dust
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
