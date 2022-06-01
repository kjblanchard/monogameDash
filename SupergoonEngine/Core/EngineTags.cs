namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public struct EngineTags
{
    public struct GameObjectTags
    {
        public const int Default = 0;
        public const int Player = 1;
        public const int Tile = 2;

    }

    public struct ComponentTags
    {
        public const int Default = 0;
        public const int PlayerController = 1;
        public const int RigidBody = 2;
        public const int BoxCollider = 3;
        public const int Sprite = 5;
        public const int Debug = 99;
    }
}