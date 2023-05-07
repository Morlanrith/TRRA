using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.Items.Consumables
{
	public class MoonSummoner : ModItem
	{
		public override void SetStaticDefaults() {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.BloodMoonStarter;
        }

        public override void SetDefaults() {
            Item.width = 22;
            Item.height = 30;
            Item.maxStack = 20;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Yellow;
        }

        public override bool CanUseItem(Player player)
        {
            if (TRRAWorld.BeginShatteredMoon())
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                if (Main.moonPhase == 4) Main.moonPhase = 0;
                if (Main.slimeRain) Main.StopSlimeRain();
                if (LanternNight.LanternsUp) LanternNight.GenuineLanterns = false;
                return true;
            }
            else if (!NPC.downedPlantBoss)
            {
                Main.NewText("The ruler of the jungle is holding the Shattered Moon at bay...", 186, 34, 64);
            }
            return false;
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.position, 0.2f, 0f, 0f);
        }
    }
}