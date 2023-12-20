using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.Audio;
using TRRA.Items.Weapons;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Terraria.DataStructures;
using TRRA.Projectiles.Item.Weapon.CrescentRose;

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

        private static readonly SoundStyle HarbingerScTransformSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Harbinger/HarbingerScytheTransform")
        {
            Volume = 0.7f,
            Pitch = 0.0f,
        };

        private static readonly SoundStyle HarbingerSwTransformSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/Harbinger/HarbingerSwordTransform")
        {
            Volume = 0.7f,
            Pitch = -0.2f,
        };

        private static readonly SoundStyle WhiteRoseTransformSound = new($"{nameof(TRRA)}/Sounds/Item/Weapon/SunderedRose/WhiteRoseTransform")
        {
            Volume = 0.5f,
            Pitch = 0.0f,
        };

        private ModKeybind altUseHotkey = null;
        private readonly List<Projectile> blades = new();
        // Immediately gets instances of all TRRA weapons and weapon types (used to prevent instance issues)
        private static readonly Item 
            gun = GetModItem(ItemType<CrescentRoseG>()).Item,
            scythe = GetModItem(ItemType<CrescentRoseS>()).Item,
            gunJr = GetModItem(ItemType<CrescentBloomG>()).Item,
            scytheJr = GetModItem(ItemType<CrescentBloomS>()).Item,
            rapier = GetModItem(ItemType<Myrtenaster>()).Item,
            rapierF = GetModItem(ItemType<MyrtenasterF>()).Item,
            rapierJr = GetModItem(ItemType<Silbernelke>()).Item,
            rapierFJr = GetModItem(ItemType<SilbernelkeF>()).Item,
            katana = GetModItem(ItemType<GambolShroudS>()).Item,
            gunkata = GetModItem(ItemType<GambolShroudG>()).Item,
            katanaJr = GetModItem(ItemType<GambolShadeS>()).Item,
            gunkataJr = GetModItem(ItemType<GambolShadeG>()).Item,
            fist = GetModItem(ItemType<EmberCelicaS>()).Item,
            rocket = GetModItem(ItemType<EmberCelicaR>()).Item,
            oldSword = GetModItem(ItemType<HarbingerSw>()).Item,
            oldScythe = GetModItem(ItemType<HarbingerSc>()).Item,
            axe = GetModItem(ItemType<SunderedRoseA>()).Item,
            axeGun = GetModItem(ItemType<SunderedRoseG>()).Item;

        public override void PostUpdate()
        {
            EmberFists();
            if(Player.sleeping.isSleeping && TRRAWorld.IsShatteredMoon())
                Player.sleeping.timeSleeping = 0;
        }

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (Player.HasBuff<PetalBurstBuff>()) return true;
            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            // Transform Weapon
            if(TRRA.GetTransformHotKey().JustPressed && Player.altFunctionUse != 2 && Player.itemAnimation == 0)
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
                    case "Crescent Bloom":
                        SoundEngine.PlaySound(RoseTransformSound); // Plays the transform sound effect for Crescent Rose
                        if (heldItem.type.Equals(scytheJr.type)) // If the current held Crescent Bloom is in Scythe form, swaps to gun
                            chosenItem = gunJr;
                        else // Otherwise, swaps to scythe
                            chosenItem = scytheJr;
                        break;
                    case "Myrtenaster":
                        SoundEngine.PlaySound(DustSpinSound); // Plays the transform sound effect for Myrtenaster
                        if (heldItem.type.Equals(rapier.type)) // If the current held Myrtenaster is in base form, swaps to fire
                            chosenItem = rapierF;
                        else // Otherwise, swaps to base
                            chosenItem = rapier;
                        break;
                    case "Silbernelke":
                        SoundEngine.PlaySound(DustSpinSound); // Plays the transform sound effect for Myrtenaster
                        if (heldItem.type.Equals(rapierJr.type)) // If the current held Silbernelke is in base form, swaps to fire
                            chosenItem = rapierFJr;
                        else // Otherwise, swaps to base
                            chosenItem = rapierJr;
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
                    case "Gambol Shade":
                        if (heldItem.type.Equals(katanaJr.type)) // If the current held Gambol Shade is in katana form, swaps to gun
                        {
                            SoundEngine.PlaySound(GambolCockSound); // Plays the relevant transform sound effect
                            chosenItem = gunkataJr;
                        }
                        else // Otherwise, swaps to katana
                        {
                            SoundEngine.PlaySound(GambolTransformSound); // Plays the relevant transform sound effect
                            chosenItem = katanaJr;
                        }
                        break;
                    case "Ember Celica":
                        SoundEngine.PlaySound(EmberTransformSound); // Plays the transform sound effect for the Ember Celica
                        if (heldItem.type.Equals(fist.type)) // If the current held Ember Celica is in shotgun form, swaps to rocket
                            chosenItem = rocket;
                        else // Otherwise, swaps to shotgun
                            chosenItem = fist;
                        break;
                    case "Harbinger":
                        if (heldItem.type.Equals(oldSword.type)) // If the current held Harbinger is in sword form, swaps to scythe
                        {
                            SoundEngine.PlaySound(HarbingerScTransformSound); // Plays the relevant transform sound effect
                            chosenItem = oldScythe;
                        }
                        else // Otherwise, swaps to sword
                        {
                            SoundEngine.PlaySound(HarbingerSwTransformSound); // Plays the relevant transform sound effect
                            chosenItem = oldSword;
                        }
                        break;
                    case "Sundered Rose":
                        SoundEngine.PlaySound(WhiteRoseTransformSound); // Plays the transform sound effect for Sundered Rose
                        if (heldItem.type.Equals(axe.type)) // If the current held Sundered Rose is in Axe form, swaps to gun
                            chosenItem = axeGun;
                        else // Otherwise, swaps to axe
                            chosenItem = axe;
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
            if (Player.HeldItem.Name == "Ember Celica" || Player.HeldItem.Name == "Steel Celica" || Player.HeldItem.Name == "Spark Celica" || Player.HeldItem.Name == "Fire Fists")
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
