using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace SupergoonEngine.Tiled;

public class TiledGameComponent : GameComponent
{
    public TiledTmxContent LoadedTmxContent;

    public TiledGameComponent(Game game) : base(game)
    {
    }

    public TiledTmxContent LoadTilesets(string mapToLoadFor)
    {
        var map = new TiledMap($"{Game.Content.RootDirectory}\\tiled\\{mapToLoadFor}.tmx");
        var content = Game.Content;
        var tilesetLength = map.Tilesets.Length;
        var tilesets = new TiledTileset[tilesetLength];
        var textures = new Texture2D[tilesetLength];
        for (int i = 0; i < map.Tilesets.Length; i++)
        {
            var tilesetName = map.Tilesets[i].source;
            var tilesetFullPath = $"Content\\Tiled\\{tilesetName}";
            var loadedTileset = new TiledTileset(tilesetFullPath);
            var tilesetImageName = loadedTileset.Image.source.Split('.').First();
            var tilesetImageFullPath = $"Tiled\\{tilesetImageName}";
            var tilesetImage = content.Load<Texture2D>(tilesetImageFullPath);

            
            tilesets[i] = loadedTileset;
            textures[i] = tilesetImage;
        }
        LoadedTmxContent = new TiledTmxContent(map, tilesets, textures);
        LoadedTmxContent.CreateTileGameObjectsFromContent();
        return LoadedTmxContent;
    }
}