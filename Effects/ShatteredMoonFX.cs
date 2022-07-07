using Terraria;
using Terraria.ModLoader;

namespace TRRA.Effects
{
	public class ShatteredMoonFX : ModSceneEffect
	{
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("ShatteredMoon", TRRAWorld.IsShatteredMoon());
        }
    }
}