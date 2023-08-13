using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.FloatingArray
{
	public class ArrayLaser : ModProjectile
	{
		private static Asset<Texture2D> laserTexture;
		private static Asset<Texture2D> laserEndTexture;
		private Vector2 endPoint = new();

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
		}

        public override Color? GetAlpha(Color lightColor)
        {
			return Color.White;
        }

        public override void Load()
		{
			laserTexture = ModContent.Request<Texture2D>("TRRA/Projectiles/Item/Weapon/FloatingArray/ArrayLaserBeam");
			laserEndTexture = ModContent.Request<Texture2D>("TRRA/Projectiles/Item/Weapon/FloatingArray/ArrayLaserEnd");
		}

		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, 0f, 0.9f, 0.46f);
			Laser_AI();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			float collisionPoint10 = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], 22f * Projectile.scale, ref collisionPoint10))
			{
				return true;
			}
			return false;
		}


        // Type = 632
        private void Laser_AI()
        {
			Vector2? vector63 = null;
			if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
			{
				Projectile.velocity = -Vector2.UnitY;
			}
			if (!Main.projectile[(int)Projectile.ai[1]].active)
			{
				Projectile.Kill();
				return;
			}
			Projectile owner = Main.projectile[(int)Projectile.ai[1]];
			Vector2 distToOwner = owner.Center - owner.Top.RotatedBy(owner.rotation, owner.Center);
			float distance = distToOwner.Length();
			float newX = distToOwner.X * ((20f*Projectile.scale) / distance);
			float newY = distToOwner.Y * ((20f*Projectile.scale) / distance);
			Projectile.Center = new Vector2(owner.Center.X - newX, owner.Center.Y - newY);


			float num699 = (float)(int)Projectile.ai[0] - 2.5f;
			Vector2 vector68 = Vector2.Normalize(Main.projectile[(int)Projectile.ai[1]].velocity);
			Projectile projectile = Main.projectile[(int)Projectile.ai[1]];
			float num700 = num699 * ((float)Math.PI / 6f);
			float num701 = 20f;
			Vector2 zero = Vector2.Zero;
			float num702 = 1f;
			float num703 = 15f;
			float num704 = -2f;
			if (projectile.ai[0] < 180f)
			{
				num702 = 1f - projectile.ai[0] / 180f;
				num703 = 20f - projectile.ai[0] / 180f * 14f;
				if (projectile.ai[0] < 120f)
				{
					num701 = 20f - 4f * (projectile.ai[0] / 120f);
				}
				else
				{
					num701 = 16f - 10f * ((projectile.ai[0] - 120f) / 60f);
				}
				num704 = -22f + projectile.ai[0] / 180f * 20f;
			}
			else
			{
				num702 = 0f;
				num701 = 1.75f;
				num703 = 6f;
				num704 = -2f;
			}
			float num705 = (projectile.ai[0] + num699 * num701) / (num701 * 6f) * ((float)Math.PI * 2f);
			num700 = Vector2.UnitY.RotatedBy(num705).Y * ((float)Math.PI / 6f) * num702;
			zero = (Vector2.UnitY.RotatedBy(num705) * new Vector2(4f, num703)).RotatedBy(projectile.velocity.ToRotation());
			





			Projectile.velocity = Main.projectile[(int)Projectile.ai[1]].velocity;
			Projectile.scale = 1f * (1f - num702);
			Projectile.damage = projectile.damage;
			if (projectile.ai[0] >= 180f)
			{
				Projectile.damage *= 3;
				vector63 = projectile.Center;
			}
			if (!Collision.CanHitLine(Main.player[Projectile.owner].Center, 0, 0, projectile.Center, 0, 0))
			{
				vector63 = Main.player[Projectile.owner].Center;
			}
			Projectile.friendly = projectile.ai[0] > 30f;
			if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
			{
				Projectile.velocity = -Vector2.UnitY;
			}
			float num709 = Projectile.velocity.ToRotation();
			Projectile.rotation = Main.projectile[(int)Projectile.ai[1]].rotation;
			Projectile.velocity = num709.ToRotationVector2();
			float num710 = 0f;
			float num711 = 0f;
			Vector2 samplingPoint = Projectile.Center;
			if (vector63.HasValue)
			{
				samplingPoint = vector63.Value;
			}
			num710 = 2f;
			num711 = 0f;
			float[] array5 = new float[(int)num710];
			Collision.LaserScan(samplingPoint, Projectile.velocity, num711 * Projectile.scale, 2400f, array5);
			float num712 = 0f;
			for (int num713 = 0; num713 < array5.Length; num713++)
			{
				num712 += array5[num713];
			}
			num712 /= num710;
			float amount = 0.75f;
			Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], num712, amount);


			// Endpoint?
			Vector2 vector81 = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 14.5f * Projectile.scale);
			endPoint = vector81;

			if (!(Math.Abs(Projectile.localAI[1] - num712) < 100f) || !(Projectile.scale > 0.15f))
			{
				return;
			}


			Color color = new Color(0f, 0.9f, 0.46f);


			// Sparkles dust
			float x3 = Main.rgbToHsl(color).X;
			for (int num734 = 0; num734 < 2; num734++)
			{
				float num735 = Projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
				float num736 = (float)Main.rand.NextDouble() * 0.8f + 1f;
				Vector2 vector82 = new Vector2((float)Math.Cos(num735) * num736, (float)Math.Sin(num735) * num736);
				int num737 = Dust.NewDust(vector81, 0, 0, 267, vector82.X, vector82.Y);
				Main.dust[num737].color = color;
				Main.dust[num737].scale = 1.2f;
				if (Projectile.scale > 1f)
				{
					Dust dust2 = Main.dust[num737];
					dust2.color = color;
					dust2.velocity *= Projectile.scale;
					dust2 = Main.dust[num737];
					dust2.scale *= Projectile.scale;
				}
				Main.dust[num737].noGravity = true;
				if (Projectile.scale != 1f && num737 != 6000)
				{
					Dust dust17 = Dust.CloneDust(num737);
					dust17.color = color;
					Dust dust2 = dust17;
					dust2.scale /= 2f;
				}
			}


			// Smoke dust
			if (Main.rand.NextBool(5))
			{
				Vector2 vector83 = Projectile.velocity.RotatedBy(1.5707963705062866) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
				int num738 = Dust.NewDust(vector81 + vector83 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
				Dust dust2 = Main.dust[num738];
				dust2.velocity *= 0.5f;
				Main.dust[num738].velocity.Y = 0f - Math.Abs(Main.dust[num738].velocity.Y);
			}


			DelegateMethods.v3_1 = color.ToVector3() * 0.3f;
			float value7 = 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
			Vector2 size = new Vector2(Projectile.velocity.Length() * Projectile.localAI[1], (float)Projectile.width * Projectile.scale);
			float num739 = Projectile.velocity.ToRotation();
			if (Main.netMode != NetmodeID.Server)
			{
				((WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader()).QueueRipple(Projectile.position + new Vector2(size.X * 0.5f, 0f).RotatedBy(num739), new Color(0.5f, 0.1f * (float)Math.Sign(value7) + 0.5f, 0f, 1f) * Math.Abs(value7), size, RippleShape.Square, num739);
			}
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], (float)Projectile.width * Projectile.scale, DelegateMethods.CastLight);
		
		}

		// Draw Beam
		public override bool PreDrawExtras()
		{
			Vector2 playerCenter = Projectile.Top.RotatedBy(Projectile.rotation,Projectile.Center);
			Vector2 center = endPoint;
			Vector2 distToProj = playerCenter - center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			distToProj.Normalize();
			distToProj *= 11f;
			center += distToProj;
			distToProj = playerCenter - center;
			distance = distToProj.Length();
			Main.EntitySpriteDraw(laserEndTexture.Value, center - Main.screenPosition,
				new Rectangle(0, 0, laserEndTexture.Value.Width, (int)(laserEndTexture.Value.Height / Projectile.scale)), Color.White, projRotation,
				laserEndTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			while (distance > 17f && !float.IsNaN(distance))
			{
				distToProj.Normalize();
				distToProj *= 17f;
				center += distToProj;
				distToProj = playerCenter - center;
				distance = distToProj.Length();
				//Draw laser
				Main.EntitySpriteDraw(laserTexture.Value, center - Main.screenPosition,
					new Rectangle(0, 0, laserTexture.Value.Width, (int)(laserTexture.Value.Height / Projectile.scale)), Color.White, projRotation,
					laserTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
	}
}