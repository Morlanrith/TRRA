using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TRRA.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ProtectorRibbon : ModItem
	{
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Protector Ribbon");
			Tooltip.SetDefault("'Sal...u...TATIOOOOOOOONS!'");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
			ArmorIDs.Head.Sets.FrontToBackID[Item.headSlot] = EquipLoader.GetEquipSlot(Mod, "ProtectorRibbonTex", EquipType.Head);
        }

        public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

        public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Nanites, 1)
            .AddIngredient(ItemID.PinkPaint, 1)
			.AddRecipeGroup("IronBar", 1)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
