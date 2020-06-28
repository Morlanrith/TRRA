using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace TRRA.Sounds.Item
{
	public class EmberShot : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type) {
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .3f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = 0f;
			return soundInstance;
		}
	}
}
