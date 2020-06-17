using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viewer.Files
{
    interface ISerializable
    {
        bool Serialize(ref object writer);
        bool Deserialize(object data);
    }
}
