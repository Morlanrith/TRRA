using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Weapon.GambolShroud;
using Terraria.Audio;
using TRRA.Items.Materials;
using TRRA.Tiles;

namespace TRRA.Items.Weapons
{
	public class GambolShroudG : ModItem
	{
		private bool resetTime = false;

		private static readonly SoundStyle GambolShotSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/GambolShroud/GambolShot")
		{
			Volume = 0.3f,
			Pitch = -0.1f,
		};

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Gambol Shroud");
			Tooltip.SetDefault("'Don't be so dramatic'\nRight Click to fire as a gun\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			Item.damage = 150;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 36;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.noUseGraphic = true;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 7f;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<GambolRibbonEnd>();
			Item.shootSpeed = 10f;
			Item.crit = 8;
			Item.useAmmo = AmmoID.None;
			Item.channel = true;
			Item.autoReuse = false;
			Item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
			// Prevents the player from utilising the scope function with Right Click
			player.scope = false;
        }

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (player.altFunctionUse == 2)
			{
				if (PlayerInput.Triggers.JustReleased.MouseRight) //Stops the animation manually
				{
					resetTime = true;
				}
				if (player.itemAnimation == 1) //Resets the animation so it doesn't let the hand return to resting position
				{
					if (!resetTime)
                    {
						player.itemAnimation = Item.useAnimation;
						SoundEngine.PlaySound(GambolShotSound, player.Center);
					}
					else resetTime = false;
				}
			}
		}

        public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.noUseGraphic = false;
				Item.damage = 130;
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.knockBack = 4;
				Item.UseSound = GambolShotSound;
				Item.shoot = ProjectileID.PurificationPowder;
				Item.shootSpeed = 13f;
				Item.crit = 16;
				Item.useAmmo = AmmoID.Bullet;
				Item.channel = false;
				Item.autoReuse = true;
			}
			else
			{
				Item.noUseGraphic = true;
				Item.damage = 150;
				Item.useTime = 10;
				Item.useAnimation = 10;
				Item.knockBack = 7f;
				Item.UseSound = SoundID.Item1;
				Item.shoot = ProjectileType<GambolRibbonEnd>();
				Item.shootSpeed = 10f;
				Item.crit = 8;
				Item.useAmmo = AmmoID.None;
				Item.channel = true;
				Item.autoReuse = false;
			}
			return base.CanUseItem(player);
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<GambolShadeG>(), 1)
			.AddIngredient(ItemType<PlantDustCrystal>(), 20)
			.AddIngredient(ItemType<GravityDustCrystal>(), 20)
			.AddIngredient(ItemID.BlackPaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(-15, 3);
		}

		// Changes musket balls to high velocity bullets
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2 && type == ProjectileID.Bullet) type = ProjectileID.BulletHighVelocity;
		}

	}
}
