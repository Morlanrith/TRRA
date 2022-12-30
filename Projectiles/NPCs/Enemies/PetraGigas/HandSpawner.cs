using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameInput;
using static Terraria.ModLoader.ModContent;
using TRRA.Dusts;

namespace TRRA.Projectiles.NPCs.Enemies.PetraGigas
{
    public class HandSpawner : ModProjectile
	{
		private bool handSpawned = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geist Hand Spawner");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 52;
			Projectile.height = 70;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 90;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 205);
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

        public override bool? CanCutTiles()
        {
			return false;
        }

		public override void AI()
		{
			if(Projectile.timeLeft <= 60 && !handSpawned)
			{
				handSpawned = true;
				Vector2 targetPosition = Main.player[(int)Projectile.ai[1]].position;
                float projXVelocity = targetPosition.X - Projectile.position.X + (float)Main.rand.Next(-20, 21);
                float projYVelocity = targetPosition.Y - Projectile.position.Y + (float)Main.rand.Next(-20, 21);
                float num20 = (float)Math.Sqrt(projXVelocity * projXVelocity + projYVelocity * projYVelocity);
                num20 = 5f / num20;
                projXVelocity *= num20;
                projYVelocity *= num20;
                Vector2 projVelocity = new Vector2(projXVelocity, projYVelocity);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, projVelocity, ProjectileType<GeistHand>(), Projectile.damage, Projectile.knockBack, 0, 1);
            }
			if (Main.rand.NextBool(5))
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
			}
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 6)
				{
					Projectile.frame = 0;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < Main.rand.Next(5, 10); i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
		}
	}
}
