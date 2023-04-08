using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace TRRA.Items.Accessories
{
	public class EyeOfTheHuntress : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Eye of the Huntress");
			// Tooltip.SetDefault("Strike a Grimm down with but a mere sight\n12% increased melee damage\n12% increased ranged damage\n8% increased critical strike chance");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 9);
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage(DamageClass.Melee) += 0.12f;
			player.GetDamage(DamageClass.Ranged) += 0.12f;
			player.GetCritChance(DamageClass.Melee) += 8;
			player.GetCritChance(DamageClass.Ranged) += 8;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<HuntressEmblem>(), 1)
			.AddIngredient(ItemID.EyeoftheGolem, 1)
			.AddTile(TileID.TinkerersWorkbench)
			.Register();
	}
}
