using System;
using System.Collections.Generic;
using System.Linq;
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

    public TiledTmxContent(TiledMap map, TiledTileset[] tilesets, Texture2D[] textures)
    {
        TileMap = map;
        Tilesets = tilesets;
        TilesetTextures = textures;
    }

    public void CreateTileGameObjectsFromContent()
    {
        var groups = TileMap.Groups;
        var bgGroup = groups.FirstOrDefault(group => group.name == "bg");
        //For each layer in the tilemap
        var drawLayers = new List<float>();
        for (int i = 0; i < bgGroup.layers.Length; i++)
        {
            // var layer = TileMap.Layers[i];
            var layer = bgGroup.layers[i];
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

                var currentTile =
                    TileMap.GetTiledTile(TileMap.Tilesets[drawTilesetNum], Tilesets[drawTilesetNum], gid);

                int column = tileFrame % Tilesets[drawTilesetNum].Columns;
                int row = (int)Math.Floor((double)tileFrame / (double)Tilesets[drawTilesetNum].Columns);
                float x = (j % TileMap.Width) * TileMap.TileWidth;
                float y = (float)Math.Floor(j / (double)TileMap.Width) * TileMap.TileHeight;

                Rectangle tilesetRec = new Rectangle(Tilesets[drawTilesetNum].TileWidth * column,
                    Tilesets[drawTilesetNum].TileHeight * row, Tilesets[drawTilesetNum].TileWidth,
                    Tilesets[drawTilesetNum].TileHeight);

                //Create correct tile type

                var drawOrderMultiplier = 0.001f;
                var drawOrder = (i + 1) * drawOrderMultiplier;
                if (layer.name.ToLower().StartsWith("solid"))
                {
                    var solidTile = new SolidTile(
                        new Vector2(x, y),
                        tilesetRec,
                        TilesetTextures[drawTilesetNum],drawOrder
                        );
                    // drawLayers.Add(solidTile.DrawOrder);
                    SolidTiles.Add(solidTile);
                }
                else
                {
                    var tile = new Tile(
                        new Vector2(x, y),
                        tilesetRec,
                        TilesetTextures[drawTilesetNum],drawOrder
                    );
                    // tile.DrawOrder = (i + 1) * drawOrderMultiplier;
                    drawLayers.Add(tile.DrawOrder);
                    BackgroundTiles.Add(tile);
                }
            }
        }

        //Create all of the actors (non tilemaps)
        var actorLayer = TileMap.Layers.FirstOrDefault(layer => layer.name.StartsWith("actor"));
        var actors = actorLayer.objects;
        for (int i = 0; i < actors.Length; i++)
        {
            var potentialActor = actors[i];
            var actorName = potentialActor.name;
            var newActorFunc = TiledActorFactory.NameToSpawnFunction[actorName];
            if (newActorFunc == null) continue;
            var actorLocation = new Vector2(potentialActor.x, potentialActor.y);
            var actorTags = potentialActor.properties;
            var newActor = newActorFunc.Invoke(actorLocation,actorTags);
            Actors.Add(newActor);
            // var go = new GameObject(new Vector2(potentialActor.x, potentialActor.y));
            // var box = new BoxColliderComponent(go, new Point(32, 32));
            // box.Debug = true;
            // go.AddComponent(box);
            // var rb = new RigidbodyComponent(go, box);
            // var drawOrderMultiplier = 0.001f;
            // go.DrawOrder = (actorLayer.id + 1) * drawOrderMultiplier;
            // go.AddComponent(rb);
            // Actors.Add(go);
        }

        drawLayers.Sort();
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