using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GameServer {
    public class Server {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;

        private static TcpListener _tcpListener;
        private static UdpClient _udpListener;

        public static void Start(int maxPlayers, int port) {
            MaxPlayers = maxPlayers;
            Port = port;
            
            Console.WriteLine($"Starting server...");
            
            InitializeServerData();
            
            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            
            //_udpListener = new UdpClient(Port);
            //_udpListener.BeginReceive(UDPReceiveCallback, null);
            
            Console.WriteLine($"Server started on port {port}.");
        }

        private static void TCPConnectCallback(IAsyncResult result) {
            TcpClient client = _tcpListener.EndAcceptTcpClient(result);
            _tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");
            
            for (int i = 1; i <= MaxPlayers; i++) {
                if (clients[i].Tcp.Socket == null) {
                    clients[i].Tcp.Connect(client);
                    return;
                }
            }
            
            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void UDPReceiveCallback(IAsyncResult result) {
            try {
                IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = _udpListener.EndReceive(result, ref clientEndpoint);
                _udpListener.BeginReceive(UDPReceiveCallback, null);

                if (data.Length < 4) {
                    return;
                }

                using (Packet packet = new Packet(data)) {
                    int clientId = packet.ReadInt();

                    if (clientId == 0) {
                        return;
                    }

                    if (clients[clientId].Udp.EndPoint == null) {
                        clients[clientId].Udp.Connect(clientEndpoint);
                        return;
                    }

                    if (clients[clientId].Udp.EndPoint.ToString() == clientEndpoint.ToString()) {
                        clients[clientId].Udp.HandleData(packet);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine($"Error receiving UDP data: {e}");
            }
        }

        public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet) {
            try {
                if (clientEndPoint != null) {
                    _udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
                }
            }
            catch (Exception e) {
                Console.WriteLine($"Error sending data to {clientEndPoint} via UDP: {e}");
            }
        }

        private static void InitializeServerData() {
            for (int i = 1; i <= MaxPlayers; i++) {
                clients.Add(i, new Client(i));
            }
            
            PacketHandlers = new Dictionary<int, PacketHandler>() {
                {(int) ClientPackets.WelcomeReceived, ServerHandle.WelcomeReceived},
                {(int) ClientPackets.UdpTestReceived, ServerHandle.UdpTestReceived},
                {(int) ClientPackets.boxClicked, ServerHandle.BoxClicked}
            };
            
            Console.WriteLine("Initialized packet handlers.");
        }
    }
}