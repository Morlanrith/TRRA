using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.Audio;
using TRRA.Items.Weapons;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;

namespace TRRA
{
    public class TRRAPlayer : ModPlayer
	{
        private static readonly SoundStyle RoseTransformSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/CrescentRose/RoseTransform")
        {
            Volume = 0.5f,
            Pitch = 0.0f,
        };

        private static readonly SoundStyle DustSpinSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Myrtenaster/DustSpin")
        {
            Volume = 0.8f,
            Pitch = 0.0f,
        };

        private static readonly SoundStyle GambolCockSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/GambolShroud/GambolCock")
        {
            Volume = 0.5f,
            Pitch = 0.0f,
        };

        private static readonly SoundStyle GambolTransformSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/GambolShroud/GambolTransform")
        {
            Volume = 0.5f,
            Pitch = 0.0f,
        };

        private static readonly SoundStyle EmberTransformSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/EmberCelica/EmberTransform")
        {
            Volume = 0.5f,
            Pitch = 0.0f,
        };

        private ModKeybind altUseHotkey = null;
        private List<Projectile> blades = new List<Projectile>();
        // Immediately gets instances of all TRRA weapons and weapon types (used to prevent instance issues)
        private static readonly Item 
            gun = GetModItem(ItemType<CrescentRoseG>()).Item,
            scythe = GetModItem(ItemType<CrescentRoseS>()).Item,
            rapier = GetModItem(ItemType<Myrtenaster>()).Item,
            rapierF = GetModItem(ItemType<MyrtenasterF>()).Item,
            katana = GetModItem(ItemType<GambolShroudS>()).Item,
            gunkata = GetModItem(ItemType<GambolShroudG>()).Item,
            fist = GetModItem(ItemType<EmberCelicaS>()).Item,
            rocket = GetModItem(ItemType<EmberCelicaR>()).Item;

        public override void PostUpdate()
        {
            EmberFists();
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            // Transform Weapon
            if(TRRA.TransformHotKey.JustPressed && Player.altFunctionUse != 2)
            {
                Item heldItem = Player.inventory[Player.selectedItem]; // Obtains the current held item from the players inventory
                Item chosenItem = null;
                switch (heldItem.Name) // Determines action based on the name of the item
                {
                    case "Crescent Rose":
                        SoundEngine.PlaySound(RoseTransformSound); // Plays the transform sound effect for Crescent Rose
                        if (heldItem.type.Equals(scythe.type)) // If the current held Crescent Rose is in Scythe form, swaps to gun
                            chosenItem = gun;
                        else // Otherwise, swaps to scythe
                            chosenItem = scythe;
                        break;
                    case "Myrtenaster":
                        SoundEngine.PlaySound(DustSpinSound); // Plays the transform sound effect for Myrtenaster
                        if (heldItem.type.Equals(rapier.type)) // If the current held Myrtenaster is in base form, swaps to fire
                            chosenItem = rapierF;
                        else // Otherwise, swaps to base
                            chosenItem = rapier;
                        break;
                    case "Gambol Shroud":
                        if(heldItem.type.Equals(katana.type)) // If the current held Gambol Shroud is in katana form, swaps to gun
                        {
                            SoundEngine.PlaySound(GambolCockSound); // Plays the relevant transform sound effect
                            chosenItem = gunkata;
                        }
                        else // Otherwise, swaps to katana
                        {
                            SoundEngine.PlaySound(GambolTransformSound); // Plays the relevant transform sound effect
                            chosenItem = katana;
                        }
                        break;
                    case "Ember Celica":
                        SoundEngine.PlaySound(EmberTransformSound); // Plays the transform sound effect for the Ember Celica
                        if (heldItem.type.Equals(fist.type)) // If the current held Ember Celica is in shotgun form, swaps to rocket
                            chosenItem = rocket;
                        else // Otherwise, swaps to shotgun
                            chosenItem = fist;
                        break;
                }
                if(chosenItem != null)
                {
                    Player.inventory[Player.selectedItem] = chosenItem.Clone();
                    Player.inventory[Player.selectedItem].SetDefaults(chosenItem.type);
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
            if (Player.HeldItem.Name == "Ember Celica")
            {
                if (Player.HeldItem.handOnSlot > 0)
                {
                    Player.handon = Player.HeldItem.handOnSlot;
                    Player.cHandOn = 0;
                }
                if (Player.HeldItem.handOffSlot > 0)
                {
                    Player.handoff = Player.HeldItem.handOffSlot;
                    Player.cHandOff = 0;
                }
            }
        }

        public void AddBlade(Projectile projectile)
        {
            blades.Add(projectile);
        }

        public void RemoveBlade(Projectile projectile)
        {
            blades.Remove(projectile);
        }

        public int KillBlades()
        {
            int currentAmount = blades.Count;
            for(int i=blades.Count-1; i >= 0; i--)
                blades[i].Kill();
            return currentAmount;
        }

    }
}
