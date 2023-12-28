using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TRRA.Projectiles.Item.Weapon.EmberCelica;

namespace TRRA.Items.Weapons
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class FireFists : ModItem
	{
		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<EmberCelicaS>();
        }

		public override void SetDefaults() {
			Item.damage = 140;
			Item.DamageType = DamageClass.Magic;
			Item.width = 38;
			Item.height = 42;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 7;
			Item.noUseGraphic = true;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item105;
			Item.autoReuse = false;
			Item.shoot = ProjectileType<FireBlast>();
			Item.shootSpeed = 9f;
			Item.mana = 10;
			Item.noMelee = true;
			Item.crit = 26;
		}

		public override bool AltFunctionUse(Player player) {
			if (player.mount.Active) return false;
			return true;
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				if (!PlayerInput.Triggers.JustPressed.MouseRight) return false; //Equivalent to autoReuse being set to false, as that flag is bugged with alternate use
                Item.UseSound = SoundID.Item62;
                Item.useStyle = ItemUseStyleID.Thrust;
				Item.shoot = ProjectileID.PurificationPowder;
				Item.useTime = 40;
				Item.useAnimation = 40;
                Item.mana = 0;
                Vector2 newVelocity = player.velocity;
				newVelocity.X = 8.5f * player.direction;
				player.velocity = newVelocity;
			}
			else {
				if (!PlayerInput.Triggers.JustPressed.MouseLeft) return false; //Equivalent to autoReuse being set to false, as that flag is bugged with alternate use
                Item.UseSound = SoundID.Item105;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.shoot = ProjectileType<FireBlast>();
                Item.useTime = 20;
				Item.useAnimation = 20;
                Item.mana = 10;
            }
            return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
            {
				Projectile.NewProjectile(source, position, velocity * .25f, ProjectileType<FirePunch>(), (int)(280 * player.GetDamage(DamageClass.Melee).Additive), 8, player.whoAmI);
				return true;
			}
			else Projectile.NewProjectile(source, position, velocity * .25f, ProjectileType<FirePunch>(), (int)(300 * player.GetDamage(DamageClass.Melee).Additive), 12, player.whoAmI, 1);
			return false; // return false because we don't want to shoot automatic projectile
		}

	}
}