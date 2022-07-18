using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Microsoft.Xna.Framework;
using TRRA.Projectiles.Item.Weapon.Hush;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
    public class Quiet : ModItem
	{

		private static readonly SoundStyle HushStabSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Hush/HushStab")
		{
			Volume = 0.4f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Quiet");
			Tooltip.SetDefault("'Sarcastic sign not included'");
		}

		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 40;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(silver: 54);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.damage = 20;
			Item.crit = 31;
			Item.knockBack = 5f;
			Item.UseSound = HushStabSound;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<QuietClosed>();
			Item.noUseGraphic = true;
			Item.shootSpeed = 5f;
			Item.noMelee = true;
			Item.autoReuse = true;
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(0, -12);
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 30)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

	}



}