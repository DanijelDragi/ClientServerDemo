using System.Numerics;

namespace GameServer {
    public static class Constants {
        public const int TICKS_PER_SEC = 30;
        public const int MS_PER_TICK = 1000 / TICKS_PER_SEC;
        
        public const int BOX_MIN_CLICKS = 1;
        public const int BOX_MAX_CLICKS = 7;
        public const int GAME_DURATION = 30000;
        public const int BOX_CREATION_DELAY = 1000;
        public const int BOX_POSITION_UPDATE_INTERVAL = 250;

        public const float DELTA_X = 0.25f;
        public const float DELTA_Z = 0.125f;
    }
}