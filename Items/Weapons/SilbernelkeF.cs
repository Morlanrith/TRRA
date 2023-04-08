using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Weapon.Myrtenaster;

namespace TRRA.Items.Weapons
{
	public class SilbernelkeF : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Silbernelke");
			// Tooltip.SetDefault("'Not the start of its name, and certainly not the end'");
		}

		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 46;
			Item.rare = ItemRarityID.Pink;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;
			Item.damage = 30;
			Item.crit = 5;
			Item.knockBack = 4f;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<SilbernelkeFR>();
			Item.shootSpeed = 15f;
			Item.value = Item.sellPrice(gold: 8);
			Item.useStyle = ItemUseStyleID.Rapier; // 13
			Item.useAnimation = 18;
			Item.useTime = 6;
			Item.maxStack = 1;
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -11);
		}
	}
}