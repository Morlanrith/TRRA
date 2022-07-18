using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
    public class CrescentBloomS : ModItem
	{

		private static readonly SoundStyle BloomSliceSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseSlice")
		{
			Volume = 0.4f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Crescent Bloom");
			Tooltip.SetDefault("'Justice will be swift!'\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() 
		{
			Item.damage = 118;
			Item.width = 66;
			Item.height = 58;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.Pink;
			Item.crit = 18;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.shoot = ProjectileID.None;
			Item.UseSound = BloomSliceSound;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<CrescentBud>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 10)
			.AddIngredient(ItemID.RedPaint, 5)
			.AddIngredient(ItemID.SoulofSight, 20)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

	}



}