using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items
{
	public class CrescentRoseG : ModItem
	{
		public override void SetStaticDefaults() {
			// Sets the display name and tooltip for Crescent Rose (gun form)
			DisplayName.SetDefault("Crescent Rose");
			Tooltip.SetDefault("'It's also a scythe'\nRight Click to zoom out\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			item.damage = 190;
			item.ranged = true;
			item.width = 65;
			item.height = 15;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 7;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Cyan;
			// Plays the sound effect for a Crescent Rose gunshot
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/RoseShot");
			item.autoReuse = false;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 16f;
			item.crit = 26;
			item.useAmmo = AmmoID.Bullet;
		}

		public override void UseStyle(Player player)
		{
			// Allows the player to utilise the scope function with Right Click
			player.scope = true;
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(7, 0);
		}

		// Offsets the fire location of the bullet from the weapons muzzle
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}

	}
}
