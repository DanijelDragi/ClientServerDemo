using System;
using System.Numerics;

namespace GameServer {
    public class Box {
        private static Random _random = new Random();
        private static int nextId = 1;

        public int Id;
        public int ClickCapacity;
        public Vector3 Position = new Vector3(0, 0, 0);

        private int _clicksRecorded = 0;
        private Player _player;
        private Vector3 _delta;
        private bool _destroy = false;
        
        public Box(Player player) {
            _delta = GenerateDelta();
            ClickCapacity = _random.Next(Constants.BOX_MIN_CLICKS, Constants.BOX_MAX_CLICKS);
            _player = player;
            Id = nextId;
            nextId++;
            if (nextId >= int.MaxValue - 1) nextId = 1;
        }

        public void RecordClick() {
            _clicksRecorded++;
            if (_clicksRecorded >= ClickCapacity) {
                _player.IncreaseScore();
                _destroy = true;
            }
        }

        public void UpdatePosition() {
            Position += _delta;
        }

        public bool shouldDestroy() {
            return _destroy;
        }

        private static Vector3 GenerateDelta() {
            int positiveX = -1;
            int positiveZ = -1;
            if (_random.NextDouble() > 0.5) positiveX = 1;
            if (_random.NextDouble() > 0.5) positiveZ = 1;
            
            return new Vector3(Constants.DELTA_X * positiveX, 0, Constants.DELTA_Z * positiveZ);
        }
    }
}