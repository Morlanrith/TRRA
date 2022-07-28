using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameInput;
using static Terraria.ModLoader.ModContent;
using TRRA.Dusts;

namespace TRRA.Projectiles.Item.Weapon.Harbinger
{
	public class CorvidBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corvid Form");
			Description.SetDefault("Gifted with the ability to \"see\" more");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ProjectileType<HarbingerCorvid>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
				player.invis = true;
			}
			else
			{
				player.DelBuff(buffIndex);
				player.invis = false;
				buffIndex--;
			}
		}
	}
	// Code adapted from the vanilla's magic missile.
	public class HarbingerCorvid : ModProjectile
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
			DisplayName.SetDefault("Harbinger Corvid");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
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
			// This part makes the Projectile do a shime sound every 10 ticks as long as it is moving.
			if (Projectile.soundDelay == 0 && Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) > 2f)
			{
				Projectile.soundDelay = 20;
				SoundEngine.PlaySound(HarbingerFlapSound, Projectile.position);
			}

			// In Multi Player (MP) This code only runs on the client of the Projectile's owner, this is because it relies on mouse position, which isn't the same across all clients.
			if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
			{

				Player player = Main.player[Projectile.owner];
				// If the player channels the weapon, do something. This check only works if item.channel is true for the weapon.
				if (!PlayerInput.Triggers.JustReleased.MouseRight && !(player.CCed || player.dead || player.mount.Active || player.grappling[0] > -1 || PlayerInput.Triggers.JustPressed.Grapple) && player.HeldItem.type == ItemType<Items.Weapons.HarbingerSc>())
				{
					float maxDistance = 15f; // This also sets the maximun speed the Projectile can reach while following the cursor.
					Vector2 vectorToCursor = Main.MouseWorld - Projectile.Center;
					float distanceToCursor = vectorToCursor.Length();

					// Here we can see that the speed of the Projectile depends on the distance to the cursor.
					if (distanceToCursor > maxDistance)
					{
						distanceToCursor = maxDistance / distanceToCursor;
						vectorToCursor *= distanceToCursor;
					}

					int velocityXBy1000 = (int)(vectorToCursor.X * 1000f);
					int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
					int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
					int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);

					// This code checks if the precious velocity of the Projectile is different enough from its new velocity, and if it is, syncs it with the server and the other clients in MP.
					// We previously multiplied the speed by 1000, then casted it to int, this is to reduce its precision and prevent the speed from being synced too much.
					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
					{
						Projectile.netUpdate = true;
					}

					Projectile.velocity = vectorToCursor;
					player.position = Projectile.position;

				}
				// If the player stops channeling, do something else.
				else if (Projectile.ai[0] == 0f)
					Projectile.Kill();

			}

			// Set the rotation so the Projectile points towards where it's going.
			if (Projectile.velocity != Vector2.Zero)
			{
				Projectile.spriteDirection = Projectile.direction*-1;
				Main.player[Projectile.owner].direction = Projectile.spriteDirection * -1;
			}
			//Animation and firing in terms of frameCounter and first counter
			if (++Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.player[Projectile.owner].direction = Projectile.spriteDirection*-1;
			Main.player[Projectile.owner].velocity = Projectile.velocity/3;
			SoundEngine.PlaySound(HarbingerWooshSound, Projectile.position);
			for (int i = 0; i < Main.rand.Next(4, 7); i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CrowFeathers>());
		}
	}
}
