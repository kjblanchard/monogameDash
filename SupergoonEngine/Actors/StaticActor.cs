﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Documents;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using TiledCS;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

/// <summary>
/// Used for Actors that will not need to move or be animated, only comes with a box collider and sprite component, eg spikes
/// </summary>
public class StaticActor : GameObject
{
    protected SpriteComponent _spriteComponent;
    protected BoxColliderComponent _boxColliderComponent;

    public StaticActor(ActorParams actorParams) : base(actorParams.Location)
    {
        _spriteComponent = new SpriteComponent(this);
        _boxColliderComponent = new BoxColliderComponent(this, actorParams.BoxSize, actorParams.BoxColliderOffset);

        AddComponent(_boxColliderComponent, _spriteComponent
        );
    }

    public static GameObject FactoryFunction(Vector2 location, TiledProperty[] tags = null,
        Rectangle textureRect = new Rectangle(), Texture2D texture = null)
    {
        throw new System.NotImplementedException();
    }
}