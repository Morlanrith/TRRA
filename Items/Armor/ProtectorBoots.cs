using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ProtectorBoots : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Protector Boots");
			Tooltip.SetDefault("'Take a message of hope to the stars'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

		public override void AddRecipes() => CreateRecipe()
            .AddIngredient(ItemID.Nanites, 1)
            .AddIngredient(ItemID.BlackPaint, 1)
            .AddRecipeGroup("IronBar", 5)
            .AddTile(TileID.Anvils)
            .Register();
	}
}
