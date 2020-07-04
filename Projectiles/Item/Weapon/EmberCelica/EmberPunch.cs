using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.EmberCelica
{
	public class EmberPunch : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ember Punch");
		}

		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.scale = 1f;
			projectile.aiStyle = 0;
			projectile.timeLeft = 40;
			projectile.alpha = 100;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 1.0f, 0.42f, 0.0f);
			if (Main.rand.Next(5) == 0) Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 150, default, 0.7f);

			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			projectile.position = Main.player[projectile.owner].MountedCenter;
			if (projectile.ai[0] == 0) // The projectile works slightly differently depending on which fire method was used, this checks for that
			{
				float iR = Main.player[projectile.owner].itemRotation;
				projectile.timeLeft -= 1; // Causes the timeleft to decrease by an additional count of 1 (decreasing by 2 with each call), as the primary fire version has a usetime of 20
				if (projectile.direction == -1)
				{
					projectile.position.X -= 20;
					if (iR > 0.75f)
					{
						projectile.position.Y -= 23;
						projectile.rotation = -0.8f;
					}
					else if (iR <= 0.75f && iR > -0.6f)
					{
						projectile.position.Y -= 9;
						projectile.rotation = -1.6f;
					}
					else 
					{
						projectile.position.Y -= 6;
						projectile.rotation = -2.4f; 
					}
				}
				else
				{
					projectile.position.X -= 7;
					if (iR <= -0.75f)
					{
						projectile.position.Y -= 23;
						projectile.rotation = 0.8f;
					}
					else if (iR > -0.75f && iR <= 0.6f)
					{
						projectile.position.Y -= 9;
						projectile.rotation = 1.6f;
					}
					else
					{
						projectile.position.Y -= 6;
						projectile.rotation = 2.4f;
					}
				}
			}
			else
            {
				projectile.position.Y -= 9;
				if (projectile.direction == -1)
				{
					projectile.position.X -= 20;
					projectile.rotation = -1.6f;				
				}
				else
				{
					projectile.position.X -= 7;
					projectile.rotation = 1.6f;
				}
			}
		}
	}
}
