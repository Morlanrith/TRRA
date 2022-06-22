using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Weapons
{
	public class CrescentRoseG : ModItem
	{
		private static readonly SoundStyle RoseShotSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseShot")
		{
			Volume = 0.6f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			// Sets the display name and tooltip for Crescent Rose (gun form)
			DisplayName.SetDefault("Crescent Rose");
			Tooltip.SetDefault("'It's also a scythe'\nRight Click to zoom out\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			Item.damage = 200;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 68;
			Item.height = 18;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			// Plays the sound effect for a Crescent Rose gunshot
			Item.UseSound = RoseShotSound;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 16f;
			Item.crit = 26;
			Item.useAmmo = AmmoID.Bullet;
			Item.maxStack = 1;
		}

		public override void UseStyle(Player player, Rectangle r)
		{
			// Allows the player to utilise the scope function with Right Click
			player.scope = true;
		}

		public override void HoldItemFrame(Player player)
		{
			player.scope = true;
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(4, 0);
		}

		// Offsets the fire location of the bullet from the weapons muzzle
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
		}

	}
}
