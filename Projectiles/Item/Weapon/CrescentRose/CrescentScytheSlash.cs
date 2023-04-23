using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TRRA.Dusts;
using TRRA.Items.Weapons;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.CrescentRose
{
	public class CrescentScytheSlash : ModProjectile
	{
        private bool completeRose = false;

		public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults() {
            Projectile.width = 170;
            Projectile.height = 170;
            Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 5;
            Projectile.timeLeft = 90;
			Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.spriteDirection = owner.direction;
            completeRose = owner.HeldItem.type == ItemType<CrescentRoseS>();
            if(!completeRose)
            {
                Projectile.penetrate = 2;
                Projectile.timeLeft = 45;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (!completeRose)
            {
                return lightColor.MultiplyRGB(new(0.75f, 0.75f, 0.75f));
            }
            return base.GetAlpha(lightColor);
        }

        public override void AI()
		{
            Projectile.alpha = 200 - (int)(Projectile.timeLeft * (100.0f / (completeRose ? 45 : 22.5f)));
            Projectile.frame = (int)Math.Floor((Projectile.alpha) / 50.0f);

            float num = 50f;
            float num2 = 15f;
            float num3 = Projectile.ai[1] + num;
            float num4 = num3 + num2;
            float num5 = 77f;
            Projectile.localAI[0] += 1f;
            if (Projectile.damage == 0 && Projectile.localAI[0] < MathHelper.Lerp(num3, num4, 0.5f))
            {
                Projectile.localAI[0] += 6f;
            }
            if (Projectile.localAI[0] >= num4)
            {
                Projectile.localAI[1] = 1f;
                Projectile.Kill();
                return;
            }
            Projectile.direction = Projectile.spriteDirection;
            int num7 = 3;
            if (Projectile.damage != 0 && Projectile.localAI[0] >= num5 + (float)num7)
            {
                Projectile.damage = 0;
            }
            if (Projectile.damage != 0)
            {
                int num8 = 80;
                bool flag = false;
                float num9 = Projectile.velocity.ToRotation();
                for (float num10 = -1f; num10 <= 1f; num10 += 0.5f)
                {
                    Vector2 position = Projectile.Center + (num9 + num10 * ((float)Math.PI / 4f) * 0.25f).ToRotationVector2() * num8 * 0.5f * Projectile.scale;
                    Vector2 position2 = Projectile.Center + (num9 + num10 * ((float)Math.PI / 4f) * 0.25f).ToRotationVector2() * num8 * Projectile.scale;
                    if (!Collision.SolidTiles(Projectile.Center, 0, 0) && Collision.CanHit(position, 0, 0, position2, 0, 0))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    Projectile.damage = 0;
                }
            }
            Projectile.localAI[1] += 1f;
            Projectile.rotation += Projectile.spriteDirection * ((float)Math.PI * 2f) * (4f + Projectile.Opacity * 4f) / 90f;
            float f = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
            Vector2 position3 = Projectile.Center + f.ToRotationVector2() * 84f * Projectile.scale;

            if (!completeRose) return;

            // Spawn dust and lighting
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustPerfect(position3, DustType<RosePetal>(), null, 150, default, 1.4f);
                dust.noLight = (dust.noLightEmittence = true);
            }
            Lighting.AddLight(Projectile.Center, 0.4f, 0f, 0f);
        }
    }
}
