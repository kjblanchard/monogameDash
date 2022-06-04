﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Cameras;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.Graphics;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class BoxColliderComponent : Component
{
    public Point _size;
    public Rectangle Bounds => new Rectangle(Parent._location.ToPoint() + _offset.ToPoint(), _size);

    public BoxColliderComponent(GameObject parent, Point size, Vector2 offset = new()) : base(parent, offset)
    {
        _size = size;
        this.UpdateOrder = 10;
        AddTag(EngineTags.ComponentTags.BoxCollider);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (Debug)
        {
            // Parent.DrawDebug(spriteBatch, new Rectangle(Parent._location.ToPoint(), _size));
            var cameraOffsetBounds = CameraGameComponent.MainCamera.CalculateCameraOffset(Bounds.Location.ToVector2());
            var newRect = new Rectangle(cameraOffsetBounds.ToPoint(), Bounds.Size);
            // Parent.DrawDebug(spriteBatch, Bounds, 1);
            Parent.DrawDebug(spriteBatch, newRect, 1);
        }
    }
}