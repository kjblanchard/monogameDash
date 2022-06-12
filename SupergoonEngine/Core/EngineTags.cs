namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

/// <summary>
/// These tags are used by the engine's Gameobjects or components to determine update order, and to determine what type of gameobject they are for quick(er) lookups at runtime rather than casting.
/// </summary>
public struct EngineTags
{
    
    /// <summary>
    /// Tags that are attached to gameobjects
    /// </summary>
    public struct GameObjectTags
    {
        public const int Default = 0;
        public const int Player = 1;
        public const int Tile = 2;

    }

    /// <summary>
    /// Tags that are attached to components, used for draworder mostly.
    /// </summary>
    public struct ComponentTags
    {
        public const int Default = 0;
        public const int PlayerController = 1;
        public const int RigidBody = 2;
        public const int BoxCollider = 3;
        public const int Sprite = 5;
        public const int Camera = 6;
        public const int Debug = 99;
    }
}