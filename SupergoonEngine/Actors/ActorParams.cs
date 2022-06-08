using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class ActorParams
{
    public Vector2 Location;
    public TiledProperty[] Tags;
    public Rectangle SourceRect;
    public Texture2D Texture;
    public string AsepriteDocString;
    public Vector2 BoxColliderOffset;
    public Point BoxSize;
}