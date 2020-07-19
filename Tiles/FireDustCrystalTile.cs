using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace TRRA.Tiles
{
	public class FireDustCrystalTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = false;
			Main.tileSolidTop[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;


			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.RandomStyleRange = 6;

			// Allow hanging from ceilings
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { 124 };
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorRight = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(0);

			// Allow attaching to a solid object that is to the left of the sign
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { 124 };
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide , TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(0);

			// Allow attaching to a solid object that is to the right of the sign
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { 124 };
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide , TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(0);

			// Allow attaching sign to the ground
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[] { 124 };
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.addAlternate(0);

			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Fire Dust Crystal");
			AddMapEntry(new Color(255, 77, 26), name);
			disableSmartCursor = true;
			adjTiles = new int[] { Type };
		}

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			Tile tile = Main.tile[i, j];
			Tile tile8 = Main.tile[i, j - 1];
			Tile tile9 = Main.tile[i, j + 1];
			Tile tile2 = Main.tile[i - 1, j];
			Tile tile3 = Main.tile[i + 1, j];
			int num23 = -1;
			int num24 = -1;
			int num25 = -1;
			int num26 = -1;
			if (tile8 != null && tile8.nactive() && !tile8.bottomSlope())
			{
				num24 = tile8.type;
			}
			if (tile9 != null && tile9.nactive() && !tile9.halfBrick() && !tile9.topSlope())
			{
				num23 = tile9.type;
			}
			if (tile2 != null && tile2.nactive())
			{
				num25 = tile2.type;
			}
			if (tile3 != null && tile3.nactive())
			{
				num26 = tile3.type;
			}
			if (num23 >= 0 && Main.tileSolid[num23] && !Main.tileSolidTop[num23])
			{
				tile.frameY = 0;
			}
			else if (num24 >= 0 && Main.tileSolid[num24] && !Main.tileSolidTop[num24])
			{
				tile.frameY = 18;
			}
			else if (num25 >= 0 && Main.tileSolid[num25] && !Main.tileSolidTop[num25])
			{
				tile.frameY = 54;
			}
			else if (num26 >= 0 && Main.tileSolid[num26] && !Main.tileSolidTop[num26])
			{
				tile.frameY = 36;
			}
			return base.TileFrame(i, j, ref resetFrame, ref noBreak);
        }

        public override bool KillSound(int i, int j)
        {
			return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 1.0f;
			g = 0.3f;
			b = 0.1f;
		}

        public override bool Drop(int i, int j)
		{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemType<Items.Materials.FireDustCrystal>());
			Main.PlaySound(SoundID.Item27, i*16,j*16);
			return base.Drop(i, j);
		}
	}
}