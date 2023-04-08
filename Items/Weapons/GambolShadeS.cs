using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;
using TRRA.Projectiles.Item.Weapon.GambolShroud;
using Terraria.DataStructures;

namespace TRRA.Items.Weapons
{
	public class GambolShadeS : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 110;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 42;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.crit = 24;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.shoot = ProjectileType<VeilBlade>();
			Item.shootSpeed = 5f;
			Item.maxStack = 1;
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -12);
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<GambolVeil>(), 1)
			.AddIngredient(ItemType<PlantDustCrystal>(), 20)
			.AddIngredient(ItemType<GravityDustCrystal>(), 20)
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