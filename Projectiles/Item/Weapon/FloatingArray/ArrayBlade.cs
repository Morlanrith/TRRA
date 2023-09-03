using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Projectiles.Item.Weapon.FloatingArray
{

	public class ArrayBladeBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ArrayBlade>()] > 0)
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

	public class ArrayBlade : ModProjectile
	{
		private static List<int> _sword_blacklistedTargets = new List<int>();
		private static Asset<Texture2D> threadTexture;

		public override void SetStaticDefaults() {
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void Load()
		{
			threadTexture = ModContent.Request<Texture2D>("TRRA/Projectiles/Item/Weapon/FloatingArray/BladeThread");
		}

		public override void SetDefaults() {
			Projectile.netImportant = true;
			Projectile.width = 14;
			Projectile.height = 48;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minionSlots = 1f;
			Projectile.timeLeft *= 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.scale = 1f;
			Projectile.manualDirectionChange = true;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return target.CanBeChasedBy();
		}

		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<ArrayBladeBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<ArrayBladeBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}



		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0f, 0.9f, 0.46f);
			CheckActive(Main.player[Projectile.owner]);
			Swords_Main();
		}

		// AI adapted from the existing code for the Terraprisma blades
		private void Swords_Main()
		{
			List<int> ai156_blacklistedTargets = _sword_blacklistedTargets;
			Player player = Main.player[Projectile.owner];
			DelegateMethods.v3_1 = Color.Transparent.ToVector3();
			Point point2 = Projectile.Center.ToTileCoordinates();
			DelegateMethods.CastLightOpen(point2.X, point2.Y);
			ai156_blacklistedTargets.Clear();
			Swords_Think(ai156_blacklistedTargets);
			Projectile.spriteDirection = player.direction;
		}

		private void Swords_Think(List<int> blacklist)
		{
			int num = 40;
			int num2 = num - 1;
			int num3 = num + 40;
			int num4 = num3 - 1;
			int num5 = num + 1;
			Player player = Main.player[Projectile.owner];
			if (player.active && Vector2.Distance(player.Center, Projectile.Center) > 2000f)
			{
				Projectile.ai[0] = 0f;
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[0] == -1f)
			{
				Swords_GetMyGroupIndexAndFillBlackList(blacklist, out var index, out var totalIndexesInGroup);
				Swords_GetIdlePosition(index, totalIndexesInGroup, out var idleSpot, out var idleRotation);
				Projectile.velocity = Vector2.Zero;
				Projectile.Center = Projectile.Center.MoveTowards(idleSpot, 32f);
				Projectile.rotation = Projectile.rotation.AngleLerp(idleRotation, 0.2f);
				if (Projectile.Distance(idleSpot) < 2f)
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
				}
				return;
			}
			if (Projectile.ai[0] == 0f)
			{
				Swords_GetMyGroupIndexAndFillBlackList(blacklist, out var index3, out var totalIndexesInGroup3);
				Swords_GetIdlePosition(index3, totalIndexesInGroup3, out var idleSpot3, out var idleRotation3);
				Projectile.velocity = Vector2.Zero;
				Projectile.Center = Vector2.SmoothStep(Projectile.Center, idleSpot3, 0.45f);
				Projectile.rotation = Projectile.rotation.AngleLerp(idleRotation3, 0.45f);
				if (Main.rand.NextBool(20))
				{
					int num7 = Swords_TryAttackingNPCs(blacklist);
					if (num7 != -1)
					{
						Projectile.ai[0] = Main.rand.NextFromList<int>(num, num3);
						Projectile.ai[0] = num3;
						Projectile.ai[1] = num7;
						Projectile.netUpdate = true;
					}
				}
				return;
			}
			bool skipBodyCheck = true;
			int num14 = 0;
			int num15 = num2;
			int num16 = 0;
			if (Projectile.ai[0] >= (float)num5)
			{
				num14 = 1;
				num15 = num4;
				num16 = num5;
			}
			int num17 = (int)Projectile.ai[1];
			if (!Main.npc.IndexInRange(num17))
			{
				int num18 = Swords_TryAttackingNPCs(blacklist, skipBodyCheck);
				if (num18 != -1)
				{
					Projectile.ai[0] = Main.rand.NextFromList<int>(num, num3);
					Projectile.ai[1] = num18;
					Projectile.netUpdate = true;
				}
				else
				{
					Projectile.ai[0] = -1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
				return;
			}
			NPC nPC2 = Main.npc[num17];
			if (!nPC2.CanBeChasedBy(this))
			{
				int num19 = Swords_TryAttackingNPCs(blacklist, skipBodyCheck);
				if (num19 != -1)
				{
					Projectile.ai[0] = Main.rand.NextFromList<int>(num, num3);
					Projectile.ai[1] = num19;
					Projectile.netUpdate = true;
				}
				else
				{
					Projectile.ai[0] = -1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
				return;
			}
			Projectile.ai[0] -= 1f;
			if (Projectile.ai[0] >= (float)num15)
			{
				Projectile.direction = ((Projectile.Center.X < nPC2.Center.X) ? 1 : (-1));
				if (Projectile.ai[0] == (float)num15)
				{
					Projectile.localAI[0] = Projectile.Center.X;
					Projectile.localAI[1] = Projectile.Center.Y;
				}
			}
			float lerpValue2 = Utils.GetLerpValue(num15, num16, Projectile.ai[0], clamped: true);
			if (num14 == 0)
			{
				Vector2 vector5 = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
				if (lerpValue2 >= 0.5f)
				{
					vector5 = Vector2.Lerp(nPC2.Center, Main.player[Projectile.owner].Center, 0.5f);
				}
				Vector2 center2 = nPC2.Center;
				float num20 = (center2 - vector5).ToRotation();
				float num21 = ((Projectile.direction == 1) ? (-(float)Math.PI) : ((float)Math.PI));
				float num22 = num21 + (0f - num21) * lerpValue2 * 2f;
				Vector2 spinningpoint2 = num22.ToRotationVector2();
				spinningpoint2.Y *= 0.5f;
				spinningpoint2.Y *= 0.8f + (float)Math.Sin((float)Projectile.identity * 2.3f) * 0.2f;
				spinningpoint2 = spinningpoint2.RotatedBy(num20);
				float num23 = (center2 - vector5).Length() / 2f;
				Vector2 vector7 = (Projectile.Center = Vector2.Lerp(vector5, center2, 0.5f) + spinningpoint2 * num23);
				float num24 = MathHelper.WrapAngle(num20 + num22 + 0f);
				Projectile.rotation = num24 + (float)Math.PI / 2f;
				Vector2 vector8 = (Projectile.velocity = num24.ToRotationVector2() * 10f);
				Projectile.position -= Projectile.velocity;
			}
			if (num14 == 1)
			{
				Vector2 vector9 = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
				vector9 += new Vector2(0f, Utils.GetLerpValue(0f, 0.4f, lerpValue2, clamped: true) * -100f);
				Vector2 v = nPC2.Center - vector9;
				Vector2 vector10 = v.SafeNormalize(Vector2.Zero) * MathHelper.Clamp(v.Length(), 60f, 150f);
				Vector2 value = nPC2.Center + vector10;
				float lerpValue3 = Utils.GetLerpValue(0.4f, 0.6f, lerpValue2, clamped: true);
				float lerpValue4 = Utils.GetLerpValue(0.6f, 1f, lerpValue2, clamped: true);
				float targetAngle = v.SafeNormalize(Vector2.Zero).ToRotation() + (float)Math.PI / 2f;
				Projectile.rotation = Projectile.rotation.AngleTowards(targetAngle, (float)Math.PI / 5f);
				Projectile.Center = Vector2.Lerp(vector9, nPC2.Center, lerpValue3);
				if (lerpValue4 > 0f)
				{
					Projectile.Center = Vector2.Lerp(nPC2.Center, value, lerpValue4);
				}
			}
			if (Projectile.ai[0] == (float)num16)
			{
				int num25 = Swords_TryAttackingNPCs(blacklist, skipBodyCheck);
				if (num25 != -1)
				{
					Projectile.ai[0] = Main.rand.NextFromList<int>(num, num3);
					Projectile.ai[1] = num25;
					Projectile.netUpdate = true;
				}
				else
				{
					Projectile.ai[0] = -1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
			}
		}

		private int Swords_TryAttackingNPCs(List<int> blackListedTargets, bool skipBodyCheck = false)
		{
			Vector2 center = Main.player[Projectile.owner].Center;
			int result = -1;
			float num = -1f;
			NPC ownerMinionAttackTargetNPC = Projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this))
			{
				bool flag = true;
				if (!ownerMinionAttackTargetNPC.boss && blackListedTargets.Contains(ownerMinionAttackTargetNPC.whoAmI))
				{
					flag = false;
				}
				if (ownerMinionAttackTargetNPC.Distance(center) > 1000f)
				{
					flag = false;
				}
				if (!skipBodyCheck && !Projectile.CanHitWithOwnBody(ownerMinionAttackTargetNPC))
				{
					flag = false;
				}
				if (flag)
				{
					return ownerMinionAttackTargetNPC.whoAmI;
				}
			}
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.CanBeChasedBy(this) && (nPC.boss || !blackListedTargets.Contains(i)))
				{
					float num2 = nPC.Distance(center);
					if (!(num2 > 1000f) && (!(num2 > num) || num == -1f) && (skipBodyCheck || Projectile.CanHitWithOwnBody(nPC)))
					{
						num = num2;
						result = i;
					}
				}
			}
			return result;
		}

		private void Swords_GetMyGroupIndexAndFillBlackList(List<int> blackListedTargets, out int index, out int totalIndexesInGroup)
		{
			index = 0;
			totalIndexesInGroup = 0;
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.owner == Projectile.owner && projectile.type == Projectile.type && (projectile.type != 759 || projectile.frame == Main.projFrames[projectile.type] - 1))
				{
					if (Projectile.whoAmI > i)
					{
						index++;
					}
					totalIndexesInGroup++;
				}
			}
		}

		private void Swords_GetIdlePosition(int stackedIndex, int totalIndexes, out Vector2 idleSpot, out float idleRotation)
		{
			Player player = Main.player[Projectile.owner];
			idleRotation = 0f;
			idleSpot = Vector2.Zero;
			int num3 = stackedIndex + 1;
			idleRotation = (float)num3 * ((float)Math.PI * 2f) * (1f / 60f) * (float)player.direction + (float)Math.PI / 2f;
			idleRotation = MathHelper.WrapAngle(idleRotation);
			int num4 = num3 % totalIndexes;
			Vector2 vector = new Vector2(0f, 0.5f).RotatedBy((player.miscCounterNormalized * (2f + (float)num4) + (float)num4 * 0.5f + (float)player.direction * 1.3f) * ((float)Math.PI * 2f)) * 4f;
			idleSpot = idleRotation.ToRotationVector2() * 10f + player.MountedCenter + new Vector2(player.direction * (num3 * -6 - 16), player.gravDir * -15f);
			idleSpot += vector;
			idleRotation += (float)Math.PI / 2f;
		}

		public override bool PreDrawExtras()
		{
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 center = Projectile.Bottom.RotatedBy(Projectile.rotation,Projectile.Center);
			Vector2 directionToPlayer = playerCenter - center;
			float threadRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
			float distanceToPlayer = directionToPlayer.Length();

			while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer))
			{
				directionToPlayer /= distanceToPlayer;
				directionToPlayer *= threadTexture.Height();

				center += directionToPlayer;
				directionToPlayer = playerCenter - center;
				distanceToPlayer = directionToPlayer.Length();

				Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

				// Draw thread
				Main.EntitySpriteDraw(threadTexture.Value, center - Main.screenPosition,
					new Rectangle(0, 0, 1, Terraria.GameContent.TextureAssets.Chain30.Value.Height), drawColor, threadRotation,
					threadTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}
			return false;
		}

        public override void OnKill(int timeLeft)
        {
			Main.player[Projectile.owner].GetModPlayer<TRRAPlayer>().RemoveBlade(Projectile);
        }
    }
}
