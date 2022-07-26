using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.Harbinger
{
	public class HarbingerG : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Harbinger Gun");
		}

		public override void SetDefaults()
		{
			Projectile.width = 62;
			Projectile.height = 60;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 30;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
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
			Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
			Projectile.position = Main.player[Projectile.owner].MountedCenter;
			float iR = Main.player[Projectile.owner].itemRotation;
			if (Projectile.direction == -1) // Facing left
			{
				if (iR > 0.75f) // Up
				{
					Projectile.position.X -= 10;
					Projectile.position.Y -= 33;
					Projectile.rotation = -0.8f;
				}
				else if (iR <= 0.75f && iR > -0.6f) // Center
				{
					Projectile.position.X -= 16;
					Projectile.position.Y -= 44;
					Projectile.rotation = -1.6f;
				}
				else // Down
				{
					Projectile.position.X -= 35;
					Projectile.position.Y -= 54;
					Projectile.rotation = -2.4f;
				}
			}
			else
			{
				if (iR <= -0.75f) // Up
				{
					Projectile.position.X -= 52;
					Projectile.position.Y -= 33;
					Projectile.rotation = 0.8f;
				}
				else if (iR > -0.75f && iR <= 0.6f) // Center
				{
					Projectile.position.X -= 46;
					Projectile.position.Y -= 44;
					Projectile.rotation = 1.6f;
				}
				else // Down
				{
					Projectile.position.X -= 25;
					Projectile.position.Y -= 54;
					Projectile.rotation = 2.4f;
				}
			}
			Projectile.spriteDirection = Projectile.direction * -1;
		}
	}
}
