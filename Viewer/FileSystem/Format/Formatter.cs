using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viewer.Files
{
    interface IFormatter
    {
        bool SaveFile(IFileable f);
        bool LoadFile(IFileable f);
    }
}
