using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace TRRA.Sounds.Item.Weapon.EmberCelica
{
	public class EmberDash : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type) {
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .3f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = -.1f;
			return soundInstance;
		}
	}
}
