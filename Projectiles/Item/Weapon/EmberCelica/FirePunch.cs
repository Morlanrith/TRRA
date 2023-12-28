using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.EmberCelica
{
	public class FirePunch : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.scale = 1f;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 32;
			Projectile.alpha = 100;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 1.0f, 0.0f, 1.0f);
			if (Main.rand.NextBool(10)) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
            if (Main.rand.NextBool(10)) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.HallowedTorch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);

            Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
			Projectile.position = Main.player[Projectile.owner].MountedCenter;
			if (Projectile.ai[0] == 0) // The Projectile works slightly differently depending on which fire method was used, this checks for that
			{
				float iR = Main.player[Projectile.owner].itemRotation;
				Projectile.timeLeft -= 1; // Causes the timeleft to decrease by an additional count of 1 (decreasing by 2 with each call), as the primary fire version has a usetime of 20
				if (Projectile.direction == -1)
				{
					Projectile.position.X -= 20;
					if (iR > 0.75f)
					{
						Projectile.position.Y -= 23;
						Projectile.rotation = -0.8f;
					}
					else if (iR <= 0.75f && iR > -0.6f)
					{
						Projectile.position.Y -= 9;
						Projectile.rotation = -1.6f;
					}
					else 
					{
						Projectile.position.Y -= 6;
						Projectile.rotation = -2.4f; 
					}
				}
				else
				{
					Projectile.position.X -= 7;
					if (iR <= -0.75f)
					{
						Projectile.position.Y -= 23;
						Projectile.rotation = 0.8f;
					}
					else if (iR > -0.75f && iR <= 0.6f)
					{
						Projectile.position.Y -= 9;
						Projectile.rotation = 1.6f;
					}
					else
					{
						Projectile.position.Y -= 6;
						Projectile.rotation = 2.4f;
					}
				}
			}
			else
            {
				Projectile.position.Y -= 9;
				if (Projectile.direction == -1)
				{
					Projectile.position.X -= 20;
					Projectile.rotation = -1.6f;				
				}
				else
				{
					Projectile.position.X -= 7;
					Projectile.rotation = 1.6f;
				}
			}
		}
	}
}
