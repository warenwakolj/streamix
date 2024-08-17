using System.IO;

namespace osum.Online
{
    public class OsuReader : BinaryReader
    {
        public OsuReader() : base(new MemoryStream()) { }
        public OsuReader(Stream stream) : base(stream) { }

        public override string ReadString()
        {
            int stringByte = ReadByte();
            if (stringByte != 11) return "";
            return base.ReadString();
        }
    }
}
