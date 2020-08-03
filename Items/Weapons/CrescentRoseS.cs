using TRRA.Dusts;
using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameInput;
using TRRA.Tiles;

namespace TRRA.Items.Weapons
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
			item.damage = 240;
			item.width = 66;
			item.height = 58;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.melee = true;
			item.knockBack = 7;
			item.value = Item.sellPrice(gold: 25);
			item.rare = ItemRarityID.Cyan;
			item.crit = 26;
			item.autoReuse = true;
			item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player)
		{
			if(player.mount.Active) return false;
			return true;
		}

        public override bool CanUseItem(Player player)
		{
			// If the player uses the alt function (Right Click), causes the player to dash in the direction they are currently facing
			if (player.altFunctionUse == 2)
			{
				if (!PlayerInput.Triggers.JustPressed.MouseRight) return false; //Equivalent to autoReuse being set to false, as that flag is bugged with alternate use
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/CrescentRose/RoseDash");
				item.noUseGraphic = true;
				item.useStyle = ItemUseStyleID.Stabbing;
				item.autoReuse = false;
				item.noMelee = true;
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
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Weapon/CrescentRose/RoseSlice");
				item.noUseGraphic = false;
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = 25;
				item.useAnimation = 25;
				item.autoReuse = true;
				item.noMelee = false;
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
			recipe.AddIngredient(ItemType<DustExtract>(), 1);
			recipe.AddIngredient(ItemType<DustWeaponKit>(), 1);
			recipe.AddIngredient(ItemType<FireDustCrystal>(), 10);
			recipe.AddIngredient(ItemType<PlantDustCrystal>(), 10);
			recipe.AddIngredient(ItemType<GravityDustCrystal>(), 10);
			recipe.AddIngredient(ItemType<IceDustCrystal>(), 10);
			recipe.AddIngredient(ItemID.RedPaint, 10);
			recipe.AddTile(TileType<DustToolbenchTile>());
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