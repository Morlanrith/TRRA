using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Weapon.Myrtenaster;
using TRRA.Tiles;
using TRRA.Items.Materials;

namespace TRRA.Items.Weapons
{
	public class MyrtenasterF : ModItem
	{
		private bool resetTime = false;

		private static readonly SoundStyle FireWaveSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Myrtenaster/FireWave")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() {
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 46;
			Item.rare = ItemRarityID.Cyan;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;
			Item.damage = 80;
			Item.crit = 10;
			Item.knockBack = 4f;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<MyrtenasterFR>();
			Item.shootSpeed = 15f;
			Item.value = Item.sellPrice(gold: 25);
			Item.useStyle = ItemUseStyleID.Rapier; // 13
			Item.useAnimation = 18;
			Item.useTime = 6;
			Item.maxStack = 1;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<SilbernelkeF>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 20)
			.AddIngredient(ItemType<IceDustCrystal>(), 20)
			.AddIngredient(ItemID.WhitePaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.altFunctionUse != 2)
			{
				target.AddBuff(BuffID.OnFire, 180);
			}
		}


		public override void HoldItem(Player player)
		{
			if (player.altFunctionUse == 2) player.itemRotation = 0f;
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (player.altFunctionUse == 2)
			{
				if (PlayerInput.Triggers.JustReleased.MouseRight) //Stops the animation manually
				{
					resetTime = true;
				}
				if (player.itemAnimation == 1) //Resets the animation so it doesn't let the hand return to resting position
				{
					if (!resetTime)
					{
						player.itemAnimation = Item.useAnimation;
						SoundEngine.PlaySound(FireWaveSound, player.Center);
					}
					else resetTime = false;
				}
			}
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.DamageType = DamageClass.Ranged;
				Item.useTime = 45;
				Item.useAnimation = 45;
				Item.damage = 135;
				Item.knockBack = 7.5f;
				Item.shoot = ProjectileType<MyrtenasterFS>();
				Item.UseSound = FireWaveSound;
				Item.noUseGraphic = false;
				Item.channel = false;
				Item.shootSpeed = 6f;
				Item.autoReuse = true;
			}
			else {
				Item.noUseGraphic = true;
				Item.channel = true;
				Item.useStyle = ItemUseStyleID.Rapier;
				Item.DamageType = DamageClass.Melee;
				Item.useAnimation = 18;
				Item.useTime = 6;
				Item.damage = 80;
				Item.shoot = ProjectileType<MyrtenasterFR>();
				Item.shootSpeed = 15f;
				Item.UseSound = null;
				Item.autoReuse = false;
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(6, -11);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
				Projectile.NewProjectile(source, position, velocity, type, (int)(135 * player.GetDamage(DamageClass.Ranged).Additive), Item.knockBack, player.whoAmI);
			else
				Projectile.NewProjectile(source, position, velocity, type, (int)(80 * player.GetDamage(DamageClass.Melee).Additive), Item.knockBack, player.whoAmI);
			return false;
		}
	}
}