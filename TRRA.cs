using Terraria.ModLoader;
using TRRA.Effects;
using Terraria.Graphics.Effects;

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

            // Setup filter for Shattered Moon
            Filters.Scene["ShatteredMoon"] = new Filter(new ShatteredMoonShader("FilterBloodMoon").UseColor(1f, 0f, 0.5f), EffectPriority.Medium);

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