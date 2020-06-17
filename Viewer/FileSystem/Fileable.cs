using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viewer.Files
{
    interface IFileable
    {
        CFileInfo FileInfo { get; set; }
        bool Init();
    }
}
