using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.GameObjects;
using TiledCS;

namespace SupergoonEngine.Tiled;

public class TiledTmxContent
{
    #region Configuration

    #endregion

    public TiledMap TileMap;
    public TiledTileset[] Tilesets;
    public Texture2D[] TilesetTextures;

    public List<Tile> BackgroundTiles = new();
    public List<SolidTile> SolidTiles = new();

    public List<GameObject> Actors = new();


    /// <summary>
    /// This is used so that we can multiply the actual layer by this amount to use for the draw order in xna
    /// </summary>
    private const float _drawOrderMultiplier = 0.001f;

    public TiledTmxContent(TiledMap map, TiledTileset[] tilesets, Texture2D[] textures)
    {
        TileMap = map;
        Tilesets = tilesets;
        TilesetTextures = textures;
    }

    public void Reset()
    {
        Actors.Clear();
        SpawnActorsFromTilemap();
    }


    public void CreateTileGameObjectsFromContent()
    {
        var groups = TileMap.Groups;
        var bgGroup = groups.FirstOrDefault(group => group.name == "bg");
        //For each layer in the tilemap
        for (int layerIterator = 0; layerIterator < bgGroup.layers.Length; layerIterator++)
        {
            var layer = bgGroup.layers[layerIterator];
            //Draw the tile at the correct location
            for (int tileIterator = 0; tileIterator < layer.data.Length; tileIterator++)
            {
                int gid = layer.data[tileIterator];
                //If gid is 0, this is an empty tiled tile, so skip
                if (gid == 0)
                    continue;
                var drawTilesetNum = GetTilesetNumberFromTileGid(gid);
                var tileNumberInTileset = GetTileNumberFromTileGid(gid, drawTilesetNum);
                int column = GetTileColumn(tileNumberInTileset, drawTilesetNum);
                int row = GetTileRow(tileNumberInTileset, drawTilesetNum);
                float x = (tileIterator % TileMap.Width) * TileMap.TileWidth;
                float y = (float)Math.Floor(tileIterator / (double)TileMap.Width) * TileMap.TileHeight;

                Rectangle tilesetRec = GetTileRectangleInTexture(drawTilesetNum, column, row);
                var drawOrder = (layerIterator + 1) * _drawOrderMultiplier;


                if (layer.name.ToLower().StartsWith("solid"))
                {
                    var tileObjectData = GetTileObjectData(drawTilesetNum, tileNumberInTileset);
                    var boxSize = GetTileBoundingBoxSize(tileObjectData, drawTilesetNum);
                    var offset = GetTileBoundingBoxOffset(tileObjectData, drawTilesetNum);

                    var solidTile = new SolidTile(
                        new Vector2(x, y),
                        tilesetRec,
                        TilesetTextures[drawTilesetNum], drawOrder,
                        boxSize, offset
                    );
                    SolidTiles.Add(solidTile);
                }
                else
                {
                    var tile = new Tile(
                        new Vector2(x, y),
                        tilesetRec,
                        TilesetTextures[drawTilesetNum], drawOrder
                    );
                    BackgroundTiles.Add(tile);
                }
            }
        }

    }

    public void SpawnActorsFromTilemap()
    {
        //Create all of the actors (non tilemaps)
        var actorLayer = TileMap.Layers.FirstOrDefault(layer => layer.name.StartsWith("actor"));
        var actors = actorLayer.objects;
        for (int i = 0; i < actors.Length; i++)
        {
            var potentialActor = actors[i];
            var actorName = potentialActor.name;
            var exists = TiledActorFactory.NameToSpawnFunction.ContainsKey(actorName);
            if (!exists) continue;
            var gid = potentialActor.gid;
            var drawTileset = GetTilesetNumberFromTileGid(gid);
            var tileFrame = GetTileNumberFromTileGid(gid, drawTileset);
            int column = GetTileColumn(tileFrame, drawTileset);
            int row = GetTileRow(tileFrame, drawTileset);
            Rectangle tilesetRec = GetTileRectangleInTexture(drawTileset, column, row);
            var tileObjectData = GetTileObjectData(drawTileset, tileFrame);
            var boxSize = GetTileBoundingBoxSize(tileObjectData, drawTileset);
            var offset = GetTileBoundingBoxOffset(tileObjectData, drawTileset);

            var newActorFunc = TiledActorFactory.NameToSpawnFunction[actorName];
            var actorLocation = new Vector2(potentialActor.x, potentialActor.y - 32);
            var actorTags = potentialActor.properties;
            var actorParams = new ActorParams();
            actorParams.Location = actorLocation;
            actorParams.Tags = actorTags;
            actorParams.BoxSize = boxSize;
            actorParams.BoxColliderOffset = offset;
            actorParams.SourceRect = tilesetRec;
            actorParams.Texture = TilesetTextures[drawTileset];
            var newActor = newActorFunc.Invoke(actorParams);
            Actors.Add(newActor);
        }
    }

    private Point GetTileBoundingBoxSize(TiledObject tileObjectData, int drawTilesetNum)
    {
        if (tileObjectData == null)
        {
            var tileset = Tilesets[drawTilesetNum];
            return new Point(tileset.TileWidth, tileset.TileHeight);
        }

        return new Point((int)tileObjectData.width, (int)tileObjectData.height);
    }

    private Vector2 GetTileBoundingBoxOffset(TiledObject tileObjectData, int drawTilesetNum)
    {
        if (tileObjectData == null)
        {
            return new Vector2(0, 0);
        }

        return new Vector2(tileObjectData.x, tileObjectData.y);
    }

    //Gets the rectangle so that we can draw from the texture in the right location (in the spritesheet).
    private Rectangle GetTileRectangleInTexture(int drawTilesetNum, int column, int row)
    {
        return new Rectangle(Tilesets[drawTilesetNum].TileWidth * column,
            Tilesets[drawTilesetNum].TileHeight * row, Tilesets[drawTilesetNum].TileWidth,
            Tilesets[drawTilesetNum].TileHeight);
    }

    /// <summary>
    /// This tileobject holds a lot of metadata from the tile, if we have defined it in tiled, otherwise returns null
    /// </summary>
    /// <param name="drawTilesetNum"></param>
    /// <param name="tileNumberInTileset"></param>
    /// <returns></returns>
    private TiledObject GetTileObjectData(int drawTilesetNum, int tileNumberInTileset)
    {
        var tileDefinitions = Tilesets[drawTilesetNum].Tiles;
        TiledObject tileDefFound = null;
        foreach (var tileDefinition in tileDefinitions)
        {
            if (tileDefinition.id == tileNumberInTileset)
                tileDefFound = tileDefinition.objects[0];
        }

        return tileDefFound;
    }

    //We need to subtract The amount of tiles prior to this, due to multiple tilesets in the map
    private int GetTileNumberFromTileGid(int tileGid, int tilesetNum)
    {
        return tileGid - TileMap.Tilesets[tilesetNum].firstgid;
    }

    private int GetTileColumn(int tileNumberInTileset, int drawTilesetNum)
    {
        return tileNumberInTileset % Tilesets[drawTilesetNum].Columns;
    }

    private int GetTileRow(int tileNumberInTileset, int drawTilesetNum)
    {
        return (int)Math.Floor((double)tileNumberInTileset / (double)Tilesets[drawTilesetNum].Columns);
    }

    //Determine which tileset we should use for this tile (for when using multiple tilesets in the layer.
    private int GetTilesetNumberFromTileGid(int tileGid)
    {
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
            if (tileGid >= firstTilesetGid && tileGid < TileMap.Tilesets[it + 1].firstgid)
            {
                drawTilesetNum = it;
                break;
            }

            it++;
        }

        return drawTilesetNum;
    }

    public void Update(GameTime gameTime)
    {
        BackgroundTiles.ForEach(tile => tile.Update(gameTime));
        SolidTiles.ForEach(tile => tile.Update(gameTime));
        Actors.ForEach(actor => actor.Update(gameTime));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        BackgroundTiles.ForEach(tile => tile.Draw(spriteBatch));
        SolidTiles.ForEach(tile => tile.Draw(spriteBatch));
        Actors.ForEach(actor => actor.Draw(spriteBatch));
    }


    #region Methods

    #endregion
}