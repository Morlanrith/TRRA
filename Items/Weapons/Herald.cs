using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using TRRA.Items.Materials;

namespace TRRA.Items.Weapons
{
	public class Herald : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Herald");
			Tooltip.SetDefault("'A dusty, old sword...'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 44;
			Item.crit = 6;
			Item.DamageType = DamageClass.Melee;
			Item.width = 62;
			Item.height = 64;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(silver: 54);
			Item.rare = ItemRarityID.Orange;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.maxStack = 1;
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