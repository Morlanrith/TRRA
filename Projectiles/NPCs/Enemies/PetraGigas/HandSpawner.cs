using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace TRRA.Projectiles.NPCs.Enemies.PetraGigas
{
    public class HandSpawner : ModProjectile
	{
		private bool handSpawned = false;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 78;
			Projectile.height = 78;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 90;
        }

		public override Color? GetAlpha(Color lightColor)
		{
			int timeAlpha = (int)(Projectile.timeLeft * (255.0f/90));
			return new Color(timeAlpha, timeAlpha, timeAlpha, timeAlpha);
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

        public override bool? CanCutTiles()
        {
			return false;
        }

        public override void OnSpawn(IEntitySource source)
		{
            Projectile.rotation += (float)(Math.PI / 180) * Main.rand.Next(0,361);
        }

        public override void AI()
		{
            Projectile.rotation += Projectile.timeLeft * (0.012f / 90) * Projectile.ai[1];
            Projectile.scale = 0.5f + ((float)Projectile.timeLeft / 180);
            if (Projectile.timeLeft <= 60 && !handSpawned)
			{
				handSpawned = true;
				Vector2 targetPosition = Main.player[(int)Projectile.ai[0]].position;
                float projXVelocity = targetPosition.X - Projectile.position.X + (float)Main.rand.Next(-20, 21);
                float projYVelocity = targetPosition.Y - Projectile.position.Y + (float)Main.rand.Next(-20, 21);
                float num20 = (float)Math.Sqrt(projXVelocity * projXVelocity + projYVelocity * projYVelocity);
                num20 = 5f / num20;
                projXVelocity *= num20;
                projYVelocity *= num20;
                Vector2 projVelocity = new Vector2(projXVelocity, projYVelocity);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projVelocity, ProjectileType<GeistHand>(), Projectile.damage, Projectile.knockBack, 0, 1);
            }
			if (Main.rand.NextBool(5))
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < Main.rand.Next(5, 10); i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
		}
	}
}
