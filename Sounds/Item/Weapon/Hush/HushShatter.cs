using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace TRRA.Sounds.Item.Weapon.Hush
{
	public class HushShatter : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type) {
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .4f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = 0f;
			return soundInstance;
		}
	}
}
