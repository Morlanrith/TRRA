using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TRRA.Projectiles.Item.Weapon.Harbinger;
using Terraria.Audio;
using TRRA.Tiles;
using TRRA.Items.Materials;
using Terraria.GameInput;
using TRRA.Dusts;

namespace TRRA.Items.Weapons
{
	public class HarbingerSw : ModItem
	{
		private bool resetTime = false;

		private static readonly SoundStyle HarbingerSliceSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Harbinger/HarbingerSlice")
		{
			Volume = 0.4f,
			Pitch = 0f,
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harbinger");
			Tooltip.SetDefault("'Doubles as a bad luck charm'\nRight Click to fire as a gun\nTransforms by pressing a mapped hotkey");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 220;
			Item.crit = 14;
			Item.DamageType = DamageClass.Melee;
			Item.width = 62;
			Item.height = 64;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = HarbingerSliceSound;
			Item.shoot = ProjectileType<HarbingerBB>();
			Item.shootSpeed = 7f;
			Item.autoReuse = true;
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
						SoundEngine.PlaySound(SoundID.Item38, player.Center);
					}
					else resetTime = false;
				}
			}
		}

        public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.noUseGraphic = true;
				Item.damage = 110;
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.knockBack = 5;
				Item.DamageType = DamageClass.Ranged;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.UseSound = SoundID.Item38;
				Item.shoot = ProjectileID.Bullet;
				Item.shootSpeed = 13f;
				Item.crit = 8;
				Item.useAmmo = AmmoID.Bullet;
				Item.noMelee = true;
			}
			else
			{
				Item.noUseGraphic = false;
				Item.damage = 220;
				Item.useTime = 25;
				Item.useAnimation = 25;
				Item.knockBack = 6;
				Item.DamageType = DamageClass.Melee;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.UseSound = HarbingerSliceSound;
				Item.shoot = ProjectileType<HarbingerBB>();
				Item.shootSpeed = 7f;
				Item.crit = 14;
				Item.useAmmo = AmmoID.None;
				Item.noMelee = false;
			}
			return base.CanUseItem(player);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(20))
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<CrowFeathers>());
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Vector2 bullet1Pos = position;
				bullet1Pos.Y += 2;
				bullet1Pos = bullet1Pos.RotatedBy(velocity.ToRotation(), position);
				Vector2 bullet2Pos = position;
				bullet2Pos.Y -= 2;
				bullet2Pos = bullet2Pos.RotatedBy(velocity.ToRotation(), position);
				Projectile.NewProjectile(source, bullet1Pos, velocity, type, damage, Item.knockBack, player.whoAmI);
				Projectile.NewProjectile(source, bullet2Pos, velocity, type, damage, Item.knockBack, player.whoAmI);
				Projectile.NewProjectile(source, position, velocity, ProjectileType<HarbingerG>(), damage, Item.knockBack, player.whoAmI);
			}
			else
				Projectile.NewProjectile(source, position, velocity, type, (int)(220 * player.GetDamage(DamageClass.Melee).Additive) / 3, Item.knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<Portent>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 20)
			.AddIngredient(ItemID.GrayPaint, 5)
			.AddIngredient(ItemID.RedPaint, 5)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

	}
}