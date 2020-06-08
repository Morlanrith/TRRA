using Terraria.ModLoader;

namespace TRRA
{
    public class TRRA : Mod
    {
        public static ModHotKey TransformHotKey;

        public TRRA()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true

            };
        }

        public override void Load()
        {
            // Registers hotkeys
            TransformHotKey = RegisterHotKey("Transform Weapon", "F");

            base.Load();
        }

        public override void Unload()
        {
            // Unload hotkeys
            TransformHotKey = null;

            base.Unload();
        }
    }
}