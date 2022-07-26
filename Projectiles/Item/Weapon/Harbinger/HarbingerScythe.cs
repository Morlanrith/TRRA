using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Dusts;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.Harbinger
{
	public class HarbingerScythe : ModProjectile
	{
		private static readonly SoundStyle HarbingerSliceSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Harbinger/HarbingerSlice")
		{
			Volume = 0.4f,
			Pitch = 0.1f,
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harbinger Scythe");
		}

		public override void SetDefaults()
		{
			Projectile.width = 110;
			Projectile.height = 92;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.hide = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.ownerHitCheck = true;
		}

        public override void AI()
		{

			float num = 50f;
			float num2 = 2f;
			float num3 = 20f;
			Player player = Main.player[Projectile.owner];
			float num4 = -(float)Math.PI / 4f;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
            _ = Vector2.Zero;
            if (player.dead || player.HeldItem.type != ItemType<Items.Weapons.HarbingerSc>())
			{
				Projectile.Kill();
				return;
			}
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 24;
				SoundEngine.PlaySound(HarbingerSliceSound, Projectile.position);
			}
			if (Main.rand.NextBool(5))
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
			if (Main.rand.NextBool(15))
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CrowFeathers>(), Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
			int num11 = Math.Sign(Projectile.velocity.X);
			Projectile.velocity = new Vector2(num11, 0f);
			if (Projectile.ai[0] == 0f)
			{
				Projectile.rotation = new Vector2(num11, 0f - player.gravDir).ToRotation() + num4 + (float)Math.PI;
				if (Projectile.velocity.X < 0f)
				{
					Projectile.rotation -= (float)Math.PI / 2f;
				}
			}
			Projectile.alpha -= 128;
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			_ = Projectile.ai[0] / num;
			float num12 = 1f;
			Projectile.ai[0] += num12;
			Projectile.rotation += (float)Math.PI * 4f * num2 / num * (float)num11;
			bool flag2 = Projectile.ai[0] == (float)(int)(num / 2f);
			if (Projectile.ai[0] >= num || (flag2 && !player.controlUseItem))
			{
				Projectile.Kill();
				player.reuseDelay = 2;
			}
			else if (flag2)
			{
				Vector2 mouseWorld2 = Main.MouseWorld;
				int num13 = ((player.DirectionTo(mouseWorld2).X > 0f) ? 1 : (-1));
				if ((float)num13 != Projectile.velocity.X)
				{
					player.ChangeDir(num13);
					Projectile.velocity = new Vector2(num13, 0f);
					Projectile.netUpdate = true;
					Projectile.rotation -= (float)Math.PI;
				}
			}
			if ((Projectile.ai[0] == num12 || (Projectile.ai[0] == (float)(int)(num / 2f) && Projectile.active)) && Projectile.owner == Main.myPlayer)
			{
				Vector2 mouseWorld3 = Main.MouseWorld;
				_ = player.DirectionTo(mouseWorld3) * 0f;
			}
			float num14 = Projectile.rotation - (float)Math.PI / 4f * (float)num11;
            Vector2 vector2 = (num14 + (num11 == -1 ? (float)Math.PI : 0f)).ToRotationVector2() * (Projectile.ai[0] / num) * num3;
            Projectile.position = vector - Projectile.Size / 2f;
			Projectile.position += vector2;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.timeLeft = 2;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(2);
			player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
		}
	}
}
