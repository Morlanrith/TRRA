using TRRA.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRRA.Tiles;
using Microsoft.Xna.Framework;
using TRRA.Projectiles.Item.Weapon.Hush;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
    public class Whisper : ModItem
	{
		private static readonly SoundStyle HushOpenSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Hush/HushOpen")
		{
			Volume = 0.3f,
			Pitch = 0.0f,
		};

		private static readonly SoundStyle HushStabSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Hush/HushStab")
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
			Item.width = 50;
			Item.height = 48;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(gold: 8);
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.damage = 110;
			Item.crit = 47;
			Item.knockBack = 5f;
			Item.UseSound = HushStabSound;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ProjectileType<WhisperClosed>();
			Item.noUseGraphic = true;
			Item.shootSpeed = 5f;
			Item.noMelee = true;
			Item.autoReuse = true;
		}

		public override void HoldItem(Player player)
        {
			if (player.itemAnimation == 0 && !player.mount.Active)
			{
				player.itemLocation.X = player.position.X + (float)player.width * 0.5f - (float)(16 * player.direction);
				player.itemLocation.Y = player.position.Y + 18f;
				player.fallStart = (int)(player.position.Y / 16f);
				if (player.gravDir == -1f)
				{
					Item.noUseGraphic = false;
					Item.useStyle = ItemUseStyleID.Thrust;
					if (Item.holdStyle != 2)
					{
						Item.holdStyle = 2;
						SoundEngine.PlaySound(HushOpenSound);
					}
					player.itemLocation.Y = player.position.Y + (float)player.height + (player.position.Y - player.itemLocation.Y);
					if (player.velocity.Y < -2f && !player.controlDown)
					{
						player.velocity.Y = -2f;
					}
				}
				else if (player.velocity.Y > 2f && !player.controlDown)
				{
					Item.noUseGraphic = false;
					Item.useStyle = ItemUseStyleID.Thrust;
					if (Item.holdStyle != 2)
					{
						Item.holdStyle = 2;
						SoundEngine.PlaySound(HushOpenSound);
					}
					player.velocity.Y = 2f;
				}
				else
				{
					Item.holdStyle = 0;
				}
			}
			base.HoldItem(player);
		}

		public override bool CanUseItem(Player player)
		{
			Item.holdStyle = 0;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			// Offsets the weapon model, so it is being held correctly
			return new Vector2(0, -12);
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemType<Quiet>(), 1)
			.AddIngredient(ItemType<IceDustCrystal>(), 30)
			.AddIngredient(ItemType<GravityDustCrystal>(), 10)
			.AddIngredient(ItemID.BlackThread, 1)
			.AddIngredient(ItemID.SoulofFright, 20)
			.AddTile(TileType<DustToolbenchTile>())
			.Register();

	}



}