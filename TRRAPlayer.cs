using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;
using TRRA.Projectiles.Item.Weapon.CrescentRose;
using TRRA.Items.Weapons;
using TRRA.Items.Weapons.CrescentRose;
using System;

namespace TRRA
{
    public class TRRAPlayer : ModPlayer
	{
        private readonly List<Projectile> _arrayBlades = [];

        public override void PostUpdate()
        {
            if(Player.HeldItem.ModItem != null && Player.HeldItem.ModItem is TRRAWeapon heldWeapon)
                heldWeapon.ApplyVisualEffects(Player);

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
            if (TRRA.GetTransformHotKey().JustPressed &&
                Player.altFunctionUse != 2 &&
                Player.itemAnimation == 0 &&
                Player.HeldItem.ModItem != null &&
                Player.HeldItem.ModItem is TransformingWeapon transfItem)
            {
                uint currentRank = transfItem.CurrentRank;
                Item chosenItem = transfItem.TransformWeapon().Item;
                Player.inventory[Player.selectedItem] = chosenItem.Clone();
                Player.inventory[Player.selectedItem].SetDefaults(chosenItem.type);
                if (Player.HeldItem.ModItem != null && Player.inventory[Player.selectedItem].ModItem is TRRAWeapon weaponItem)
                {
                    weaponItem.SetRank(currentRank);
                }
            }
            else if (TRRA.GetTransformHotKey().JustPressed &&
                Player.altFunctionUse != 2 &&
                Player.itemAnimation == 0 &&
                Player.HeldItem.ModItem != null &&
                Player.HeldItem.ModItem is CrescentBud cBud) // Test functionality - should be removed eventually
            {
                if (!cBud.IsMaxRank())
                {
                    cBud.IncreaseRank();
                    Main.NewText($"Increased rank to {cBud.CurrentRank}");
                }
                else {
                    cBud.SetRank(1);
                    Main.NewText($"Reset rank to {cBud.CurrentRank}");
                }
            }

            base.ProcessTriggers(triggersSet);
        }

        public void AddBlade(Projectile projectile)
        {
            _arrayBlades.Add(projectile);
        }

        public void RemoveBlade(Projectile projectile)
        {
            _arrayBlades.Remove(projectile);
        }

        public int KillBlades()
        {
            int currentAmount = _arrayBlades.Count;
            for(int i= _arrayBlades.Count-1; i >= 0; i--)
                _arrayBlades[i].Kill();
            return currentAmount;
        }
    }
}
