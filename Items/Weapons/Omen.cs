using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;
using TRRA.Projectiles.Item.Weapon.Omen;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace TRRA.Items.Weapons
{
	public class Omen : ModItem
	{
		private bool canPortal = true;

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Omen");
			Tooltip.SetDefault("'Now that's a katana!'");
		}

		public override void SetDefaults() {
			Item.damage = 195;
			Item.DamageType = DamageClass.Melee;
			Item.width = 52;
			Item.height = 50;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.buffType = BuffType<PortalBuff>();
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.crit = 40;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.shoot = ProjectileType<OmenBlade>();
			Item.shootSpeed = 5f;
			Item.maxStack = 1;
		}

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }

		private void ResetValues()
		{
			Item.channel = false;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.shoot = ProjectileType<OmenPortal>();
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (PlayerInput.Triggers.JustReleased.MouseRight) canPortal = false;
			if (player.altFunctionUse != 2 && player.itemAnimation == 0)
			{
				ResetValues();
				canPortal = true;
			}
		}

		public override bool CanUseItem(Player player)
		{
			if (!canPortal && player.itemAnimation == 0) return false;

			if (player.altFunctionUse == 2)
				ResetValues();
			else
			{
				Item.channel = true;
				Item.useTime = 21;
				Item.useAnimation = 21;
				Item.shoot = ProjectileType<OmenBlade>();
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -12);
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<GambolShadeS>(), 1) // Change to child Omen
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemID.YellowPaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				if (!player.HasBuff(Item.buffType))
				{
					player.AddBuff(Item.buffType, 2);
					Projectile.NewProjectile(source, player.Top, new Vector2(0.0f), type, damage, Item.knockBack, player.whoAmI, 30f, 0f);
				}
				else
                {

                }
			}
			else
			{
				player.channel = true;
				Projectile.NewProjectile(source, position, velocity, type, damage, Item.knockBack, player.whoAmI, 30f, 0f);
			}
			return false;
		}
	}
}