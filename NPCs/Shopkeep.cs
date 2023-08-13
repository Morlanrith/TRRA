using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
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

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 26;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 5;
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;
			NPCID.Sets.AttackType[NPC.type] = 1;
			NPCID.Sets.AttackTime[NPC.type] = 90;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Biomes
			NPC.Happiness.SetBiomeAffection<ForestBiome>(Terraria.GameContent.Personalities.AffectionLevel.Like); // Shopkeep prefers the forest.
			NPC.Happiness.SetBiomeAffection<SnowBiome>(Terraria.GameContent.Personalities.AffectionLevel.Like); // Shopkeep prefers the forest.
			NPC.Happiness.SetBiomeAffection<UndergroundBiome>(Terraria.GameContent.Personalities.AffectionLevel.Dislike); // Shopkeep dislikes the underground.
			// NPCs
			NPC.Happiness.SetNPCAffection(NPCID.Demolitionist,Terraria.GameContent.Personalities.AffectionLevel.Hate); // Hates living near the demolitionist.
			NPC.Happiness.SetNPCAffection(NPCID.Merchant, Terraria.GameContent.Personalities.AffectionLevel.Dislike);// Dislikes living near the merchant.
			NPC.Happiness.SetNPCAffection(NPCID.PartyGirl, Terraria.GameContent.Personalities.AffectionLevel.Dislike); // Dislikes living near the party girl.
			NPC.Happiness.SetNPCAffection(NPCID.Mechanic, Terraria.GameContent.Personalities.AffectionLevel.Like);// Likes living near the mechanic.
			NPC.Happiness.SetNPCAffection(NPCID.Cyborg, Terraria.GameContent.Personalities.AffectionLevel.Love); // Loves living near the cyborg.
		}

		public override void SetDefaults() {
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 42;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,

				new FlavorTextBestiaryInfoElement("Mods.TRRA.Bestiary.Shopkeep"),

			});
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Shopkeep_Head").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Shopkeep_Arm").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Shopkeep_Leg").Type);
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }
                foreach (Item item in player.inventory)
                {
                    if (item.type == ItemType<Items.Materials.FireDustCrystal>() || item.type == ItemType<Items.Materials.PlantDustCrystal>() || item.type == ItemType<Items.Materials.GravityDustCrystal>() || item.type == ItemType<Items.Materials.IceDustCrystal>() || (item.ModItem?.Mod == Mod && item.damage > 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
				"Kerry",
				"Miles",
				"Eddy",
				"Chris",
				"Cole",
				"Matt",
				"Brent",
				"Jeff"
			};
		}

		public override string GetChat() {
			if (TRRAWorld.IsShatteredMoon()) return "There are Grimm here too?!";
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
            return Main.rand.Next(4) switch
            {
                0 => "I hear Argus is lovely this time of year.",
                1 => "No, I don't have any hard-light Dust. Stop asking.",
                2 => "If your Dust explodes, you can complain to the SDC. Just keep me out of it!",
                _ => "There... there aren't any robbers in " + Main.worldName + "... right?",
            };
        }

		public override void SetChatButtons(ref string button, ref string button2) {
			button = Language.GetTextValue("LegacyInterface.28");
		}

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            shopName = "Shop";
        }

        public override void AddShops()
        {
			var npcShop = new NPCShop(Type, "Shop")
			.Add<Items.Placeable.DustToolbench>()
			.Add<Items.Materials.DustWeaponKit>();

            npcShop.Register(); // Name of this shop tab
        }

        public override bool CanGoToStatue(bool toKingStatue) {
			return toKingStatue;
		}

		public static void StatueTeleport() {
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

        public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
        {
            scale = 0.75f;
            Main.GetItemDrawFrame(ItemID.Handgun, out item, out itemFrame);
			horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1f, ItemID.Handgun).X - 0;
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
