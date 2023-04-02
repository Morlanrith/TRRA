using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.Harbinger
{
	public class PortentG : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 66;
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
			if (Main.player[Projectile.owner].dead || Main.player[Projectile.owner].HeldItem.type != ItemType<Items.Weapons.Portent>() || Main.player[Projectile.owner].itemAnimation < 1)
			{
				Projectile.Kill();
				return;
			}
			Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
			Projectile.position = Main.player[Projectile.owner].MountedCenter;
			float iR = Main.player[Projectile.owner].itemRotation;
			if (Projectile.direction == -1) // Facing left
			{
				if (iR > 0.75f) // Up
				{
					Projectile.position.X -= 55;
					Projectile.position.Y -= 30;
					Projectile.rotation = -0.8f;
				}
				else if (iR <= 0.75f && iR > -0.6f) // Center
				{
					Projectile.position.X -= 39;
					Projectile.position.Y -= 8;
					Projectile.rotation = -1.6f;
				}
				else // Down
				{
					Projectile.position.X -= 25;
					Projectile.position.Y -= 7;
					Projectile.rotation = -2.4f;
				}
			}
			else
			{
				if (iR <= -0.75f) // Up
				{
					Projectile.position.X -= -7;
					Projectile.position.Y -= 30;
					Projectile.rotation = 0.8f;
				}
				else if (iR > -0.75f && iR <= 0.6f) // Center
				{
					Projectile.position.X -= 10;
					Projectile.position.Y -= 8;
					Projectile.rotation = 1.6f;
				}
				else // Down
				{
					Projectile.position.X -= 24;
					Projectile.position.Y -= 7;
					Projectile.rotation = 2.4f;
				}
			}
			Projectile.spriteDirection = Projectile.direction * -1;
		}
	}
}
