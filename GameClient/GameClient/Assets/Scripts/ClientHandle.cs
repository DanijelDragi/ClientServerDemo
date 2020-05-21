using System;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet) {
        string msg = packet.ReadString();
        int myId = packet.ReadInt();
        
        Debug.Log($"Message from server: {msg}");
        Client.Instance.id = myId;
        ClientSend.WelcomeReceived();
        
        //Client.Instance.udp.Connect(((IPEndPoint)Client.Instance.tcp.Socket.Client.LocalEndPoint).Port);
    }

    public static void UdpTest(Packet packet) {
        string msg = packet.ReadString();
        
        Debug.Log($"Received packet via UDP with message: {msg}.");
        ClientSend.UDPTestReceived();
    }

    public static void TimeRemaining(Packet packet) {
        int timeRemaining = packet.ReadInt();
        int clientId = packet.ReadInt();

        if (clientId != Client.Instance.id) {
            Debug.Log($"Somehow got message for client {clientId}, my ID is: {Client.Instance.id}");
        }
        else {
            UIManager.Instance.UpdateTime((int)Math.Floor(timeRemaining/ 1000f));
        }
    }

    public static void GameOver(Packet packet) {
        int score = packet.ReadInt();
        int clientId = packet.ReadInt();
        
        if (clientId != Client.Instance.id) {
            Debug.Log($"Somehow got message for client {clientId}, my ID is: {Client.Instance.id}");
        }
        else {
            UIManager.Instance.GameOver(score);
        }
    }

    public static void BoxSpawned(Packet packet) {
        int clientId = packet.ReadInt();
        
        if (clientId != Client.Instance.id) {
            Debug.Log($"Somehow got message for client {clientId}, my ID is: {Client.Instance.id}");
        }
        else {
            TargetManager.Instance.SpawnTarget(packet.ReadInt(), packet.ReadVector3(), packet.ReadInt());
        }
    }

    public static void BoxPositions(Packet packet) {
        int clientId = packet.ReadInt();
        
        if (clientId != Client.Instance.id) {
            Debug.Log($"Somehow got message for client {clientId}, my ID is: {Client.Instance.id}");
        }
        else {
            int numberOfBoxes = packet.ReadInt();
            for (int i = 0; i < numberOfBoxes; i++) {
                int boxId = packet.ReadInt();
                Vector3 position = packet.ReadVector3();
                TargetManager.Instance.MoveTarget(position, boxId);
            }
        }
    }

    public static void BoxDestroyed(Packet packet) {
        int clientId = packet.ReadInt();
        
        if (clientId != Client.Instance.id) {
            Debug.Log($"Somehow got message for client {clientId}, my ID is: {Client.Instance.id}");
        }
        else {
            int boxId = packet.ReadInt();
            TargetManager.Instance.DestroyTarget(boxId);
        }
    }
}
