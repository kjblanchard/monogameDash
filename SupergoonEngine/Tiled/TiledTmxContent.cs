using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace SupergoonEngine.Tiled;

public class TiledTmxContent
{
    #region Configuration

    #endregion

    public TiledMap TileMap;
    public TiledTileset[] Tilesets;
    public Texture2D[] TilesetTextures;

    public TiledTmxContent(TiledMap map, TiledTileset[] tilesets, Texture2D[] textures)
    {
        TileMap = map;
        Tilesets = tilesets;
        TilesetTextures = textures;
    }

    public void DrawTilemap(SpriteBatch spriteBatch)
    {
        //For each layer in the tilemap
        for (int i = 0; i < TileMap.Layers.Length; i++)
        {
            var layer = TileMap.Layers[i];
            //Draw the tile at the correct location
            for (int j = 0; j < layer.data.Length; j++)
            {
                int gid = layer.data[j];
                //If gid is 0, this is an empty tiled tile, so skip
                if (gid == 0)
                    continue;

                //Determine which tileset we should use for this tile (for when using multiple tilesets in the layer.
                var drawTilesetNum = -1;
                var it = 0;
                while (drawTilesetNum == -1)
                {
                    //If There is not more tilesets, use the current one as it must be this one.
                    if (it + 1 >= Tilesets.Length)
                    {
                        drawTilesetNum = it;
                        break;
                    }

                    var firstTilesetGid = TileMap.Tilesets[it].firstgid;
                    if (gid >= firstTilesetGid && gid < TileMap.Tilesets[it + 1].firstgid)
                    {
                        drawTilesetNum = it;
                        break;
                    }
                    it++;
                }
                //We need to subtract The amount of tiles prior to this.
                var tileFrame = gid - TileMap.Tilesets[drawTilesetNum].firstgid;

                var currentTile = TileMap.GetTiledTile(TileMap.Tilesets[drawTilesetNum], Tilesets[drawTilesetNum], gid);
                
                int column = tileFrame % Tilesets[drawTilesetNum].Columns;
                int row = (int)Math.Floor((double)tileFrame / (double)Tilesets[drawTilesetNum].Columns);
                float x = (j % TileMap.Width) * TileMap.TileWidth;
                float y = (float)Math.Floor(j / (double)TileMap.Width) * TileMap.TileHeight;

                Rectangle tilesetRec = new Rectangle(Tilesets[drawTilesetNum].TileWidth * column,
                    Tilesets[drawTilesetNum].TileHeight * row, Tilesets[drawTilesetNum].TileWidth,
                    Tilesets[drawTilesetNum].TileHeight);

                spriteBatch.Draw(TilesetTextures[drawTilesetNum],
                    new Rectangle((int)x, (int)y, Tilesets[drawTilesetNum].TileWidth,
                        Tilesets[drawTilesetNum].TileHeight), tilesetRec, Color.White);
            }
        }
    }


    #region Methods

    #endregion
}