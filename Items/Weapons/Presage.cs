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
	public class Presage : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Presage");
			Tooltip.SetDefault("'The weak die, the strong live. Those are the rules.'");
		}

		public override void SetDefaults() {
			Item.damage = 105;
			Item.DamageType = DamageClass.Melee;
			Item.width = 52;
			Item.height = 50;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.crit = 28;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.shoot = ProjectileType<PresageBlade>();
			Item.shootSpeed = 5f;
			Item.maxStack = 1;
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -12);
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<Forebode>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 20)
			.AddIngredient(ItemType<IceDustCrystal>(), 20)
			.AddIngredient(ItemID.BlackPaint, 5)
			.AddIngredient(ItemID.SoulofFright, 20)
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