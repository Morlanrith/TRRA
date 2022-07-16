using System;
using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;
using TRRA.Projectiles.Item.Weapon.Myrtenaster;
using Terraria.DataStructures;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
	public class FakeStarlight : ModItem
	{

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Myrtenaster (Fake)");
			Tooltip.SetDefault("For those who are more than just a name\nRight Click to fire a summoned sword\nTransforms by pressing a mapped hotkey");
		}

		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 46;
			Item.rare = ItemRarityID.Yellow;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;
			Item.damage = 90;
			Item.crit = 10;
			Item.knockBack = 4f;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<FakeStarlightProj>();
			Item.shootSpeed = 15f;
			Item.value = Item.sellPrice(0, 5);
			Item.useStyle = ItemUseStyleID.Rapier; // 13
			Item.useAnimation = 18;
			Item.useTime = 6;
		}
	}
}