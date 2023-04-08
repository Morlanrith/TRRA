using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Tiles;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items.Weapons
{
	public class PlantBullet : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Plant Dust Bullet");
			// Tooltip.SetDefault("A special kind of bullet that uses plant dust");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults() {
			Item.damage = 14;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 1.0f;
			Item.value = Item.sellPrice(copper: 50);
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ProjectileType<Projectiles.Item.Weapon.PlantBullet>();
			Item.shootSpeed = 5f;
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes() => CreateRecipe(50)
			.AddIngredient(ItemID.EmptyBullet, 50)
            .AddIngredient(ItemType<Materials.PlantDustCrystal>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();
	}
}
