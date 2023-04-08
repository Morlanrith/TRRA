using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TRRA.Projectiles.Item.Weapon.FloatingArray;
using Terraria.GameInput;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
    public class FloatingArray : ModItem
	{
		private bool canSummon = true, laserFiring = false;
		private int bladeAmount = 0;

		private static readonly SoundStyle SwordSummonSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/FloatingArray/SwordSummon")
		{
			Volume = 0.5f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() 
		{
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.damage = 85;
			Item.width = 33;
			Item.height = 35;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Summon;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.mana = 10;
			Item.crit = 26;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.shootSpeed = 10f;
			Item.shoot = ProjectileType<ArrayBlade>();
			Item.buffType = ModContent.BuffType<ArrayBladeBuff>();
			Item.UseSound = SwordSummonSound;
			Item.noMelee = true;
			Item.knockBack = 4f;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<PlantDustCrystal>(), 50)
			.AddIngredient(ItemType<GravityDustCrystal>(), 30)
			.AddIngredient(ItemID.BlackPaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 40)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		private void ResetValues()
		{
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.mana = 10;
			Item.UseSound = SwordSummonSound;
			Item.noUseGraphic = false;
			Item.channel = false;
			Item.shoot = ProjectileType<ArrayBlade>();
			Item.shootSpeed = 10f;
		}

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (PlayerInput.Triggers.JustReleased.MouseRight) canSummon = false;
			if (!player.channel && laserFiring)
			{
				laserFiring = false;
				if (bladeAmount > player.maxMinions) bladeAmount = player.maxMinions;
				for(int i=0; i<bladeAmount; i++)
					CreateBlade(player, new EntitySource_ItemUse_WithAmmo(player,Item,Item.ammo), player.position, player.velocity, ProjectileType<ArrayBlade>(), Item.damage, Item.knockBack);
			}
			if (player.altFunctionUse != 2 && player.itemAnimation == 0)
			{
				ResetValues();
				canSummon = true;
			}
		}

		public override bool CanUseItem(Player player)
		{
			if (!canSummon && player.itemAnimation == 0) return false;

			if (player.altFunctionUse == 2)
				ResetValues();
			else
			{
				if (!player.HasBuff(ModContent.BuffType<ArrayBladeBuff>())) return false;
				Item.useTime = 21;
				Item.useAnimation = 21;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.UseSound = null;
				Item.noUseGraphic = true;
				Item.channel = true;
				Item.shoot = ProjectileType<ArrayGuns>();
				Item.shootSpeed = 5f;
			}
			return base.CanUseItem(player);
		}

		private void CreateBlade(Player player, IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;
			player.GetModPlayer<TRRAPlayer>().AddBlade(projectile);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.altFunctionUse != 2)
			{
				player.channel = true;
				bladeAmount = player.GetModPlayer<TRRAPlayer>().KillBlades();
				laserFiring = true;
				var projectile = Projectile.NewProjectile(source, position, velocity, type, (damage/6)*bladeAmount, Item.knockBack, player.whoAmI, 30f, 0f);
			}
			else CreateBlade(player, source, position, velocity, type, damage, knockback);
			return false;
        }

    }



}