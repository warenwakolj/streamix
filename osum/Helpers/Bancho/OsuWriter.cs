using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace osum.Online
{
    public class OsuWriter : BinaryWriter
    {
        public OsuWriter() : base(new MemoryStream()) { }

        public OsuWriter(Stream s) : base(s) { }

        public override void Write(string value)
        {
            if (value == null || value == "")
            {
                base.Write((byte) 0);
                return; 
            }

            base.Write((byte) 11);
            base.Write(value);
        }
    }
}
