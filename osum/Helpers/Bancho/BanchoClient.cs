using osum;
using osum.GameModes;
using osum.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace osum.Online
{
    public class BanchoClient
    {
        private const int PING_TIMEOUT = 20000;
        private bool Authenticated;
        private TcpClient client;
        private bool Connected;
        private long lastPingTime;
        private int pingTimeout = PING_TIMEOUT;
        private byte[] readByteArray = new byte[6];
        private int readBytes;
        private bool readingHeader = true;
        private int readType;
        private int sendSequence;
        private NetworkStream stream;
        private Thread thread;
        private StreamWriter writer;

        public static event EventHandler<LoginResultEventArgs> LoginResult;

        private static void OnLoginResult(int result)
        {
            LoginResult?.Invoke(null, new LoginResultEventArgs(result));
        }

        public bool IsConnected => Connected;
        public bool IsAuthenticated => Authenticated;
        public int userId { get; private set; }


        public bool Connect()
        {
            try
            {
                Disconnect(true);
                client = new TcpClient("149.28.160.232", 13381);
                client.NoDelay = true;

                var socket = client.Client;
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                stream = client.GetStream();
                writer = new StreamWriter(stream);

                Console.WriteLine("Bancho is connected..");

                Connected = true;
                Authenticated = true;

                writer.AutoFlush = true;
                   Thread.Sleep(500);
                return true;
            }
            catch (Exception)
            {
                FailConnection("Connection to Bancho failed..", 60000);
                return false;
            }
        }



        public int Login(string username, string hashedPassword)
        {
            if (!Connected)
                return -5;

            for (int attempt = 0; attempt < 3; attempt++)
            {
                writer.WriteLine(username);
                writer.WriteLine(hashedPassword);
                writer.WriteLine("b420|" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours);

                int receivedUserId = ProcessServerResponse();

                if (receivedUserId > 0)
                {
                    Authenticated = true;
                    userId = receivedUserId; 
                    return userId; 
                }
                else if (receivedUserId == -5 && attempt < 2)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    return receivedUserId; 
                }
            }

            return -5; 
        }





        public int ProcessServerResponse()
        {
            try
            {
                byte[] headerBuffer = new byte[7];
                int bytesRead = stream.Read(headerBuffer, 0, headerBuffer.Length);
                if (bytesRead != 7)
                {
                    return -5;
                }

                int packetType = BitConverter.ToUInt16(headerBuffer, 0);
                bool isCompressed = BitConverter.ToBoolean(headerBuffer, 2);
                uint contentLength = BitConverter.ToUInt32(headerBuffer, 3);

                if (contentLength > 0)
                {
                    byte[] contentBuffer = new byte[contentLength];
                    bytesRead = stream.Read(contentBuffer, 0, (int)contentLength);
                    if (bytesRead != contentLength)
                    {
                        return -5;
                    }

                    if (packetType == 5)
                    {
                        int result = BitConverter.ToInt32(contentBuffer, 0);
                        return result;
                    }
                }

                return -5;
            }
            catch (Exception)
            {
                Thread.Sleep(200);
                try
                {
                    byte[] retryBuffer = new byte[4];
                    int retryBytesRead = stream.Read(retryBuffer, 0, 4);
                    if (retryBytesRead == 4)
                    {
                        int retryResult = BitConverter.ToInt32(retryBuffer, 0);
                        return retryResult;
                    }
                }
                catch { }

                return -5;
            }
        }



        public class LoginResultEventArgs : EventArgs
        {
            public int Result { get; }

            public LoginResultEventArgs(int result)
            {
                Result = result;
            }
        }

        private void FailConnection(string message, int msRetry)
        {
            Console.WriteLine(message);

            lastPingTime = GameBase.Time;
            pingTimeout = msRetry;

            Disconnect(false);
        }


        public void Disconnect(bool resetTimeout)
        {
            Authenticated = false;
            Connected = false;

            if (resetTimeout)
                pingTimeout = PING_TIMEOUT;

            sendSequence = 0;

            ResetReadArray(true);

            if (client == null || !client.Connected)
                return;

            client.Close();
        }



        private void ResetReadArray(bool isHeader)
        {
            if (isHeader)
                readByteArray = new byte[7];
            readingHeader = isHeader;
            readBytes = 0;
        }

        private void Run()
        {
            while (Connected)
            {
                if (client != null && client.Connected)
                {
                    try
                    {
                        while (stream != null && stream.DataAvailable)
                        {
                            readBytes += stream.Read(readByteArray, readBytes, readByteArray.Length - readBytes);

                            if (readingHeader && readBytes == 7)
                            {
                                readType = BitConverter.ToUInt16(readByteArray, 0);
                                uint length = BitConverter.ToUInt32(readByteArray, 3);

                                ResetReadArray(false);
                                readByteArray = new byte[length];
                            }

                            if (!readingHeader && readBytes == readByteArray.Length)
                            {
                                ProcessPacket(readType, new MemoryStream(readByteArray));
                                ResetReadArray(true);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        FailConnection("Error while reading from the server.", 60000);
                    }
                }

                Thread.Sleep(20); // lets not make my pc die
            }
        }

        private void ProcessPacket(int packetType, MemoryStream packetData)
        {
            var reader = new BinaryReader(packetData);

            switch (packetType)
            {
                case 5:
                    int userId = reader.ReadInt32();
                    OnLoginResult(userId);
                    break;
                case 8:
                    SendPacket(8, new byte[0]);
                    break;
            }
        }
        // I don't have gamestate yet xd
        //      public void UpdateStatus(GameState state, string beatmapChecksum = "", string extraText = "")
        //     {
        //           if (!Authenticated || !Connected) return;
        //
        //     MemoryStream ms = new MemoryStream();
        //      BinaryWriter writer = new BinaryWriter(ms);

        //        writer.Write((byte)state); 
        //        writer.Write(true);
        //       writer.Write(extraText);
        //       writer.Write(beatmapChecksum); 
        //       writer.Write((ushort)0); 

        //        SendPacket(0, ms.ToArray());
        //       }

        private void SendPacket(int packetType, byte[] data)
        {
            writer.Write((ushort)packetType);
            writer.Write(data.Length);
            writer.Write(data);
        }

        public void Exit()
        {
            Disconnect(true);
            thread?.Abort();
        }
    }
}