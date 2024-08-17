using System;
using System.IO;
using System.Windows.Forms;

namespace osum.Online
{
    public class Request
    {
        public int Id;
        public byte[] Payload;

        public Request(int id, byte[] payload)
        {
            Id = id;
            Payload = payload;
        }

        public void Send(Stream s)
        {
            s.Write(BitConverter.GetBytes((ushort) Id), 0, sizeof(ushort));
            s.WriteByte(0);
            if (Payload == null)
            {
                s.Write(BitConverter.GetBytes((uint)0), 0, sizeof(uint));
                s.Flush();
                return;
            }

            s.Write(BitConverter.GetBytes((uint)Payload.Length), 0, sizeof(uint));
            s.Write(Payload, 0, Payload.Length);
            s.Flush();
        }
    }
}
