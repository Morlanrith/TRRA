using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Tiles;
using TRRA.Projectiles.Item.Weapon.GambolShroud;
using Terraria.DataStructures;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{

    public class GambolShroudNS : ModItem
	{
		private bool canParry = true;

		private static readonly SoundStyle ShadowCloneSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/GambolShroud/ShadowClone")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<GambolShroudS>();
        }

        public override void SetDefaults() {
			Item.damage = 210;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 44;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.crit = 24;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.shoot = ProjectileType<GambolNBlade>();
			Item.shootSpeed = 5f;
			Item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player)
		{
			if (player.shadowDodgeTimer == 0) return true;
			return false;
		}

		private void ResetValues()
		{
			Item.noUseGraphic = false;
			Item.UseSound = ShadowCloneSound;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.channel = false;
			Item.shoot = ProjectileID.None;
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (PlayerInput.Triggers.JustReleased.MouseRight) canParry = false;
			if (player.altFunctionUse != 2 && player.itemAnimation == 0)
			{
				ResetValues();
				canParry = true;
			}
		}

		public override bool CanUseItem(Player player)
		{
			if (!canParry && player.itemAnimation == 0) return false;

			if (player.altFunctionUse == 2)
			{
				ResetValues();
				player.AddBuff(BuffType<ShadowCloneBuff>(), 30);
			}
			else
			{
				Item.noUseGraphic = true;
				Item.UseSound = null;
				Item.useTime = 21;
				Item.useAnimation = 21;
				Item.channel = true;
				Item.shoot = ProjectileType<GambolNBlade>();
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -12);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				Projectile.NewProjectile(source, position, velocity, type, damage, Item.knockBack, player.whoAmI, 30f, 0f);

				return false;
			}
			return true;
		}
	}
}