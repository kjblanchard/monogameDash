using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using TiledCS;

namespace SupergoonEngine.Tiled;

public static class TiledActorFactory
{
    public delegate GameObject ActorFactoryDelegate(ActorParams actorParams);

    // public static readonly Dictionary<string, Func<Vector2, TiledProperty[], Actor>> NameToSpawnFunction = new();
    public static readonly Dictionary<string, ActorFactoryDelegate> NameToSpawnFunction = new();
}