using TRRA.Dusts;
using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.DataStructures;

namespace TRRA.Items.Weapons
{
    public class CrescentRoseN : ModItem
	{
		private bool canSwing = true;

		private static readonly SoundStyle RoseSliceSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseSlice")
		{
			Volume = 0.4f,
			Pitch = 0.3f,
		};

		private static readonly SoundStyle RoseShotSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseShot")
		{
			Volume = 0.6f,
			Pitch = 0.3f,
		};

		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Crescent Rose (Nightmare)");
			// Tooltip.SetDefault("'Wait, that's not right...'\nRight Click to fire as a gun");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.damage = 240;
			Item.width = 66;
			Item.height = 56;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.Lime;
			Item.crit = 20;
			Item.autoReuse = true;
			Item.shootSpeed = 16f;
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

		private void ResetValues()
		{
			Item.autoReuse = true;
			Item.damage = 240;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = false;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.shoot = ProjectileID.None;
			Item.useAmmo = AmmoID.None;
			Item.UseSound = RoseSliceSound;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (!PlayerInput.Triggers.JustPressed.MouseRight) return false;
				canSwing = false;
				Item.damage = 200;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.DamageType = DamageClass.Ranged;
				Item.noMelee = true;
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.autoReuse = false;
				Item.shoot = ProjectileID.Bullet;
				Item.useAmmo = AmmoID.Bullet;
				Item.UseSound = RoseShotSound;
			}
			else
            {
				if (!canSwing)
				{
					ResetValues();
					canSwing = true;
					return false;
				}
				ResetValues();
				Item.autoReuse = true;
			}
			return base.CanUseItem(player);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(10))
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<RosePetal>());
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 10)
			.AddIngredient(ItemID.RedPaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			if (player.altFunctionUse == 2)
				velocity = velocity.RotatedBy(MathHelper.ToRadians(45 * player.direction)); // Rotates CR so its being held correctly
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.altFunctionUse == 2) // Fires the shot backwards
				Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-45 * player.direction)) * -1, type, damage, Item.knockBack, player.whoAmI);
			return false;
        }
		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(-23, 0);
		}
	}



}