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
		private static readonly SoundStyle HarbingerWooshSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Harbinger/HarbingerWoosh")
		{
			Volume = 0.4f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle HarbingerFlapSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Harbinger/HarbingerWingFlap")
		{
			Volume = 0.05f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omen Portal");
			//Main.projFrames[Projectile.type] = 4;
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
		}
	}
}
