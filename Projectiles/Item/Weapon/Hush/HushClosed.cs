using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.Hush
{
	public class HushClosed : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("HushClosed");
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 19;
			projectile.penetrate = -1;
			projectile.alpha = 0;
			projectile.scale = 1.0f;
			projectile.timeLeft = 15;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.friendly = true;
		}

		public float MovementFactor
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		public override void AI()
		{
			Player projOwner = Main.player[projectile.owner];
			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			projectile.direction = projOwner.direction;
			projOwner.heldProj = projectile.whoAmI;
			projOwner.itemTime = projOwner.itemAnimation;
			projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
			projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
			if (!projOwner.frozen)
			{
				if (MovementFactor == 0f)
				{
					MovementFactor = 3f;
					projectile.netUpdate = true;
				}
				if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3) MovementFactor -= 2.4f;
				else MovementFactor += 2.1f;
			}
			projectile.position += projectile.velocity * MovementFactor;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= MathHelper.ToRadians(90f);
			}
		}
	}
}
