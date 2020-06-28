using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class EmberCelicaR : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ember Celica (Rocket)");
			Tooltip.SetDefault("Armed and Ready\nRight Click to fire a splitting Rocket\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			item.damage = 50;
			item.ranged = true;
			item.width = 62;
			item.height = 34;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 6;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Cyan;
			item.autoReuse = false;
			item.shoot = ProjectileID.RocketI;
			item.shootSpeed = 9f;
			item.useAmmo = AmmoID.Rocket;
			item.noUseGraphic = true;
			item.noMelee = true;
		}

		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2)
			{
				if (!PlayerInput.Triggers.JustPressed.MouseRight) return false; //Equivalent to autoReuse being set to false, as that flag is bugged with alternate use
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/RocketTriple");
			}
			else item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/RocketSingle");
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{		
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
			if (type == ProjectileID.RocketIII || type == ProjectileID.RocketIV) damage += 40;
			if (player.altFunctionUse == 2) type = ProjectileType<Projectiles.TripEmberRocket>();
			else type = ProjectileType<Projectiles.EmberRocket>();
			return true;
		}

	}
}