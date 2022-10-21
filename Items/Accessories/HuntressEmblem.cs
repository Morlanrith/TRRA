using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Accessories
{
	public class HuntressEmblem : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Huntress Emblem");
			Tooltip.SetDefault("For the best and brightest warriors of Remnant\n14% increased melee damage\n14% increased ranged damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage(DamageClass.Melee) += 0.14f;
			player.GetDamage(DamageClass.Ranged) += 0.14f;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.WarriorEmblem, 1)
			.AddIngredient(ItemID.RangerEmblem, 1)
			.AddTile(TileID.TinkerersWorkbench)
			.Register();
		
	}
}
