using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.EmberCelica
{
	public class FireFistsExplosion : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
		{
			Projectile.width = 52;
			Projectile.height = 52;
			Projectile.scale = 1f;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 16;
			Projectile.alpha = 50;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 1.0f, 0.0f, 1.0f);
			if (Main.rand.NextBool(10)) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
            if (Main.rand.NextBool(10)) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.HallowedTorch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);

            Projectile.frame = (int)(5 - Math.Ceiling(Projectile.timeLeft / 3.2));
        }
	}
}
