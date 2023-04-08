using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
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
	public class EmberCelicaS : ModItem
	{
		private static readonly SoundStyle EmberShotSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/EmberCelica/EmberShot")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle EmberDashSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/EmberCelica/EmberDash")
		{
			Volume = 0.3f,
			Pitch = -0.1f,
		};

		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 90;
			Item.DamageType = DamageClass.Melee;
			Item.width = 62;
			Item.height = 34;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 7;
			Item.noUseGraphic = true;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = EmberShotSound;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 9f;
			Item.useAmmo = AmmoID.Bullet;
			Item.noMelee = true;
			Item.crit = 26;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<SparkCelica>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 30)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemID.YellowPaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

		public override bool AltFunctionUse(Player player) {
			if (player.mount.Active) return false;
			return true;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			// Prevents the player from utilising the scope function with Right Click
			player.scope = false;
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				if (!PlayerInput.Triggers.JustPressed.MouseRight) return false; //Equivalent to autoReuse being set to false, as that flag is bugged with alternate use
				Item.UseSound = EmberDashSound;
				Item.useStyle = ItemUseStyleID.Thrust;
				Item.shoot = ProjectileID.PurificationPowder;
				Item.useTime = 40;
				Item.useAnimation = 40;
				Item.useAmmo = AmmoID.None;
				Vector2 newVelocity = player.velocity;
				newVelocity.X = 8.5f * player.direction;
				player.velocity = newVelocity;
			}
			else {
				if (!PlayerInput.Triggers.JustPressed.MouseLeft) return false; //Equivalent to autoReuse being set to false, as that flag is bugged with alternate use
				Item.UseSound = EmberShotSound;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.shoot = ProjectileID.Bullet;
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.useAmmo = AmmoID.Bullet;
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
            {
				Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
				Projectile.NewProjectile(source, position, velocity * .25f, ProjectileType<EmberPunch>(), (int)(280 * player.GetDamage(DamageClass.Melee).Additive), 8, player.whoAmI);
				int numberProjectiles = 4 + Main.rand.Next(2); // 4 or 5 shots
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = muzzleOffset.RotatedByRandom(MathHelper.ToRadians(30)); // 30 degree spread.
					Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, Item.knockBack, player.whoAmI);
				}
			}
			else Projectile.NewProjectile(source, position, velocity * .25f, ProjectileType<EmberPunch>(), (int)(300 * player.GetDamage(DamageClass.Melee).Additive), 12, player.whoAmI, 1);
			return false; // return false because we don't want to shoot automatic projectile
		}

	}
}