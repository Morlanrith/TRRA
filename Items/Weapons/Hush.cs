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

namespace TRRA.Items.Weapons
{
    public class Hush : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Hush");
			Tooltip.SetDefault("'I scream, you scream...'\nRight Click to teleport to the position of the mouse");
		}

		public override void SetDefaults() 
		{
			item.width = 51;
			item.height = 51;
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(gold: 25);
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 15;
			item.useTime = 15;
			item.damage = 160;
			item.crit = 62;
			item.knockBack = 5f;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/Hush/HushStab");
			item.melee = true;
			item.shoot = mod.ProjectileType("HushClosed");
			item.noUseGraphic = true;
			item.shootSpeed = 5f;
			item.noMelee = true;
			item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
            return true;
		}

		private void ShatterEffect(Rectangle effectRect, float teleportTime)
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

		private void Shatter(Vector2 newPos, Player player)
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
			}
			catch
			{
			}
		}

		public override bool CanUseItem(Player player)
		{
			// If the player uses the alt function (Right Click), causes the player to teleport
			if (player.altFunctionUse == 2)
			{
				if (player.chaosState || !PlayerInput.Triggers.JustPressed.MouseRight) return false;
				item.useAnimation = 22;
				item.useTime = 22;
				item.damage = 0;
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/Hush/HushShatter");
				item.shoot = ProjectileID.None;
				item.autoReuse = false;
				item.noUseGraphic = false;
				item.useStyle = ItemUseStyleID.HoldingOut;
				Vector2 vector = default;
				vector.X = (float)Main.mouseX + Main.screenPosition.X;
				if (player.gravDir == 1f) vector.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;		
				else vector.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
				vector.X -= player.width / 2;
				if (!(vector.X > 50f) || !(vector.X < (float)(Main.maxTilesX * 16 - 50)) || !(vector.Y > 50f) || !(vector.Y < (float)(Main.maxTilesY * 16 - 50))) return false;
				int num = (int)(vector.X / 16f);
				int num2 = (int)(vector.Y / 16f);
				if ((Main.tile[num, num2].wall == 87 && (double)num2 > Main.worldSurface && !NPC.downedPlantBoss) || Collision.SolidCollision(vector, player.width, player.height)) return false;
				Shatter(vector, player);
				player.AddBuff(88, 240);
			}
			else
			{
				item.useAnimation = 15;
				item.useTime = 15;
				item.damage = 160;
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/Hush/HushStab");
				item.shoot = mod.ProjectileType("HushClosed");
				item.autoReuse = true;
				item.noUseGraphic = true;
				item.useStyle = ItemUseStyleID.HoldingOut;
			}
			return base.CanUseItem(player);
		}

		public override void HoldItem(Player player)
        {
			if (player.itemAnimation == 0 && !player.mount.Active)
			{
				player.itemRotation = 0f;
				player.itemLocation.X = player.position.X + (float)player.width * 0.5f - (float)(16 * player.direction);
				player.itemLocation.Y = player.position.Y + 22f;
				player.fallStart = (int)(player.position.Y / 16f);
				if (player.gravDir == -1f)
				{
					item.noUseGraphic = false;
					item.useStyle = ItemUseStyleID.Stabbing;
					if (item.holdStyle != 2)
					{
						item.holdStyle = 2;
						Main.PlaySound(SoundID.Item, -1, -1, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/Weapon/Hush/HushOpen"));
					}

					player.itemRotation = 0f - player.itemRotation;
					player.itemLocation.Y = player.position.Y + (float)player.height + (player.position.Y - player.itemLocation.Y);
					if (player.velocity.Y < -2f && !player.controlDown)
					{
						player.velocity.Y = -2f;
					}
				}
				else if (player.velocity.Y > 2f && !player.controlDown)
				{
					item.noUseGraphic = false;
					item.useStyle = ItemUseStyleID.Stabbing;
					if (item.holdStyle != 2)
					{
						item.holdStyle = 2;
						Main.PlaySound(SoundID.Item, -1, -1, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/Weapon/Hush/HushOpen"));
					}
					player.velocity.Y = 2f;
				}
				else
				{
					item.holdStyle = 0;
					item.noUseGraphic = true;
				}
			}
			else if (player.altFunctionUse == 2)
            {
				item.holdStyle = 0;
				item.noUseGraphic = false;
				player.itemLocation.Y = player.position.Y - 32f;
				if (player.direction == 1)
				{
					player.itemRotation = 0.8f;
					player.itemLocation.X -= 4f;
				}
                else
                {
					player.itemRotation = -0.8f;
					player.itemLocation.X += 4f;
				}
				
			}
			else
			{
				item.holdStyle = 0;
				item.noUseGraphic = true;
			}
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, 160 + (int)(160 * player.meleeDamageMult), knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<DustExtract>(), 1);
			recipe.AddIngredient(ItemType<DustWeaponKit>(), 1);
			recipe.AddIngredient(ItemType<GravityDustCrystal>(), 10);
			recipe.AddIngredient(ItemType<IceDustCrystal>(), 30);
			recipe.AddIngredient(ItemID.PinkPaint, 10);
			recipe.AddTile(TileType<DustToolbenchTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}



}