using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;

namespace Viewer.Files
{
    interface IConverter
    {
        JObject Import(string filename);
        bool Export(JObject jobj);
    }
}
