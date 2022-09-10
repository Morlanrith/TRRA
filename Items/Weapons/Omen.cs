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
using Terraria.GameContent;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
	public class Omen : ModItem
	{
		private bool canPortal = true;
		private int portalID = -1;

		private static readonly SoundStyle PortalOpenSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Omen/OmenPortalOpen")
		{
			Volume = 0.8f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle PortalCloseSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Omen/OmenPortalClose")
		{
			Volume = 1.4f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Omen");
			Tooltip.SetDefault("'Strong enough to do what others won't'\nRight Click to create a portal");
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
			Item.useAnimation = 1;
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

		private static void Warp(Vector2 newPos, Player player)
		{
			try
			{
				player.grappling[0] = -1;
				player.grapCount = 0;
				for (int i = 0; i < 1000; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].aiStyle == 7)
					{
						Main.projectile[i].Kill();
					}
				}
				float num = MathHelper.Clamp(1f - player.teleportTime * 0.99f, 0.01f, 1f);
				Vector2 position = player.position;
				for (int i = 0; i < Main.rand.Next(4, 7); i++)
					Dust.NewDust(player.Top, player.width, player.height, DustID.RedTorch);
				float num2 = Vector2.Distance(player.position, newPos);
				PressurePlateHelper.UpdatePlayerPosition(player);
				player.Center = newPos;
				player.fallStart = (int)(player.position.Y / 16f);
				if (player.whoAmI == Main.myPlayer)
				{
					bool flag = false;
					if (num2 < new Vector2(Main.screenWidth, Main.screenHeight).Length() / 2f + 100f)
					{
						Main.SetCameraLerp(0.1f, 10);
						flag = true;
					}
					else
					{
						Main.BlackFadeIn = 255;
						Main.screenLastPosition = Main.screenPosition;
						Main.screenPosition.X = player.position.X + (float)(player.width / 2) - (float)(Main.screenWidth / 2);
						Main.screenPosition.Y = player.position.Y + (float)(player.height / 2) - (float)(Main.screenHeight / 2);
					}
					if (num > 0.1f || !flag)
					{
						if (Main.mapTime < 5)
						{
							Main.mapTime = 5;
						}
						Main.maxQ = true;
					}
				}
				PressurePlateHelper.UpdatePlayerPosition(player);
				for (int i = 0; i < 3; i++) player.UpdateSocialShadow();
				player.oldPosition = player.position + player.BlehOldPositionFixer;
				SoundEngine.PlaySound(PortalCloseSound);
			}
			catch
			{
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
			.AddIngredient(ItemType<Presage>(), 1)
			.AddIngredient(ItemType<PlantDustCrystal>(), 20)
			.AddIngredient(ItemType<GravityDustCrystal>(), 20)
			.AddIngredient(ItemID.BlackPaint, 10)
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
					portalID = Projectile.NewProjectile(source, player.Top, new Vector2(0.0f), type, damage, Item.knockBack, player.whoAmI, 30f, 0f);
					SoundEngine.PlaySound(PortalOpenSound);
					for (int i = 0; i < Main.rand.Next(5, 10); i++)
						Dust.NewDust(Main.projectile[portalID].position, Main.projectile[portalID].width, Main.projectile[portalID].height, DustID.RedTorch);
				}
				else
                {
					Warp(Main.projectile[portalID].Center, player);
					Main.projectile[portalID].Kill();
					portalID = -1;
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