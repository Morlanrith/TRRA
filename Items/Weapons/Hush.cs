using TRRA.Dusts;
using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameInput;
using TRRA.Projectiles.Item.Weapon.Hush;
using Terraria.DataStructures;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
    public class Hush : ModItem
	{
		private bool canTeleport;

		private static readonly SoundStyle HushOpenSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Hush/HushOpen")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle HushShatterSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Hush/HushShatter")
		{
			Volume = 0.4f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle HushStabSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Hush/HushStab")
		{
			Volume = 0.4f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Hush");
			Tooltip.SetDefault("'I scream, you scream...'\nRight Click to teleport to the position of the mouse");
		}

		public override void SetDefaults() 
		{
			Item.width = 51;
			Item.height = 51;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(gold: 25);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.damage = 160;
			Item.crit = 62;
			Item.knockBack = 5f;
			Item.UseSound = HushStabSound;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<HushClosed>();
			Item.noUseGraphic = true;
			Item.shootSpeed = 5f;
			Item.noMelee = true;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
            return true;
		}

		private static void ShatterEffect(Rectangle effectRect, float teleportTime)
        {
			int num3 = effectRect.Width * effectRect.Height / 5;
			float num = MathHelper.Clamp(1f - teleportTime * 0.99f, 0.01f, 1f);
			num3 = (int)((float)num3 * num);
			for (int k = 0; k < num3; k++)
			{
				int num4 = Dust.NewDust(new Vector2(effectRect.X, effectRect.Y), effectRect.Width, effectRect.Height, DustType<Shatter>());
				Main.dust[num4].scale = (float)Main.rand.Next(20, 70) * 0.01f;
				if (k < 10)
				{
					Main.dust[num4].scale += 0.25f;
				}
				if (k < 5)
				{
					Main.dust[num4].scale += 0.25f;
				}
			}
		}

		private static void Shatter(Vector2 newPos, Player player)
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
                ShatterEffect(player.getRect(), player.teleportTime);
				float num2 = Vector2.Distance(player.position, newPos);
				PressurePlateHelper.UpdatePlayerPosition(player);
				player.position = newPos;
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
                ShatterEffect(player.getRect(), player.teleportTime);
				SoundEngine.PlaySound(HushShatterSound);
			}
			catch
			{
			}
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (PlayerInput.Triggers.JustReleased.MouseRight) canTeleport = false;
			if (player.altFunctionUse != 2 && player.itemAnimation == 0) canTeleport = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (!canTeleport) return false;
			// If the player uses the alt function (Right Click), causes the player to teleport
			if (player.altFunctionUse == 2)
			{
				if (player.chaosState || !PlayerInput.Triggers.JustPressed.MouseRight) return false;
				Vector2 vector = default;
				vector.X = (float)Main.mouseX + Main.screenPosition.X;
				if (player.gravDir == 1f) vector.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;		
				else vector.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
				vector.X -= player.width / 2;
				if (!(vector.X > 50f) || !(vector.X < (float)(Main.maxTilesX * 16 - 50)) || !(vector.Y > 50f) || !(vector.Y < (float)(Main.maxTilesY * 16 - 50))) return false;
				int num = (int)(vector.X / 16f);
				int num2 = (int)(vector.Y / 16f);
				if ((Main.tile[num, num2].WallType == 87 && (double)num2 > Main.worldSurface && !NPC.downedPlantBoss) || Collision.SolidCollision(vector, player.width, player.height)) return false;
				Shatter(vector, player);
				player.AddBuff(88, 240);
				return false;
			}
			else
			{
				Item.holdStyle = 0;
				Item.useAnimation = 15;
				Item.useTime = 15;
				Item.damage = 160;
				Item.UseSound = HushStabSound;
				Item.shoot = ProjectileType<HushClosed>();
				Item.noUseGraphic = true;
				Item.useStyle = ItemUseStyleID.Shoot;
			}
			return base.CanUseItem(player);
		}

		public override void HoldItem(Player player)
        {
			if (player.itemAnimation == 0 && !player.mount.Active)
			{
				player.itemLocation.X = player.position.X + (float)player.width * 0.5f - (float)(16 * player.direction);
				player.itemLocation.Y = player.position.Y + 18f;
				player.fallStart = (int)(player.position.Y / 16f);
				if (player.gravDir == -1f)
				{
					Item.noUseGraphic = false;
					Item.useStyle = ItemUseStyleID.Thrust;
					if (Item.holdStyle != 2)
					{
						Item.holdStyle = 2;
						SoundEngine.PlaySound(HushOpenSound);
					}
					player.itemLocation.Y = player.position.Y + (float)player.height + (player.position.Y - player.itemLocation.Y);
					if (player.velocity.Y < -2f && !player.controlDown)
					{
						player.velocity.Y = -2f;
					}
				}
				else if (player.velocity.Y > 2f && !player.controlDown)
				{
					Item.noUseGraphic = false;
					Item.useStyle = ItemUseStyleID.Thrust;
					if (Item.holdStyle != 2)
					{
						Item.holdStyle = 2;
						SoundEngine.PlaySound(HushOpenSound);
					}
					player.velocity.Y = 2f;
				}
				else
				{
					Item.holdStyle = 0;
				}
			}
			base.HoldItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(0, -12);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				Projectile.NewProjectile(source, position, velocity, type, 160 + (int)(160 *  player.GetDamage(DamageClass.Melee).Multiplicative), Item.knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 30)
			.AddIngredient(ItemID.PinkPaint, 10)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

	}



}