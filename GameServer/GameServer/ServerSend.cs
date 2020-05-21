using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer {
    public static class ServerSend {

        private static void SendTCPData(int toClient, Packet packet) {
            packet.WriteLength();
            Server.clients[toClient].Tcp.SendData(packet);
        }

        private static void SendTCPDataToAll(Packet packet) {
            packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++) {
                Server.clients[i].Tcp.SendData(packet);
            }
        }
        
        private static void SendTCPDataToAllButOne(int exceptClient, Packet packet) {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                if(i != exceptClient) Server.clients[i].Tcp.SendData(packet);
            }
        }

        private static void SendUDPData(int toClient, Packet packet) {
            packet.WriteLength();
            Server.clients[toClient].Udp.SendData(packet);
        }
        
        private static void SendUDPDataToAll(Packet packet) {
            packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++) {
                Server.clients[i].Udp.SendData(packet);
            }
        }
        
        private static void SendUDPDataToAllButOne(int exceptClient, Packet packet) {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                if(i != exceptClient) Server.clients[i].Udp.SendData(packet);
            }
        }
        
        public static void Welcome(int toClient, String msg) {
            using (Packet packet = new Packet((int) ServerPackets.Welcome)) {
                packet.Write(msg);
                packet.Write(toClient);

                SendTCPData(toClient, packet);
            }
        }

        public static void SendTime(int toClient, int time) {
            using (Packet packet = new Packet((int) ServerPackets.TimeRemaining)) {
                packet.Write(time);
                packet.Write(toClient);
                
                SendTCPData(toClient, packet);
            }
        }
        
        public static void GameOver(int toClient, int score) {
            using (Packet packet = new Packet((int) ServerPackets.GameOver)) {
                packet.Write(score);
                packet.Write(toClient);
                
                SendTCPData(toClient, packet);
            }
        }

        public static void BoxSpawned(int toClient, Box box) {
            using (Packet packet = new Packet((int) ServerPackets.BoxSpawned)) {
                packet.Write(toClient);
                packet.Write(box.Id);
                packet.Write(box.Position);
                packet.Write(box.ClickCapacity);
                
                SendTCPData(toClient, packet);
            }
        }

        public static void SendBoxes(int toClient, List<Box> boxes) {
            using (Packet packet = new Packet((int) ServerPackets.BoxPositions)) {
                packet.Write(toClient);
                packet.Write(boxes.Count);
                foreach (Box box in boxes) {
                    packet.Write(box.Id);
                    packet.Write(box.Position);
                }
                
                SendTCPData(toClient, packet);
            }
        }

        public static void BoxDestroyed(int toClient, int boxId) {
            using (Packet packet = new Packet((int) ServerPackets.BoxDestroyed)) {
                packet.Write(toClient);
                packet.Write(boxId);
                
                SendTCPData(toClient, packet);
            }
        }

        public static void UDPTest(int toClient) {
            using (Packet packet = new Packet((int) ServerPackets.UdpTest)) {
                packet.Write("UDP connection test");
                
                SendUDPData(toClient, packet);
            }
        }
    }
}