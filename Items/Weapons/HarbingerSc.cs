using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using TRRA.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TRRA.Projectiles.Item.Weapon.Harbinger;

namespace TRRA.Items.Weapons
{
	public class HarbingerSc : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harbinger");
			Tooltip.SetDefault("This magic weapon shoots missiles that follow your cursor.");
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Melee;
			Item.width = 26;
			Item.height = 26;
			Item.useTime = 15;
			Item.useAnimation = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.channel = true; //Channel so that you can held the weapon [Important]
			Item.noUseGraphic = true;
			Item.knockBack = 8;
			Item.value = Item.sellPrice(silver: 50);
			Item.buffType = BuffType<CorvidBuff>();
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item9;
			Item.shoot = ProjectileType<HarbingerCorvid>();
			Item.shootSpeed = 10f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			return false;
        }

        public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 20)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();
	}
}