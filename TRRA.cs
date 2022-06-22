using Terraria.ModLoader;

namespace TRRA
{
    public class TRRA : Mod
    {
        public static ModKeybind TransformHotKey;

        public TRRA()
        {
        }

        public override void Load()
        {
            // Registers hotkeys
            TransformHotKey = KeybindLoader.RegisterKeybind(this,"Transform Weapon", "F");

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