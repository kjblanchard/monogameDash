using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Cameras;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class BoxColliderComponent : Component
{
    public bool IsBlocking = true;
    public List<uint> LastFrameOverlaps = new();
    public List<uint> ThisFrameOverlaps = new();


    public event OverlapEventArgs OverlapEvent;
    public delegate void OverlapEventArgs(GameObject overlapee);
    public Point _size;
    public Rectangle Bounds => new Rectangle(Parent.Location.ToPoint() + _offset.ToPoint(), _size);

    public BoxColliderComponent(GameObject parent, Point size, Vector2 offset = new()) : base(parent, offset)
    {
        _size = size;
        this.UpdateOrder = 10;
        AddTag(EngineTags.ComponentTags.BoxCollider);
        Debug = false;
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

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        var noLongerOverlapping = LastFrameOverlaps.Except(ThisFrameOverlaps);
        foreach (var u in noLongerOverlapping)
        {
            OnOverlapEnd(null);
        }
        LastFrameOverlaps.Clear();
        LastFrameOverlaps.AddRange(ThisFrameOverlaps);
        ThisFrameOverlaps.Clear();
    }

    public void OnOverlapEvent(GameObject overlapee)
    {
        //It was already overlapped
        ThisFrameOverlaps.Add(overlapee.Id);
        if (LastFrameOverlaps.Contains(overlapee.Id))
            return;
        OverlapEvent?.Invoke(overlapee);
    }

    public void OnOverlapEnd(GameObject noLongerOverlapee)
    {
        Console.WriteLine("NOOOOOOOOOO");
    }
}