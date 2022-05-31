using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using TiledCS;

namespace SupergoonEngine.Tiled;

public static class TiledActorFactory
{
    public static readonly Dictionary<string, Func<Vector2, TiledProperty[], Actor>> NameToSpawnFunction = new();

}