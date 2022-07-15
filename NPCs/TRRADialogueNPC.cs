using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRRA.NPCs
{
	public class TRRADialogueNPC : GlobalNPC
	{
        public override void GetChat(NPC npc, ref string chat)
        {
            switch (npc.type)
            {
                case NPCID.Guide:
                    chat = "I believe these creatures are called 'Grimm', and from what I've heard, negative emotion makes them stronger. So stay positive!";
                    return;
                case NPCID.Merchant:
                    chat = "Hey can you clear out these weird creatures? I don't know what they are, but they are assuredly NOT good for business.";
                    return;
                case NPCID.Nurse:
                    chat = "Oh don't worry, I can definitely treat wounds inflicted by Grimm... I think.";
                    return;
                case NPCID.Demolitionist:
                    chat = "Ha! These Grimm blow up real nice!";
                    return;
                case NPCID.DyeTrader:
                    chat = "These Grimm have no appreciation for style or color, how dull.";
                    return;
                case NPCID.Angler:
                    chat = "Hey! What did these weirdos do to the water?! Fix it!";
                    return;
                case NPCID.BestiaryGirl:
                    chat = "Bro... those wolves look nuts dude.";
                    return;
                case NPCID.Dryad:
                    chat = "These creatures... there's something very wrong about them...";
                    return;
                case NPCID.Painter:
                    chat = "No i didn't mix paints in the water. I don't know what's done THAT.";
                    return;
                case NPCID.Golfer:
                    chat = "Ah come one! These Grimm are ruining the course!";
                    return;
                case NPCID.ArmsDealer:
                    chat = "These Grimm might not be from around here, but my wares'll still work just as fine.";
                    return;
                case NPCID.DD2Bartender:
                    chat = "I've never seen the like of these creatures before.";
                    return;
                case NPCID.Stylist:
                    chat = "A haircut? But you're not due a new look for at least 2 more volumes.";
                    return;
                case NPCID.GoblinTinkerer:
                    chat = "A gadget to deal with these things? I'll think about.";
                    return;
                case NPCID.WitchDoctor:
                    chat = "The Grimm embody negative emotion. Such interesting power.";
                    return;
                case NPCID.Clothier:
                    chat = "A Grimm-fur coat? Given they seem to evaporate on death, I don't think that'll work.";
                    return;
                case NPCID.Mechanic:
                    chat = "Maybe I could make some crazy half-gun, half-sword thing... Nah, that's just stupid.";
                    return;
                case NPCID.PartyGirl:
                    chat = "WOOOO! Otherworld invasion, let's go!";
                    return;
                case NPCID.Wizard:
                    chat = "Maidens? Never heard of them.";
                    return;
                case NPCID.TaxCollector:
                    chat = "Grimm or no, everyones money will be collected on time! Don't you worry!";
                    return;
                case NPCID.Truffle:
                    chat = "I believe the Grimm can't reach the underground, maybe it's safer back home...?";
                    return;
                case NPCID.Pirate:
                    chat = "No critters like these on any seas I've sailed!";
                    return;
                case NPCID.Steampunker:
                    chat = "I'm tellin ya, we build a sky city to escape the Grimm!";
                    return;
                case NPCID.Cyborg:
                    chat = "I have identified multiple creatures of 'Grimm'. I advise lethal force.";
                    return;
                case NPCID.SantaClaus:
                    chat = "I think I remember fighting creatures like these with a giant sword one time...";
                    return;
                case NPCID.Princess:
                    chat = "I'm sure if we spoke with the leader of these 'Grimm' we could resolve this all peacefully.";
                    return;
                default:
                    return;
            }
        }

    }
}
