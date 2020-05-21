using System;
using System.Threading;

namespace GameServer {
    class Program {
        private static bool _isRunning = false;
        
        public static void Main(string[] args) {
            Console.Title = "GameServer";

            _isRunning = true;
            Thread mainThread = new Thread(MainThread);
            mainThread.Start();
            
            Server.Start(10, 63211);
        }

        private static void MainThread() {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime nextLoop = DateTime.Now;

            while (_isRunning) {
                while (nextLoop < DateTime.Now) {
                    GameLogic.Update();
                    nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (nextLoop > DateTime.Now) {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
