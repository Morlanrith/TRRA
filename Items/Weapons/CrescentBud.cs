using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
    public class CrescentBud : ModItem
	{
		private static readonly SoundStyle BudSliceSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseSlice")
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
			Item.damage = 52;
			Item.width = 66;
			Item.height = 58;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(silver: 54);
			Item.rare = ItemRarityID.Orange;
			Item.crit = 10;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.shoot = ProjectileID.None;
			Item.UseSound = BudSliceSound;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<DustWeaponKit>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 10)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

	}



}