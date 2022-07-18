using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.Myrtenaster
{
	public class SilbernelkeFR : ModProjectile
	{
		private readonly Texture2D weaponTexture = ModContent.Request<Texture2D>($"TRRA/Items/Weapons/SilbernelkeF").Value;
		private readonly Texture2D projTexture = ModContent.Request<Texture2D>($"TRRA/Projectiles/Item/Weapon/Myrtenaster/SilbernelkeFR").Value;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SilbernelkeFR");
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 4;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 120);
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.HeldItem.type != ItemType<Items.Weapons.SilbernelkeF>()) Projectile.Kill();
			float num = (float)Math.PI / 2f;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
			int num2 = 2;
			float num3 = 0f;
			num = 0f;
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 8f)
			{
				Projectile.ai[0] = 0f;
			}
			num2 = 9;
			num3 = Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.05f;
			Projectile.soundDelay--;
			if (Projectile.soundDelay <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
				Projectile.soundDelay = 6;
			}
			if (Main.myPlayer == Projectile.owner)
			{
				if (player.channel && !player.noItems && !player.CCed)
				{
					float num46 = 1f;
					if (player.inventory[player.selectedItem].shoot == Projectile.type)
					{
						num46 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
					}
					Vector2 vec3 = Main.MouseWorld - vector;
					vec3.Normalize();
					if (vec3.HasNaNs())
					{
						vec3 = Vector2.UnitX * player.direction;
					}
					vec3 *= num46;
					if (vec3.X != Projectile.velocity.X || vec3.Y != Projectile.velocity.Y)
					{
						Projectile.netUpdate = true;
					}
					Projectile.velocity = vec3;
				}
				else
				{
					Projectile.Kill();
				}
			}
			DelegateMethods.v3_1 = new Vector3(0.5f, 0.5f, 0.5f);
			Utils.PlotTileLine(Projectile.Center - Projectile.velocity, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 80f, 16f, DelegateMethods.CastLightOpen);
			Projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - Projectile.Size / 2f;
			Projectile.rotation = Projectile.velocity.ToRotation() + num;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.timeLeft = 2;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(num2);
			player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(Projectile.velocity.Y * (float)Projectile.direction, Projectile.velocity.X * (float)Projectile.direction) + num3);
			player.itemAnimation = num2 - (int)Projectile.ai[0];
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			for (int j = 1; j <= 3; j++)
			{
				Rectangle rectangle = projHitbox;
				Vector2 vector5 = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.width * j;
				rectangle.Offset((int)vector5.X, (int)vector5.Y);
				if (rectangle.Intersects(targetHitbox))
				{
					return true;
				}
			}
			if (projHitbox.Intersects(targetHitbox))
			{
				return true;
			}
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 end = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 220f * Projectile.scale;
			Utils.PlotTileLine(Projectile.Center, end, 80f * Projectile.scale, DelegateMethods.CutTiles);
			int num2 = (int)(Projectile.position.X / 16f);
			int num3 = (int)((Projectile.position.X + (float)Projectile.width) / 16f) + 1;
			int num4 = (int)(Projectile.position.Y / 16f);
			int num5 = (int)((Projectile.position.Y + (float)Projectile.height) / 16f) + 1;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num3 > Main.maxTilesX)
			{
				num3 = Main.maxTilesX;
			}
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesY)
			{
				num5 = Main.maxTilesY;
			}
			for (int j = num2; j < num3; j++)
			{
				for (int k = num4; k < num5; k++)
				{
					if (Main.tile[j, k] != null && Main.tileCut[Main.tile[j, k].TileType] && WorldGen.CanCutTile(j, k, TileCuttingContext.AttackProjectile))
					{
						WorldGen.KillTile(j, k);
						if (Main.netMode != NetmodeID.SinglePlayer)
						{
							NetMessage.SendData(17, -1, -1, null, 0, j, k);
						}
					}
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Draw_Silbernelke();
			return false;
		}

		private void Draw_Silbernelke()
		{
			int num2 = 2;
			Vector2 vector = Projectile.Center - Projectile.rotation.ToRotationVector2() * num2;
			float num3 = Main.rand.NextFloat();
			float num4 = Utils.GetLerpValue(0f, 0.3f, num3, clamped: true) * Utils.GetLerpValue(1f, 0.5f, num3, clamped: true);
			Color color = Projectile.GetAlpha(Lighting.GetColor(Projectile.Center.ToTileCoordinates())) * num4;
			Vector2 origin = weaponTexture.Size() / 2f;
			float num5 = Main.rand.NextFloatDirection();
			float num6 = 8f + MathHelper.Lerp(0f, 20f, num3) + Main.rand.NextFloat() * 6f;
			float num7 = Projectile.rotation + num5 * ((float)Math.PI * 2f) * 0.04f;
			float num8 = num7 + (float)Math.PI / 4f;
			Vector2 position = vector + num7.ToRotationVector2() * num6 + Main.rand.NextVector2Circular(8f, 8f) - Main.screenPosition;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.rotation < -(float)Math.PI / 2f || Projectile.rotation > (float)Math.PI / 2f)
			{
				num8 += (float)Math.PI / 2f;
				spriteEffects |= SpriteEffects.FlipHorizontally;
			}
			Main.spriteBatch.Draw(weaponTexture, position, null, color, num8, origin, 1f, spriteEffects, 0f);
			for (int j = 0; j < 1; j++)
			{
				float num9 = Main.rand.NextFloat();
				float num10 = Utils.GetLerpValue(0f, 0.3f, num9, clamped: true) * Utils.GetLerpValue(1f, 0.5f, num9, clamped: true);
				float amount = Utils.GetLerpValue(0f, 0.3f, num9, clamped: true) * Utils.GetLerpValue(1f, 0.5f, num9, clamped: true);
				float num11 = MathHelper.Lerp(0.6f, 1f, amount);
				Color color2 = Color.DarkOrange;
				color2 *= num10 * 0.5f;
				Vector2 origin2 = projTexture.Size() / 2f;
				Color color3 = Microsoft.Xna.Framework.Color.White * num10;
				color3.A /= 2;
				Color color4 = color3 * 0.5f;
				float num12 = 1f;
				float num13 = Main.rand.NextFloat() * 2f;
				float num14 = Main.rand.NextFloatDirection();
				Vector2 vector2 = new Vector2(2.8f + num13, 1f) * num12 * num11;
				_ = new Vector2(1.5f + num13 * 0.5f, 1f) * num12 * num11;
				int num15 = 50;
				Vector2 vector3 = Projectile.rotation.ToRotationVector2() * ((j >= 1) ? 56 : 0);
				float num16 = 0.03f - (float)j * 0.012f;
				float num17 = 30f + MathHelper.Lerp(0f, num15, num9) + num13 * 16f;
				float num18 = Projectile.rotation + num14 * ((float)Math.PI * 2f) * num16;
				float rotation = num18;
				Vector2 position2 = vector + num18.ToRotationVector2() * num17 + Main.rand.NextVector2Circular(20f, 20f) + vector3 - Main.screenPosition;
				color2 *= num12;
				color4 *= num12;
				SpriteEffects effects = SpriteEffects.None;
				Main.spriteBatch.Draw(projTexture, position2, null, color2, rotation, origin2, vector2, effects, 0f);
				Main.spriteBatch.Draw(projTexture, position2, null, color4, rotation, origin2, vector2 * 0.6f, effects, 0f);
			}
		}
	}
}
