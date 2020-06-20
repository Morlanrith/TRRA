using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items
{
	public class MyrtenasterF : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Myrtenaster (Fire)");
			Tooltip.SetDefault("For those who are more than just a name\nRight Click to shoot a wave of fire\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			item.damage = 25;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.useAnimation = 12;
			item.useTime = 2;
			item.knockBack = 4.5f;
			item.width = 32;
			item.height = 32;
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(gold: 10);
			item.melee = true;
			item.autoReuse = true;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/FireStab");
			item.shoot = mod.ProjectileType("MyrtenasterR");
			item.shootSpeed = 6f;
			item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (player.altFunctionUse != 2)
			{
				target.AddBuff(BuffID.OnFire, 180);
			}
		}


		public override void HoldItem(Player player)
		{
			if (player.altFunctionUse == 2) player.itemRotation = 0f;
		}


		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.melee = false;
				item.noMelee = true;
				item.ranged = true;
				item.useTime = 45;
				item.useAnimation = 45;
				item.damage = 135;
				item.knockBack = 7.5f;
				item.shoot = mod.ProjectileType("MyrtenasterFS");
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/FireWave");
			}
			else {
				item.useStyle = ItemUseStyleID.Stabbing;
				item.melee = true;
				item.noMelee = false;
				item.ranged = false;
				item.useTime = 2;
				item.useAnimation = 12;
				item.damage = 25;
				item.knockBack = 4.5f;
				item.shoot = mod.ProjectileType("MyrtenasterFR");
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/FireStab");
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -12);
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
				Projectile.NewProjectile(posX, posY, speedX, 0, type, damage, knockBack, player.whoAmI);
				return false;
			}
			else return true;
		}
	}
}