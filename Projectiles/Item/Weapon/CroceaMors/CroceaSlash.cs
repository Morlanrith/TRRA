using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Dusts;
using Microsoft.Xna.Framework.Audio;
using static Humanizer.In;
using System;
using Terraria.Audio;

namespace TRRA.Projectiles.Item.Weapon.CroceaMors
{
	public class CroceaSlash : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults() {
            Projectile.width = 170;
            Projectile.height = 170;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
		}

        public override void AI()
		{
            Projectile.localAI[0] += 1f;
            Player player = Main.player[Projectile.owner];
            float num = Projectile.localAI[0] / Projectile.ai[1];
            float num2 = Projectile.ai[0];
            float num3 = Projectile.velocity.ToRotation();
            float num4 = (Projectile.rotation = (float)Math.PI * num2 * num + num3 + num2 * (float)Math.PI + player.fullRotation);
            float num5 = 0.6f;
            float num6 = 1f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = num6 + num * num5;

            float num8 = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
            Vector2 position2 = Projectile.Center + num8.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 value = (num8 + Projectile.ai[0] * ((float)Math.PI / 2f)).ToRotationVector2();
            if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
            {
                Dust dust2 = Dust.NewDustPerfect(Projectile.Center + num8.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), 278, value * 1f, 100, Color.Lerp(Color.Gold, Color.White, Main.rand.NextFloat() * 0.3f), 0.4f);
                dust2.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                dust2.noGravity = true;
            }
            if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
            {
                Dust.NewDustPerfect(position2, 43, value * 1f, 100, Color.White * Projectile.Opacity, 1.2f * Projectile.Opacity);
            }
            Projectile.scale *= Projectile.ai[2];
            if (Projectile.localAI[0] >= Projectile.ai[1])
            {
                //Projectile.Kill();
            }
        }

	}
}
