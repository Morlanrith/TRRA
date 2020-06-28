using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items
{
	public class Myrtenaster : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Myrtenaster");
			Tooltip.SetDefault("For those who are more than just a name\nRight Click to fire a summoned sword\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			item.damage = 30;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.useAnimation = 12;
			item.useTime = 2;
			item.knockBack = 3.5f;
			item.width = 70;
			item.height = 70;
			item.scale = 0.65f;
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(gold: 10);
			item.melee = true;
			item.autoReuse = true;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/IceStab");
			item.shoot = mod.ProjectileType("MyrtenasterR");
			item.shootSpeed = 6f;
			item.maxStack = 1;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DD2SquireBetsySword, 1);
			recipe.AddIngredient(ItemID.NorthPole, 1);
			recipe.AddIngredient(ItemID.WhitePaint, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool AltFunctionUse(Player player) {
			return true;
		}


		public override void HoldItem(Player player)
		{
			if (player.altFunctionUse == 2) player.itemRotation = 0f;
		}

		[Obsolete]
		public override void GetWeaponDamage(Player player, ref int damage)
		{
			if (player.altFunctionUse == 2)
			{
				if (player.itemAnimation == 1) //Resets the animation so it doesn't let the hand return to resting position
				{
					player.itemAnimation = item.useAnimation;
					Main.PlaySound(item.UseSound, player.Center);
				}
				if (PlayerInput.Triggers.JustReleased.MouseRight) //Stops the animation manually
				{
					player.itemAnimation = 0;
				}
			}
		}


		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.damage = 200;
				item.useTime = 30;
				item.useAnimation = 30;
				item.melee = false;
				item.ranged = true;
				item.shoot = mod.ProjectileType("MyrtenasterS");
				item.noMelee = true;
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/IceSword");
			}
			else {
				item.useStyle = ItemUseStyleID.Stabbing;
				item.noMelee = false;
				item.useAnimation = 12;
				item.melee = true;
				item.ranged = false;
				item.useTime = 2;
				item.damage = 30;
				item.shoot = mod.ProjectileType("MyrtenasterR");
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/IceStab");
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(7, -12);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				float posY = player.position.Y+25;
				float posX = player.position.X-33;
				if (player.direction == 1) posX += 83;
				Random r = new Random();
				posY += r.Next(-20, 20);
				speedX = new Vector2(speedX, speedY).Length() * (speedX > 0 ? 1 : -1);
				Projectile.NewProjectile(posX, posY, speedX, 0, type, 30, knockBack, player.whoAmI);
			}
			else Projectile.NewProjectile(position.X, position.Y, speedX, 0, type, 200, knockBack, player.whoAmI);
			return false;
		}
	}
}