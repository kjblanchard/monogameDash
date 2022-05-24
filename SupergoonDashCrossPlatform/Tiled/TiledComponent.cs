using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using TiledCS;

namespace SupergoonDashCrossPlatform.Tiled;

public class TiledComponent : GameComponent
{
    public TiledTmxContent LoadedTmxContent;
    #region Configuration

    #endregion


    #region Methods

    #endregion

    public TiledComponent(Game game) : base(game)
    {
        //Load the full tmx file for the level.
        var map = new TiledMap(game.Content.RootDirectory + "\\tiled\\level1.tmx");
        
        LoadedTmxContent = LoadTilesets(map, game);
    }

    private TiledTmxContent LoadTilesets(TiledMap mapToLoadFor, Game game)
    {
        var content = game.Content;
        var tilesetLength = mapToLoadFor.Tilesets.Length;
        var tilesets = new TiledTileset[tilesetLength];
        var textures = new Texture2D[tilesetLength];
        for (int i = 0; i < mapToLoadFor.Tilesets.Length; i++)
        {
            var tilesetName = mapToLoadFor.Tilesets[i].source;
            var tilesetFullPath = $"Content\\Tiled\\{tilesetName}";
            var loadedTileset = new TiledTileset(tilesetFullPath);
            var tilesetImageName = loadedTileset.Image.source.Split('.').First();
            var tilesetImageFullPath = $"Tiled\\{tilesetImageName}";
            var tilesetImage = content.Load<Texture2D>(tilesetImageFullPath);

            
            tilesets[i] = loadedTileset;
            textures[i] = tilesetImage;
        }

        return new TiledTmxContent(mapToLoadFor, tilesets, textures);
    }
}