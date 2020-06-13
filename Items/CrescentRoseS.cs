using TRRA.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Items
{
    public class CrescentRoseS : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Crescent Rose");
			Tooltip.SetDefault("'It's also a gun'\nRight Click to dash\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() 
		{
			item.damage = 130;
			item.width = 64;
			item.height = 56;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.melee = true;
			item.knockBack = 7;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Cyan;
			item.crit = 26;
			item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

        public override bool CanUseItem(Player player)
		{
			// If the player uses the alt function (Right Click), causes the player to dash in the direction they are currently facing
			if (player.altFunctionUse == 2)
			{
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/RoseDash");
				item.noUseGraphic = true;
				item.useStyle = ItemUseStyleID.Stabbing;
				item.damage = 0;
				item.autoReuse = false;
				item.noMelee = true;
				item.ranged = true;
				item.shoot = ProjectileID.PurificationPowder;
				item.shootSpeed = 16f;
				item.useTime = 40;
				item.useAnimation = 40;
				Vector2 newVelocity = player.velocity;
				newVelocity.X = 10f * player.direction;
				player.velocity = newVelocity;
			}
			else
            {
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/RoseSlice");
				item.noUseGraphic = false;
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.damage = 130;
				item.useTime = 25;
				item.useAnimation = 25;
				item.autoReuse = true;
				item.noMelee = false;
				item.ranged = false;
				item.shoot = ProjectileID.None;
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

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DeathSickle, 1);
			recipe.AddIngredient(ItemID.SniperRifle, 1);
			recipe.AddIngredient(ItemID.RedPaint, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		// Shoot override, used for the dash (doesn't actually generate a projectile)
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int dustQuantity = 5;
			for (int i = 0; i < dustQuantity; i++)
			{
				Vector2 dustOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 32f;
				int dust = Dust.NewDust(player.position + dustOffset, item.width, item.height, DustType<RosePetal>());
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].scale = 1.5f;
			}
			return false;
		}


	}



}