using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Dusts;

namespace TRRA.Projectiles.Item.Weapon.SunderedRose
{
	public class WhiteRoseBullet : ModProjectile
	{

		public override void SetDefaults() {
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 3;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 50;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.light = 0.75f;
			AIType = ProjectileID.Bullet;
		}

        public override void AI()
		{
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<WhiteRosePetal>(), Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 0.7f);
            }
        }

        public override void Kill(int timeLeft)
		{
            int dustQuantity = 5;
            for (int i = 0; i < dustQuantity; i++)
            {
                Vector2 dustOffset = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 32f;
                int dust = Dust.NewDust(Projectile.position + dustOffset, Projectile.width, Projectile.height, DustType<WhiteRosePetal>());
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].scale = 1.5f;
            }
        }
	}
}
