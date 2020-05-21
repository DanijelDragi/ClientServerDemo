using System;
using System.Collections.Generic;
using System.Threading;

namespace GameServer {
    public static class GameLogic {

        public static void Update() {
            ThreadManager.UpdateMain();
        }

        public static void PlayGame(Client client) {
            Console.WriteLine($"Game started with client {client.Id}!");
            
            Dictionary<int, Box> boxes = new Dictionary<int, Box>();
            
            DateTime endTime = DateTime.Now.AddMilliseconds(Constants.GAME_DURATION);
            DateTime positionUpdateTime = DateTime.Now.AddMilliseconds(Constants.BOX_POSITION_UPDATE_INTERVAL);
            DateTime boxCreationTime = DateTime.Now;
            List<int> clickedBoxesCopy;

            while (DateTime.Now <= endTime) {
                if (client.Player != null) {
                    DateTime now = DateTime.Now;
                    ServerSend.SendTime(client.Id, Convert.ToInt32((endTime - now).TotalMilliseconds));

                    List<int> clickedBoxes = client.Player.clickedBoxes;
                    lock (clickedBoxes) {
                        clickedBoxesCopy = new List<int>(client.Player.clickedBoxes);
                        clickedBoxes.Clear();
                    }

                    foreach (int boxId in clickedBoxesCopy) {
                        Box box = boxes[boxId];
                        box.RecordClick();
                        if (box.shouldDestroy()) {
                            boxes.Remove(boxId);
                            ServerSend.BoxDestroyed(client.Id, boxId);
                        }
                    }
                    
                    if (positionUpdateTime <= now.AddMilliseconds(5)) {
                        foreach (Box box in boxes.Values) {
                            box.UpdatePosition();
                        }
                        positionUpdateTime = now.AddMilliseconds(Constants.BOX_POSITION_UPDATE_INTERVAL);
                        
                        ServerSend.SendBoxes(client.Id, new List<Box>(boxes.Values));
                    }

                    if (boxCreationTime <= now.AddMilliseconds(5)) {
                        Box newBox = new Box(client.Player);
                        boxes.Add(newBox.Id, newBox);
                        boxCreationTime = now.AddMilliseconds(Constants.BOX_CREATION_DELAY);
                        
                        ServerSend.BoxSpawned(client.Id, newBox);
                    }

                    DateTime firstAction = positionUpdateTime < boxCreationTime ? positionUpdateTime : boxCreationTime;
                    int delayUntilNextAction = Convert.ToInt32((firstAction - DateTime.Now).TotalMilliseconds);

                    Thread.Sleep(delayUntilNextAction);
                }
                else {
                    Console.WriteLine("Player quit before game was over!");
                    return;
                }
            }
            Console.WriteLine($"Game finished with client {client.Id}, score {client.Player.Score()}!");
            ServerSend.GameOver(client.Id, client.Player.Score());
        }
    }
}