using System.Collections.Generic;
using System.Numerics;

namespace GameServer {
    public class Player {
        public int Id;
        public string Username;
        
        public List<int> clickedBoxes = new List<int>();

        private int _boxesDestroyed = 0;

        public Player(int id, string username) {
            Id = id;
            Username = username;
        }

        public void IncreaseScore() {
            _boxesDestroyed++;
        }

        public int Score() {
            return _boxesDestroyed;
        }

        public void ClickBox(int id) {
            lock (clickedBoxes) {
                clickedBoxes.Add(id);
            }
        }
    }
}