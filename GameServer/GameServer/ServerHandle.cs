using System;

namespace GameServer {
    public class ServerHandle {
        public static void WelcomeReceived(int fromClient, Packet packet) {
            int clientIdCheck = packet.ReadInt();
            string username = packet.ReadString();
            
            Console.WriteLine($"{Server.clients[fromClient].Tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
            if (fromClient != clientIdCheck) {
                Console.WriteLine($"Player \"{username}\" (ID: {fromClient} has assumed the wrong client ID ({clientIdCheck}!");
            }
            
            Server.clients[fromClient].SendIntoGame(username);
        }

        public static void BoxClicked(int fromClient, Packet packet) {
            int clientId = packet.ReadInt();
            int boxId = packet.ReadInt();
            
            if (fromClient != clientId) {
                Console.WriteLine($"Player ID: {fromClient} has assumed the wrong client ID ({clientId}!");
            }
            else {
                Server.clients[clientId].Player.ClickBox(boxId);
            }
        }

        public static void UdpTestReceived(int fromClient, Packet packet) {
            string msg = packet.ReadString();
            
            Console.WriteLine($"Received packet via UDP, contains message: {msg}.");
        }
    }
}