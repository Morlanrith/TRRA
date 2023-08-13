using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using static Terraria.ModLoader.ModContent;
using TRRA.Dusts;
using Terraria.DataStructures;
using TRRA.Projectiles.Item.Weapon.SunderedRose;

namespace TRRA.Projectiles.Item.Weapon.CrescentRose
{
	public class PetalBurstBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

        public override bool RightClick(int buffIndex)
        {
            return false;
        }

        public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ProjectileType<PetalBurst>()] > 0 || player.ownedProjectileCounts[ProjectileType<WhitePetalBurst>()] > 0)
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
	public class PetalBurst : ModProjectile
	{
		private Vector2 targetPos;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 84;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

        public override bool? CanCutTiles()
        {
			return false;
        }

		private bool AtTarget()
		{
			float xVal = Math.Abs(Projectile.position.X - targetPos.X);
            float yVal = Math.Abs(Projectile.position.Y - targetPos.Y);
			return xVal < 15f && yVal < 15f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            targetPos = new(Projectile.ai[0], Projectile.ai[1]);
			Vector2 targetVec = (targetPos-Projectile.position)*player.direction;
			targetVec.Normalize();
			Projectile.rotation = targetVec.ToRotation();
			Projectile.spriteDirection = player.direction;
        }

        public override void AI()
		{
			if (AtTarget()) Projectile.Kill();

            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<RosePetal>(), Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 0.7f);
            }
            // In Multi Player (MP) This code only runs on the client of the Projectile's owner, this is because it relies on mouse position, which isn't the same across all clients.
            if (Main.myPlayer == Projectile.owner)
			{

				Player player = Main.player[Projectile.owner];
				// If the player channels the weapon, do something. This check only works if item.channel is true for the weapon.
				if (!(player.CCed || player.dead || player.mount.Active || player.grappling[0] > -1 || PlayerInput.Triggers.JustPressed.Grapple) && player.HeldItem.type == ItemType<Items.Weapons.CrescentRoseS>())
				{
					float maxDistance = 20f; // This also sets the maximun speed the Projectile can reach while following the cursor.
					Vector2 vectorToPos = targetPos - Projectile.position;
					float distanceToPos = vectorToPos.Length();

					// Here we can see that the speed of the Projectile depends on the distance to the cursor.
					if (distanceToPos > maxDistance)
					{
						distanceToPos = maxDistance / distanceToPos;
						vectorToPos *= distanceToPos;
					}

					int velocityXBy1000 = (int)(vectorToPos.X * 1000f);
					int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
					int velocityYBy1000 = (int)(vectorToPos.Y * 1000f);
					int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);

					// This code checks if the precious velocity of the Projectile is different enough from its new velocity, and if it is, syncs it with the server and the other clients in MP.
					// We previously multiplied the speed by 1000, then casted it to int, this is to reduce its precision and prevent the speed from being synced too much.
					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
					{
						Projectile.netUpdate = true;
					}

					Projectile.velocity = vectorToPos;
					player.position = Projectile.position;

				}
				// If the player stops channeling, do something else.
				else
				{
					Projectile.Kill();
				}

			}

			// Set the rotation so the Projectile points towards where it's going.
			if (Projectile.velocity != Vector2.Zero)
			{
				Main.player[Projectile.owner].direction = Projectile.spriteDirection;
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
			Main.player[Projectile.owner].direction = Projectile.spriteDirection;
			Main.player[Projectile.owner].velocity = Projectile.velocity/3;
			for (int i = 0; i < Main.rand.Next(4, 7); i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<RosePetal>());
		}
	}
}
