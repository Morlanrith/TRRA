using TRRA.Dusts;
using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Terraria.DataStructures;
using Terraria.Audio;
using TRRA.Projectiles.Item.Weapon.CrescentRose;

namespace TRRA.Items.Weapons
{
    public class CrescentRoseS : ModItem
	{
		private bool canSwing = true;
        private Vector2 newPos;

        private static readonly SoundStyle RoseDashSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseDash")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle RoseSliceSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseSlice")
		{
			Volume = 0.4f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() 
		{
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.damage = 155;
			Item.width = 66;
			Item.height = 58;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = RoseSliceSound;
			Item.crit = 26;
			Item.autoReuse = true;
			Item.maxStack = 1;
            Item.shoot = ProjectileType<CrescentScytheSlash>();
            Item.shootSpeed = 5f;
            Item.shootsEveryUse = true;
            Item.buffType = BuffType<PetalBurstBuff>();
        }

        public override bool AltFunctionUse(Player player)
		{
			if(player.mount.Active) return false;
			return true;
		}

		private void ResetValues()
        {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = false;
			Item.noUseGraphic = false;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.UseSound = RoseSliceSound;
			Item.autoReuse = true;
            Item.shoot = ProjectileType<CrescentScytheSlash>();
            Item.shootSpeed = 5f;
        }

        public override bool CanUseItem(Player player)
		{
            if (player.HasBuff<PetalBurstBuff>()) return false;
            // If the player uses the alt function (Right Click), causes the player to travel towards the current position of the cursor
            if (player.altFunctionUse == 2)
			{
                Vector2 vector = default;
                vector.X = (float)Main.mouseX + Main.screenPosition.X;
                if (player.gravDir == 1f) vector.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;
                else vector.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                vector.X -= player.width / 2;
                if (!(vector.X > 50f) || !(vector.X < (float)(Main.maxTilesX * 16 - 50)) || !(vector.Y > 50f) || !(vector.Y < (float)(Main.maxTilesY * 16 - 50))) return false;
                int num = (int)(vector.X / 16f);
                int num2 = (int)(vector.Y / 16f);
                if ((Main.tile[num, num2].WallType == 87 && (double)num2 > Main.worldSurface && !NPC.downedPlantBoss) || Collision.SolidCollision(vector, player.width, player.height)) return false;
                canSwing = false;
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.useAnimation = 1;
                Item.shootSpeed = 10f;
                Item.shoot = ProjectileType<PetalBurst>();
                newPos = vector;
                return true;
			}
			else if (!canSwing)
			{
				ResetValues();
				canSwing = true;
				return false;
			}
			return base.CanUseItem(player);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(3))
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<RosePetal>());
			}
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<CrescentBloomS>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 10)
			.AddIngredient(ItemID.RedPaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

        // Shoot override, used for the dash (doesn't actually generate a projectile)
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.altFunctionUse == 2)
			{
				int dustQuantity = 5;
				for (int i = 0; i < dustQuantity; i++)
				{
					Vector2 dustOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 32f;
					int dust = Dust.NewDust(player.position + dustOffset, Item.width, Item.height, DustType<RosePetal>());
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity *= 1f;
					Main.dust[dust].scale = 1.5f;
				}
                player.AddBuff(Item.buffType, 2);
                SoundEngine.PlaySound(RoseDashSound, position);
                Projectile.NewProjectileDirect(source, player.direction == 1 ? player.Left : player.Right, velocity, type, damage, knockback, Main.myPlayer, newPos.X, newPos.Y);
                return false;
			}
			return true;

        }

	}



}