using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TRRA.Projectiles
{
	internal class GambolRibbonEnd : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambol Ribbon End");
		}

        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height = 100;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 27, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 150, default, 0.7f);
            }
            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 12;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/RibbonSwing"), projectile.position);
            }
            Vector2 playerCenter = Main.player[projectile.owner].MountedCenter; 
            Vector2 distToProj = playerCenter - Main.MouseWorld;
            float distance = distToProj.Length();
            if (Main.player[projectile.owner].channel)
            {
                projectile.spriteDirection = Main.player[projectile.owner].direction;
                projectile.rotation += 0.6f * Main.player[projectile.owner].direction;
                Vector2 newPosition = Main.MouseWorld;
                if (distance < 150f)
                {
                    newPosition.X -= 50;
                    newPosition.Y -= 50;
                    projectile.position = newPosition;
                }
                else
                {
                    float newX = distToProj.X * (150f / distance);
                    float newY = distToProj.Y * (150f / distance);
                    projectile.position = new Vector2(playerCenter.X-newX-50,playerCenter.Y-newY-50);
                }
                float projRotation;
                Main.player[projectile.owner].itemTime = 10;
                Main.player[projectile.owner].itemAnimation = 10;
                if (playerCenter.X - Main.MouseWorld.X > 0)
                {
                    projRotation = distToProj.ToRotation();
                    Main.player[projectile.owner].direction = -1;
                    Main.player[projectile.owner].itemRotation = projRotation;
                }
                else
                {
                    projRotation = (Main.MouseWorld - playerCenter).ToRotation();
                    Main.player[projectile.owner].direction = 1;
                    Main.player[projectile.owner].itemRotation = projRotation;
                }
                
            }
            else projectile.Kill();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
            Vector2 center = projectile.Center;
            Vector2 distToProj = playerCenter - projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= 24f;
                center += distToProj;
                distToProj = playerCenter - center;
                distance = distToProj.Length();
                Color drawColor = lightColor;
                //Draw ribbon
                spriteBatch.Draw(mod.GetTexture("Projectiles/GambolRibbon"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y), new Rectangle(0, 0, 2, Main.chain30Texture.Height*2), drawColor, projRotation, new Vector2(2 * 0.5f, Main.chain30Texture.Height * 1f), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
