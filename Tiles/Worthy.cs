using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TRRA.Tiles
{
    public class Worthy : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(242,0));
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(105, 50, 35), Language.GetText("MapObject.Painting"));
			DustType = 7;
		}
	}
}
