using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TRRA.Projectiles.Item.Weapon;
using static Terraria.ModLoader.ModContent;

namespace TRRA.NPCs
{
	[AutoloadHead]
	public class Shopkeep : ModNPC
	{
		public override string Texture => "TRRA/NPCs/Shopkeep";

		public override bool Autoload(ref string name) {
			name = "Shopkeep";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 5;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 1;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults() {
			npc.townNPC = true;
			npc.friendly = true;
			npc.width = 18;
			npc.height = 42;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money) {
			for (int k = 0; k < 255; k++) {
				Player player = Main.player[k];
				if (!player.active) {
					continue;
				}
				foreach (Item item in player.inventory) {
					if (item.type == ItemType<Items.Materials.FireDustCrystal>() || item.type == ItemType<Items.Materials.PlantDustCrystal>() || item.type == ItemType<Items.Materials.GravityDustCrystal>() || item.type == ItemType<Items.Materials.IceDustCrystal>()) {
						return true;
					}
				}
			}
			return false;
		}

		public override string TownNPCName() {
			switch (WorldGen.genRand.Next(8)) {
				case 0:
					return "Kerry";
				case 1:
					return "Miles";
				case 2:
					return "Eddy";
				case 3:
					return "Chris";
				case 4:
					return "Cole";
				case 5:
					return "Matt";
				case 6:
					return "Brent";
				default:
					return "Jeff";
			}
		}

		public override string GetChat() {
			int traveller = NPC.FindFirstNPC(NPCID.TravellingMerchant);
			if (traveller >= 0 && Main.rand.NextBool(4))
			{
				return "I wonder if " + Main.npc[traveller].GivenName + "'s wares are actually worth anything this time. I should probably go check...";
			}
			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0 && Main.rand.NextBool(4))
			{
				return "I know it may sound a bit rude, but you really shouldn't waste your time with " + Main.npc[merchant].GivenName + "'s wares.";
			}
			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && Main.rand.NextBool(4)) {
				return "That " + Main.npc[partyGirl].GivenName + " reminds me of a cat faunus who came into my store once... by the brothers, she was loud...";
			} 
			switch (Main.rand.Next(4)) {
				case 0:
					return "I hear Argus is lovely this time of year.";
				case 1:
					return "No, I don't have any hard-light Dust. Stop asking.";
				case 2:
					return "If your Dust explodes, you can complain to the SDC. Just keep me out of it!";
				default:
					return "There... there aren't any robbers in " + Main.worldName + "... right?";
			}
		}


		public override void SetChatButtons(ref string button, ref string button2) {
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop) {
			if (firstButton) {
				shop = true;
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot) {
			shop.item[nextSlot].SetDefaults(ItemType<Items.Placeable.DustToolbench>());
			nextSlot++;
			if (NPC.downedGolemBoss) {
				shop.item[nextSlot].SetDefaults(ItemType<Items.Materials.DustWeaponKit>());
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemType<Items.Materials.FireDustExtract>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Items.Materials.PlantDustExtract>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Items.Materials.GravityDustExtract>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Items.Materials.IceDustExtract>());
			nextSlot++;
		}

		public override bool CanGoToStatue(bool toKingStatue) {
			return toKingStatue;
		}

		public void StatueTeleport() {
			for (int i = 0; i < 30; i++) {
				Vector2 position = Main.rand.NextVector2Square(-20, 21);
				if (Math.Abs(position.X) > Math.Abs(position.Y)) {
					position.X = Math.Sign(position.X) * 20;
				}
				else {
					position.Y = Math.Sign(position.Y) * 20;
				}
			}
		}

        public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
        {
			scale = 0.75f;
			item = ItemID.Handgun;
		}

        public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ProjectileType<FireBullet>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 2f;
		}
	}
}
