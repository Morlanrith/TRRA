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
	public class GambolShroudS : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Gambol Shroud");
			Tooltip.SetDefault("Don't be so dramatic\nRight Click to activate a parry (has a cooldown)\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			item.damage = 180;
			item.melee = true;
			item.width = 44;
			item.height = 44;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 4;
			item.value = Item.sellPrice(gold: 25);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = null;
			item.autoReuse = false;
			item.crit = 36;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
			item.shoot = mod.ProjectileType("GambolBlade");
			item.shootSpeed = 5f;
			item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player)
		{
			if (player.shadowDodgeTimer == 0) return true;
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (!PlayerInput.Triggers.JustPressed.MouseRight) return false; //Equivalent to autoReuse being set to false, as that flag is bugged with alternate use
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/GambolShroud/ShadowClone");
				item.useTime = 30;
				item.useAnimation = 30;
				item.noUseGraphic = false;
				item.channel = false;
				item.shoot = ProjectileID.None;
				player.AddBuff(BuffID.ShadowDodge, 30);
				player.shadowDodgeTimer = 300;
			}
			else
			{
				item.UseSound = null;
				item.useTime = 21;
				item.useAnimation = 21;
				item.noUseGraphic = true;
				item.channel = true;
				item.shoot = mod.ProjectileType("GambolBlade");
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -12);
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<DustExtract>(), 1);
			recipe.AddIngredient(ItemType<DustWeaponKit>(), 1);
			recipe.AddIngredient(ItemType<PlantDustCrystal>(), 20);
			recipe.AddIngredient(ItemType<GravityDustCrystal>(), 20);
			recipe.AddIngredient(ItemID.BlackPaint, 10);
			recipe.AddTile(TileType<DustToolbenchTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				player.channel = true;

				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 30f, 0f);

				return false;
			}
			return true;
		}
	}
}