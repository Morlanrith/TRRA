using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TRRA.Projectiles.Item.Weapon.Harbinger;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
	public class HarbingerSc : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harbinger");
			Tooltip.SetDefault("This magic weapon shoots missiles that follow your cursor.");
		}

		public override void SetDefaults()
		{
			Item.damage = 140;
			Item.DamageType = DamageClass.Melee;
			Item.width = 26;
			Item.height = 26;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(gold: 25);
			Item.buffType = BuffType<CorvidBuff>();
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.DD2_SkyDragonsFurySwing;
			Item.shoot = ProjectileID.MonkStaffT3;
			Item.shootSpeed = 24f;
			Item.autoReuse = true;
			Item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player)
        {
			return true;
        }

		public override bool CanUseItem(Player player)
		{
			if (player.HasBuff<CorvidBuff>()) return false;
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.useAnimation = 1;
				Item.shootSpeed = 10f;
				Item.UseSound = null;
				Item.shoot = ProjectileType<HarbingerCorvid>();
				Item.autoReuse = false;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.useAnimation = 30;
				Item.shootSpeed = 24f;
				Item.UseSound = SoundID.DD2_SkyDragonsFurySwing;
				Item.shoot = ProjectileID.MonkStaffT3;
				Item.autoReuse = true;
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2) {
				player.AddBuff(Item.buffType, 2);
				SoundEngine.PlaySound(SoundID.Item9, position);
				Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
				return false;
			}
			return true;
        }
	}
}