using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;
using TRRA.Projectiles.Item.Weapon.Myrtenaster;

namespace TRRA.Items.Weapons
{
	public class Kleineblume : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 46;
			Item.rare = ItemRarityID.Orange;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;
			Item.damage = 15;
			Item.crit = 1;
			Item.knockBack = 4f;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<KleineblumeR>();
			Item.shootSpeed = 15f;
			Item.value = Item.sellPrice(silver: 54);
			Item.useStyle = ItemUseStyleID.Rapier; // 13
			Item.useAnimation = 18;
			Item.useTime = 6;
			Item.maxStack = 1;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 20)
			.AddIngredient(ItemType<IceDustCrystal>(), 20)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();


		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -11);
		}
	}
}