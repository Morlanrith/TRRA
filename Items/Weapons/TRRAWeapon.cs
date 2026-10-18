using System;
using Terraria.ModLoader;

namespace TRRA.Items.Weapons
{
    public abstract class TRRAWeapon(uint maxRank = 1) : ModItem
    {
        public readonly uint MaxRank = maxRank;
        private uint _currentRank = 1;
        public uint CurrentRank { get => _currentRank; }

        public double GetRankRatio()
        {
            return (double)_currentRank / MaxRank;
        }

        public void IncreaseRank()
        {
            SetRank(_currentRank + 1);
        }

        public void SetRank(uint newRank)
        {
            if (newRank == 0)
            {
                throw new ArgumentNullException(nameof(newRank));
            }
            else if (newRank > MaxRank)
            {
                _currentRank = MaxRank;
            }
            else
            {
                _currentRank = newRank;
            }
        }

		public virtual void ApplyVisualEffects(Terraria.Player player)
        {}
    }
}