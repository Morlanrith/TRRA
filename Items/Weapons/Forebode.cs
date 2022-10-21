using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;
using TRRA.Projectiles.Item.Weapon.Omen;
using Terraria.DataStructures;

namespace TRRA.Items.Weapons
{
	public class Forebode : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Forebode");
			Tooltip.SetDefault("'Strength is all that matters in a fight'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 26;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 52;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(silver: 54);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.crit = 16;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.shoot = ProjectileType<ForebodeBlade>();
			Item.shootSpeed = 5f;
			Item.maxStack = 1;
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -12);
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.channel = true;
			Projectile.NewProjectile(source, position, velocity, type, damage, Item.knockBack, player.whoAmI, 30f, 0f);
			return false;
		}
	}
}