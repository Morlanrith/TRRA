using Terraria;
using Terraria.ModLoader;

namespace TRRA.Effects
{
	public class ShatteredMoonFX : ModSceneEffect
	{
        public override int Music => 79;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("TRRA/GrimmWaterStyle");

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("ShatteredMoon", TRRAWorld.IsShatteredMoon());
        }

        public override bool IsSceneEffectActive(Player player)
        {
            return TRRAWorld.IsShatteredMoon() && player.position.Y < Main.worldSurface * 16.0;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
    }
}