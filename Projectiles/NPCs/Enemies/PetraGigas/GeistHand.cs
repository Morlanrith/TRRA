using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Dusts;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.NPCs.Enemies.PetraGigas
{
	public class GeistHand : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.damage = 135;
			Projectile.width = 54;
			Projectile.height = 54;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 90;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor)
		{
            int timeAlpha = (int)(Projectile.timeLeft * (100.0f / 45));
            return new Color(timeAlpha, timeAlpha, timeAlpha, timeAlpha);
		}

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.spriteDirection = Main.rand.NextBool() ? 1 : -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			//Redraw the Projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

	}
}
