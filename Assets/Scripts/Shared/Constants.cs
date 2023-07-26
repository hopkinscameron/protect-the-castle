namespace ProtectTheCastle.Shared
{
    public static class Constants
    {
        public const string VIRTUAL_CAMERA_TAG = "Virtual Camera";
        
        public static class Player1
        {
            public const string TAG = "Player";
            public const string CASTLE_TAG = "Player Castle";
            public const string TOWER_TAG = "Player Tower";
        }

        public static class Player2
        {
            public const string TAG = "Enemy";
            public const string CASTLE_TAG = "Enemy Castle";
            public const string TOWER_TAG = "Enemy Tower";
        }

        public static class Animations
        {
            public const string ANIMATOR_SPEED_NAME = "Speed";
            public const string ANIMATOR_ATTACK_NAME = "Attack";
            public const string ANIMATOR_GET_HIT_NAME = "GetHit";
            public const string ANIMATOR_DIE_NAME = "Die";
            public const string ANIMATOR_VICTORY_NAME = "Victory";
        }
    }
}
