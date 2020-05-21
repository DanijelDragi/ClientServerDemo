using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet packet) {
        packet.WriteLength();
        Client.Instance.tcp.SendData(packet);
    }

    private static void SendUDPData(Packet packet) {
        packet.WriteLength();
        Client.Instance.udp.SendData(packet);
    }

    public static void WelcomeReceived() {
        using (Packet packet = new Packet((int) ClientPackets.WelcomeReceived)) {
            packet.Write(Client.Instance.id);
            packet.Write(UIManager.Instance.usernameField.text);
            
            SendTCPData(packet);
        }
    }

    public static void BoxClicked(int boxId) {
        using (Packet packet = new Packet((int) ClientPackets.boxClicked)) {
            packet.Write(Client.Instance.id);
            packet.Write(boxId);
            
            SendTCPData(packet);
        }
    }

    public static void UDPTestReceived() {
        using (Packet packet = new Packet((int) ClientPackets.UdpTestReceived)) {
            packet.Write("UDP test packet received");
            
            SendUDPData(packet);
        }
    }
}
