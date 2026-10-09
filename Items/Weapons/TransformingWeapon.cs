using Terraria.ModLoader;
using Terraria.Audio;

namespace TRRA.Items.Weapons
{
    public abstract class TransformingWeapon(ModItem swapItem, SoundStyle transformSound) : TRRAWeapon
    {
		private readonly ModItem _swapItem = swapItem;
        private readonly SoundStyle _transformSound = transformSound;

        public ModItem TransformWeapon()
        {
            SoundEngine.PlaySound(_transformSound);
            return _swapItem;
        }
    }
}