using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class SnowTiara : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Snow Tiara");
			Tooltip.SetDefault("'Carve out your own way'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Shiverthorn, 1)
                .AddIngredient(ItemID.SilverBar, 1)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
				.AddIngredient(ItemID.Shiverthorn, 1)
				.AddIngredient(ItemID.TungstenBar, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
