using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using TRRA.Items.Weapons;
using static Terraria.ModLoader.ModContent;

namespace TRRA
{
    public class TRRAPlayer : ModPlayer
	{

        private ModHotKey altUseHotkey = null;
        // Immediately gets instances of all TRRA weapons and weapon types (used to prevent instance issues)
        private readonly Item gun = GetModItem(ItemType<CrescentRoseG>()).item;
        private readonly int gunType = ItemType<CrescentRoseG>();
        private readonly Item scythe = GetModItem(ItemType<CrescentRoseS>()).item;
        private readonly int scytheType = ItemType<CrescentRoseS>();
        private readonly Item rapier = GetModItem(ItemType<Myrtenaster>()).item;
        private readonly int rapierType = ItemType<Myrtenaster>();
        private readonly Item rapierF = GetModItem(ItemType<MyrtenasterF>()).item;
        private readonly int rapierFType = ItemType<MyrtenasterF>();
        private readonly Item katana = GetModItem(ItemType<GambolShroudS>()).item;
        private readonly int katanaType = ItemType<GambolShroudS>();
        private readonly Item gunkata = GetModItem(ItemType<GambolShroudG>()).item;
        private readonly int gunkataType = ItemType<GambolShroudG>();
        private readonly Item fist = GetModItem(ItemType<EmberCelicaS>()).item;
        private readonly int fistType = ItemType<EmberCelicaS>();
        private readonly Item rocket = GetModItem(ItemType<EmberCelicaR>()).item;
        private readonly int rocketType = ItemType<EmberCelicaR>();

        public override void PostUpdate()
        {
            EmberFists();
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            // Transform Weapon
            if(TRRA.TransformHotKey.JustPressed)
            {
                // Obtains the current held item from the players inventory
                Item heldItem = player.inventory[player.selectedItem];
                // Checks if the held item is one of two Crescent Rose variants
                if (heldItem.Name == "Crescent Rose")
                {
                    // Plays the transform sound effect for Crescent Rose
                    Main.PlaySound(SoundID.Item, -1, -1, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/Weapon/CrescentRose/RoseTransform"));
                    // If the current held Crescent Rose is in Scythe form, swaps to gun, and vice versa
                    if (heldItem.type.Equals(scytheType))
                    {
                        player.inventory[player.selectedItem] = gun;
                        player.inventory[player.selectedItem].SetDefaults(gunType);
                    }
                    if (heldItem.type.Equals(gunType))
                    {
                        player.inventory[player.selectedItem] = scythe;
                        player.inventory[player.selectedItem].SetDefaults(scytheType);
                    }
                }
                else if (heldItem.Name == "Myrtenaster" || heldItem.Name == "Myrtenaster (Fire)")
                {
                    // Plays the transform sound effect for Myrtenaster
                    Main.PlaySound(SoundID.Item, -1, -1, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/Weapon/Myrtenaster/DustSpin"));
                    // If the current held Myrtenaster is in base form, swaps to fire, and vice versa
                    if (heldItem.type.Equals(rapierType))
                    {
                        player.inventory[player.selectedItem] = rapierF;
                        player.inventory[player.selectedItem].SetDefaults(rapierFType);
                    }
                    if (heldItem.type.Equals(rapierFType))
                    {
                        player.inventory[player.selectedItem] = rapier;
                        player.inventory[player.selectedItem].SetDefaults(rapierType);
                    }
                }
                // If the current held item is Gambol Shroud, swaps its form between sword and gun
                else if (heldItem.type.Equals(katanaType))
                {
                    Main.PlaySound(SoundID.Item, -1, -1, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/Weapon/GambolShroud/GambolCock"));
                    player.inventory[player.selectedItem] = gunkata;
                    player.inventory[player.selectedItem].SetDefaults(gunkataType);
                }
                else if (heldItem.type.Equals(gunkataType))
                {
                    Main.PlaySound(SoundID.Item, -1, -1, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/Weapon/GambolShroud/GambolTransform"));
                    player.inventory[player.selectedItem] = katana;
                    player.inventory[player.selectedItem].SetDefaults(katanaType);
                }
                else if (heldItem.Name == "Ember Celica" || heldItem.Name == "Ember Celica (Rocket)")
                {
                    // Plays the transform sound effect for the Ember Celica
                    Main.PlaySound(SoundID.Item, -1, -1, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/Weapon/EmberCelica/EmberTransform"));
                    // If the current held Ember Celica is in shotgun form, swaps to rocket, and vice versa
                    if (heldItem.type.Equals(fistType))
                    {
                        player.inventory[player.selectedItem] = rocket;
                        player.inventory[player.selectedItem].SetDefaults(rocketType);
                    }
                    if (heldItem.type.Equals(rocketType))
                    {
                        player.inventory[player.selectedItem] = fist;
                        player.inventory[player.selectedItem].SetDefaults(fistType);
                    }
                }
            }

            // Prevents the transform hotkey from being repeatedly activated whilst holding the key down
            if (altUseHotkey != null && altUseHotkey.JustReleased)
            {
                altUseHotkey = null;
            }

            base.ProcessTriggers(triggersSet);
        }

        private void EmberFists()
        {
            if (player.HeldItem.type == fistType || player.HeldItem.type == rocketType)
            {
                if (player.HeldItem.handOnSlot > 0)
                {
                    player.handon = player.HeldItem.handOnSlot;
                    player.cHandOn = 0;
                }
                if (player.HeldItem.handOffSlot > 0)
                {
                    player.handoff = player.HeldItem.handOffSlot;
                    player.cHandOff = 0;
                }
            }
        }

    }
}
