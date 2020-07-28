using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;

namespace TRRA.Items.Weapons
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class EmberCelicaS : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ember Celica");
			Tooltip.SetDefault("Armed and Ready\nRight Click to Charge\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			item.damage = 90;
			item.melee = true;
			item.width = 62;
			item.height = 34;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 7;
			item.noUseGraphic = true;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/EmberCelica/EmberShot");
			item.autoReuse = false;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 9f;
			item.useAmmo = AmmoID.Bullet;
			item.noMelee = true;
			item.crit = 26;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<DustExtract>(), 1);
			recipe.AddIngredient(ItemType<DustWeaponKit>(), 1);
			recipe.AddIngredient(ItemType<FireDustCrystal>(), 30);
			recipe.AddIngredient(ItemType<GravityDustCrystal>(), 10);
			recipe.AddIngredient(ItemID.YellowPaint, 10);
			recipe.AddTile(TileType<DustToolbenchTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool AltFunctionUse(Player player) {
			if (player.mount.Active) return false;
			return true;
		}

		public override void UseStyle(Player player)
		{
			// Prevents the player from utilising the scope function with Right Click
			player.scope = false;
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				if (!PlayerInput.Triggers.JustPressed.MouseRight) return false; //Equivalent to autoReuse being set to false, as that flag is bugged with alternate use
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/EmberCelica/EmberDash");
				item.useStyle = ItemUseStyleID.Stabbing;
				item.shoot = ProjectileID.PurificationPowder;
				item.useTime = 40;
				item.useAnimation = 40;
				item.useAmmo = AmmoID.None;
				Vector2 newVelocity = player.velocity;
				newVelocity.X = 8.5f * player.direction;
				player.velocity = newVelocity;
			}
			else {
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/EmberCelica/EmberShot");
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.shoot = ProjectileID.Bullet;
				item.useTime = 20;
				item.useAnimation = 20;
				item.useAmmo = AmmoID.Bullet;
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            if (player.altFunctionUse != 2)
            {
				Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
				if (!player.mount.Active) Projectile.NewProjectile(position.X, position.Y, speedX * .25f, speedY * .25f, mod.ProjectileType("EmberPunch"), 180 + (int)(180 * player.meleeDamageMult), 8, player.whoAmI);
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
				int numberProjectiles = 4 + Main.rand.Next(2); // 4 or 5 shots
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = muzzleOffset.RotatedByRandom(MathHelper.ToRadians(30)); // 30 degree spread.
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
			}
			else Projectile.NewProjectile(position.X, position.Y, speedX * .25f, speedY * .25f, mod.ProjectileType("EmberPunch"), 150 + (int)(150 * player.meleeDamageMult), 12, player.whoAmI, 1);
			return false; // return false because we don't want tmodloader to shoot projectile
		}

	}
}