using TRRA.Dusts;
using TRRA.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameInput;
using TRRA.Tiles;
using Terraria.DataStructures;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
    public class CrescentRoseS : ModItem
	{
		private bool canSwing = true;

		private static readonly SoundStyle RoseDashSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseDash")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle RoseSliceSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseSlice")
		{
			Volume = 0.4f,
			Pitch = 0.0f,
		};

		public override void SetStaticDefaults() 
		{
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.damage = 300;
			Item.width = 66;
			Item.height = 58;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(gold: 25);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = RoseSliceSound;
			Item.crit = 26;
			Item.autoReuse = true;
			Item.maxStack = 1;
		}

		public override bool AltFunctionUse(Player player)
		{
			if(player.mount.Active) return false;
			return true;
		}

		private void ResetValues()
        {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = false;
			Item.noUseGraphic = false;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.shoot = ProjectileID.None;
			Item.UseSound = RoseSliceSound;
			Item.autoReuse = true;
		}

		public override bool CanUseItem(Player player)
		{
			// If the player uses the alt function (Right Click), causes the player to dash in the direction they are currently facing
			if (player.altFunctionUse == 2)
			{
				if (!PlayerInput.Triggers.JustPressed.MouseRight) return false;
				canSwing = false;
				Item.useStyle = ItemUseStyleID.Thrust;
				Item.noMelee = true;
				Item.noUseGraphic = true;
				Item.useTime = 40;
				Item.useAnimation = 20;
				Item.autoReuse = false;
				Item.shoot = ProjectileID.PurificationPowder;
				Item.shootSpeed = 16f;
				Item.UseSound = RoseDashSound;
				Vector2 newVelocity = player.velocity;
				newVelocity.X = 10f * player.direction;
				player.velocity = newVelocity;
			}
			else if (!canSwing)
			{
				ResetValues();
				canSwing = true;
				return false;
			}
			return base.CanUseItem(player);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(3))
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<RosePetal>());
			}
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<CrescentBloomS>(), 1)
			.AddIngredient(ItemType<FireDustCrystal>(), 10)
			.AddIngredient(ItemType<PlantDustCrystal>(), 10)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemType<IceDustCrystal>(), 10)
			.AddIngredient(ItemID.RedPaint, 10)
			.AddIngredient(ItemType<EssenceOfGrimm>(), 20)
			.AddIngredient(ItemType<DustExtract>(), 1)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

        // Shoot override, used for the dash (doesn't actually generate a projectile)
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int dustQuantity = 5;
			for (int i = 0; i < dustQuantity; i++)
			{
				Vector2 dustOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 32f;
				int dust = Dust.NewDust(player.position + dustOffset, Item.width, Item.height, DustType<RosePetal>());
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].scale = 1.5f;
			}
			return false;
		}

	}



}