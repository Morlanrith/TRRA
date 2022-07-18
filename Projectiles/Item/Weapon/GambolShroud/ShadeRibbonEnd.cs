using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.GambolShroud
{
    internal class ShadeRibbonEnd : ModProjectile
	{
        private static Asset<Texture2D> ribbonTexture;

        private static readonly SoundStyle RibbonSwingSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/GambolShroud/RibbonSwing")
        {
            Volume = 0.3f,
            Pitch = 0.0f,
        };

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shade Ribbon End");
		}

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
        }

        public override void Load()
        {
            ribbonTexture = ModContent.Request<Texture2D>("TRRA/Projectiles/Item/Weapon/GambolShroud/GambolRibbon");
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].HeldItem.type != ItemType<Items.Weapons.GambolShadeG>()) Projectile.Kill();
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 12;
                SoundEngine.PlaySound(RibbonSwingSound, Projectile.position);
            }
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter; 
            Vector2 distToProj = playerCenter - Main.MouseWorld;
            float distance = distToProj.Length();
            if (Main.player[Projectile.owner].channel)
            {
                Projectile.spriteDirection = Main.player[Projectile.owner].direction;
                Projectile.rotation += 0.6f * Main.player[Projectile.owner].direction;
                Vector2 newPosition = Main.MouseWorld;
                if (distance < 150f)
                {
                    newPosition.X -= 50;
                    newPosition.Y -= 50;
                    Projectile.position = newPosition;
                }
                else
                {
                    float newX = distToProj.X * (150f / distance);
                    float newY = distToProj.Y * (150f / distance);
                    Projectile.position = new Vector2(playerCenter.X-newX-50,playerCenter.Y-newY-50);
                }
                float projRotation;
                Main.player[Projectile.owner].itemTime = 10;
                Main.player[Projectile.owner].itemAnimation = 10;
                if (playerCenter.X - Main.MouseWorld.X > 0)
                {
                    projRotation = distToProj.ToRotation();
                    Main.player[Projectile.owner].direction = -1;
                    Main.player[Projectile.owner].itemRotation = projRotation;
                }
                else
                {
                    projRotation = (Main.MouseWorld - playerCenter).ToRotation();
                    Main.player[Projectile.owner].direction = 1;
                    Main.player[Projectile.owner].itemRotation = projRotation;
                }
                
            }
            else Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = playerCenter - Projectile.Center;
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
                Main.EntitySpriteDraw(
                    ribbonTexture.Value, 
                    center - Main.screenPosition, 
                    new Rectangle(0, 0, 2, Terraria.GameContent.TextureAssets.Chain30.Value.Height*2), drawColor, projRotation, 
                    new Vector2(2 * 0.5f, Terraria.GameContent.TextureAssets.Chain30.Value.Height * 1f), 
                    1f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
