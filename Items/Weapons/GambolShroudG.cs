using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Weapons
{
	public class GambolShroudG : ModItem
	{
		private bool resetTime = false;
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Gambol Shroud");
			Tooltip.SetDefault("Don't be so dramatic\nRight Click to fire as a gun\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			item.damage = 130;
			item.ranged = true;
			item.width = 36;
			item.height = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.value = Item.sellPrice(gold: 25);
			item.rare = ItemRarityID.Cyan;
			item.noUseGraphic = true;
			item.useTime = 10;
			item.useAnimation = 10;
			item.knockBack = 7f;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("GambolRibbonEnd");
			item.shootSpeed = 10f;
			item.crit = 0;
			item.useAmmo = AmmoID.None;
			item.channel = true;
			item.autoReuse = false;
			item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void UseStyle(Player player)
		{
			// Prevents the player from utilising the scope function with Right Click
			player.scope = false;
		}

        [System.Obsolete]
        public override void GetWeaponDamage(Player player, ref int damage)
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
						player.itemAnimation = item.useAnimation;
						Main.PlaySound(item.UseSound, player.Center);
					}
					else resetTime = false;
				}
			}
		}

        public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.noUseGraphic = false;
				item.damage = 120;
				item.useTime = 20;
				item.useAnimation = 20;
				item.knockBack = 4;
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/GambolShroud/GambolShot");
				item.shoot = ProjectileID.PurificationPowder;
				item.shootSpeed = 13f;
				item.crit = 10;
				item.useAmmo = AmmoID.Bullet;
				item.channel = false;
				item.autoReuse = true;
			}
			else
			{
				item.noUseGraphic = true;
				item.damage = 130;
				item.useTime = 10;
				item.useAnimation = 10;
				item.knockBack = 7f;
				item.UseSound = SoundID.Item1;
				item.shoot = mod.ProjectileType("GambolRibbonEnd");
				item.shootSpeed = 10f;
				item.crit = 0;
				item.useAmmo = AmmoID.None;
				item.channel = true;
				item.autoReuse = false;
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(-15, 3);
		}

		// Offsets the fire location of the bullet from the weapons muzzle
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
				if (type == ProjectileID.Bullet) type = ProjectileID.BulletHighVelocity;
			}
			return true;
		}

	}
}
