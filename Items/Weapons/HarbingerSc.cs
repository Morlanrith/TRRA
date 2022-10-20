using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TRRA.Projectiles.Item.Weapon.Harbinger;
using Terraria.Audio;
using TRRA.Dusts;

namespace TRRA.Items.Weapons
{
	public class HarbingerSc : ModItem
	{
		private static readonly SoundStyle HarbingerCorvidSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Harbinger/HarbingerCorvidTransform")
		{
			Volume = 0.4f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harbinger");
			Tooltip.SetDefault("'Doubles as a bad luck charm'\nRight Click to take the form of a Corvid\nTransforms by pressing a mapped hotkey");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 125;
			Item.crit = 20;
			Item.DamageType = DamageClass.Melee;
			Item.width = 76;
			Item.height = 66;
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
			Item.UseSound = null;
			Item.shoot = ProjectileType<HarbingerScythe>();
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
				Item.shoot = ProjectileType<HarbingerCorvid>();
				Item.autoReuse = false;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.useAnimation = 30;
				Item.shootSpeed = 24f;
				Item.shoot = ProjectileType<HarbingerScythe>();
				Item.autoReuse = true;
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2) {
				player.AddBuff(Item.buffType, 2);
				SoundEngine.PlaySound(HarbingerCorvidSound, position);
				Projectile.NewProjectileDirect(source, player.direction == 1 ? player.Left : player.Right, velocity, type, damage, knockback, Main.myPlayer);
				for (int i = 0; i < Main.rand.Next(5,10); i++)
					Dust.NewDust(player.Top, player.width, player.height, DustType<CrowFeathers>());
				return false;
			}
			return true;
        }
	}
}