using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.Myrtenaster
{
	public class MyrtenasterFS : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.damage = 135;
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 150;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 50);
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 1.0f, 0.42f, 0.0f);

			int fire = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 80, 80, DustID.Torch, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.25f);
			Main.dust[fire].velocity *= -0.25f;
			fire = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 80, 80, DustID.Torch, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.25f);
			Main.dust[fire].velocity *= -0.25f;
			Main.dust[fire].position -= Projectile.velocity * 0.5f;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
		}

		public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			int dustQuantity = 10;
			for (int i = 0; i < dustQuantity; i++)
			{
				Vector2 dustOffset = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 32f;
				int dust = Dust.NewDust(Projectile.position + dustOffset, Projectile.width, Projectile.height, DustID.Torch);
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].scale = 1.5f;
			}
			Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center, new Vector2(0, 0), ProjectileID.SolarWhipSwordExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 1);

		}

        public override bool PreDraw(ref Color lightColor)
        {
			//Redraw the Projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

	}
}
