using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameInput;
using static Terraria.ModLoader.ModContent;
using TRRA.Dusts;

namespace TRRA.Projectiles.Item.Weapon.Omen
{
	public class PortalBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kindred Link");
			Description.SetDefault("Can warp back to your placed portal");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ProjectileType<OmenPortal>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
	public class OmenPortal : ModProjectile
	{
		private static readonly SoundStyle PortalActiveSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Omen/OmenPortalActive")
		{
			Volume = 0.1f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omen Portal");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 52;
			Projectile.height = 70;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 205);
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
			if (Main.myPlayer == Projectile.owner)
			{
				Player player = Main.player[Projectile.owner];
				if (!player.HasBuff<PortalBuff>())
					Projectile.Kill();
			}
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 60;
				SoundEngine.PlaySound(PortalActiveSound, Projectile.position);
			}
			if (Main.rand.NextBool(5))
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
			}
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 6)
				{
					Projectile.frame = 0;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < Main.rand.Next(5, 10); i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
		}
	}
}
