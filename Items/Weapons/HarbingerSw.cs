using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TRRA.Projectiles.Item.Weapon.Harbinger;
using Terraria.Audio;
using TRRA.Dusts;
using TRRA.Tiles;
using TRRA.Items.Materials;

namespace TRRA.Items.Weapons
{
	public class HarbingerSw : ModItem
	{
		private static readonly SoundStyle HarbingerSliceSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Harbinger/HarbingerSlice")
		{
			Volume = 0.4f,
			Pitch = 0f,
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harbinger");
			Tooltip.SetDefault("'Doubles as a bad luck charm'\nRight Click to fire as a gun\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults()
		{
			Item.damage = 125;
			Item.crit = 20;
			Item.DamageType = DamageClass.Melee;
			Item.width = 74;
			Item.height = 62;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = HarbingerSliceSound;
			Item.autoReuse = true;
			Item.maxStack = 1;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<CrescentBloomS>(), 1) // Change this
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 20)
			.AddIngredient(ItemID.GrayPaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

	}
}