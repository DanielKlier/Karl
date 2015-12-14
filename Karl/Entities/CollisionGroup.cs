
namespace Karl.Entities
{
    public static class CollisionGroup
    {
        public const uint None           = 0x00000000;
        public const uint World          = 0x00000001;
        public const uint Player         = 0x00000002;
        public const uint Projectiles    = 0x00000004;
        public const uint Enemies        = 0x00000008;
    }
}
