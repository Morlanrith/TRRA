using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Projectiles.Item.Weapon.GambolShroud
{
    internal class GambolNChainEnd : ModProjectile
	{
        private static Asset<Texture2D> chainTexture;

        private static readonly SoundStyle ChainSwingSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/GambolShroud/ChainSwing")
        {
            Volume = 0.6f,
            Pitch = -0.1f,
        };

        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 140;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
        }

        public override void Load()
        {
            chainTexture = Request<Texture2D>("TRRA/Projectiles/Item/Weapon/GambolShroud/GambolNChain");
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].HeldItem.type != ItemType<Items.Weapons.GambolShroudNG>()) Projectile.Kill();
            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 18;
                SoundEngine.PlaySound(ChainSwingSound, Projectile.position);
                SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
            }
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter; 
            Vector2 distToProj = playerCenter - Main.MouseWorld;
            float distance = distToProj.Length();
            if (Main.player[Projectile.owner].channel)
            {
                Projectile.spriteDirection = Main.player[Projectile.owner].direction;
                Projectile.rotation += 0.4f * Main.player[Projectile.owner].direction;
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
                    chainTexture.Value, 
                    center - Main.screenPosition, 
                    new Rectangle(0, 0, 5, Terraria.GameContent.TextureAssets.Chain30.Value.Height*2), drawColor, projRotation, 
                    new Vector2(2 * 0.5f, Terraria.GameContent.TextureAssets.Chain30.Value.Height * 1f), 
                    1f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
