using System;
using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;
using TRRA.Projectiles.Item.Weapon.Myrtenaster;
using Terraria.DataStructures;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
	public class Myrtenaster : ModItem
	{
		private bool resetTime = false;

		private static readonly SoundStyle IceStabSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Myrtenaster/IceStab")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle IceSwordSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Myrtenaster/IceSword")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Myrtenaster");
			Tooltip.SetDefault("For those who are more than just a name\nRight Click to fire a summoned sword\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			Item.damage = 30;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useAnimation = 12;
			Item.useTime = 3;
			Item.knockBack = 3.5f;
			Item.width = 46;
			Item.height = 46;
			Item.scale = 0.9f;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(gold: 25);
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
			Item.UseSound = IceSwordSound;
			Item.shoot = ProjectileType<MyrtenasterR>();
			Item.shootSpeed = 6f;
			Item.maxStack = 1;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 20)
			.AddIngredient(ItemType<IceDustCrystal>(), 20)
			.AddIngredient(ItemID.WhitePaint, 10)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

		public override bool AltFunctionUse(Player player) {
			return true;
		}


		public override void HoldItem(Player player)
		{
			if (player.altFunctionUse == 2) player.itemRotation = 0f;
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
						SoundEngine.PlaySound(IceSwordSound, player.Center);
					}
					else resetTime = false;
				}
			}
		}


        public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.damage = 200;
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.DamageType = DamageClass.Ranged;
				Item.shoot = ProjectileType<MyrtenasterS>();
				Item.noMelee = true;
				Item.UseSound = IceSwordSound;
			}
			else {
				Item.useStyle = ItemUseStyleID.Thrust;
				Item.noMelee = false;
				Item.useAnimation = 12;
				Item.DamageType = DamageClass.Melee;
				Item.useTime = 3;
				Item.damage = 30;
				Item.shoot = ProjectileType<MyrtenasterR>();
				Item.UseSound = IceStabSound;
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -11);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				float posY = player.position.Y + 25;
				float posX = player.position.X - 33;
				if (player.direction == 1) posX += 83;
				Random r = new Random();
				posY += r.Next(-20, 20);
				velocity.X = new Vector2(velocity.X, velocity.Y).Length() * (velocity.X > 0 ? 1 : -1);
				Projectile.NewProjectile(source, posX, posY, velocity.X, 0, type, (int)(20 * player.GetDamage(DamageClass.Melee).Multiplicative), Item.knockBack, player.whoAmI);
			}
			else
			{
				Projectile.NewProjectile(source, position, velocity, type, (int)(200 * player.GetDamage(DamageClass.Ranged).Multiplicative), Item.knockBack, player.whoAmI);
			}
			return false;
		}
	}
}