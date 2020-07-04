using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.EmberCelica
{
    public class TripEmberRocket : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Triple Ember Rocket");
		}

		public override void SetDefaults() {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 30;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            aiType = ProjectileID.Bullet;
        }

		public override void AI()
		{
            int fire = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X * 4f + 2f, projectile.position.Y + 2f - projectile.velocity.Y * 4f), 10, 10, DustID.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default, 1.25f);
            Main.dust[fire].velocity *= -0.25f;
            fire = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X * 4f + 2f, projectile.position.Y + 2f - projectile.velocity.Y * 4f), 10, 10, DustID.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default, 1.25f);
            Main.dust[fire].velocity *= -0.25f;
            Main.dust[fire].position -= projectile.velocity * 0.5f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
                Main.PlaySound(SoundID.Item14, projectile.position);
                int dustQuantity = 10;
                for (int i = 0; i < dustQuantity; i++)
                {
                    Vector2 dustOffset = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 32f;
                    int dust = Dust.NewDust(projectile.position + dustOffset, projectile.width, projectile.height, DustID.Fire);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].scale = 1.5f;
                }
                Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), ProjectileID.SolarWhipSwordExplosion, projectile.damage, projectile.knockBack, projectile.owner, 0, 1);
            }
            else
            {
                float numberProjectiles = 3;
                float rotation = MathHelper.ToRadians(15);
                Vector2 position = projectile.position + Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 15f;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<ChildEmberRocket>(), projectile.damage, projectile.knockBack, 0, 1);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
