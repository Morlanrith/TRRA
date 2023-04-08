using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;
using Terraria.DataStructures;
using TRRA.Projectiles.Item.Weapon.EmberCelica;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class SteelCelica : ModItem
	{
		private static readonly SoundStyle EmberShotSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/EmberCelica/EmberShot")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Steel Celica");
			// Tooltip.SetDefault("'Instead of sweetheart, you can just call me SIR'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 58;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 7;
			Item.noUseGraphic = true;
			Item.value = Item.sellPrice(silver: 54);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = EmberShotSound;
			Item.autoReuse = false;
			Item.shoot = ProjectileType<EmberPunch>();
			Item.shootSpeed = 9f;
			Item.noMelee = true;
			Item.crit = 6;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 30)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			// Prevents the player from utilising the scope function with Right Click
			player.scope = false;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (!player.mount.Active) Projectile.NewProjectile(source, position, velocity * .25f, ProjectileType<EmberPunch>(), damage, 8, player.whoAmI);
			return false; // return false because we don't want to shoot automatic projectile
		}

	}
}