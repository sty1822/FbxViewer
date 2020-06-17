using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OpenTK;

namespace Viewer.Files
{
    class CJsonConverter
    {
        public static JArray ConvertVector2(Vector2 v)
        {
            JArray jarr = new JArray();

            jarr.Add(v.X);
            jarr.Add(v.Y);

            return jarr;
        }

        public static Vector2 ConvertVector2(JArray jarr)
        {
            Vector2 v;
            v.X = jarr[0].Value<float>();
            v.Y = jarr[1].Value<float>();

            return v;
        }

        public static JArray ConvertVector3(Vector3 v)
        {
            JArray jarr = new JArray();

            jarr.Add(v.X);
            jarr.Add(v.Y);
            jarr.Add(v.Z);

            return jarr;
        }

        public static Vector3 ConvertVector3(JArray jarr)
        {
            Vector3 v;
            v.X = jarr[0].Value<float>();
            v.Y = jarr[1].Value<float>();
            v.Z = jarr[2].Value<float>();

            return v;
        }

        public static JArray ConvertColor(Color c)
        {
            JArray jarr = new JArray();
            jarr.Add(c.R);
            jarr.Add(c.G);
            jarr.Add(c.B);
            jarr.Add(c.A);

            return jarr;
        }

        public static Color ConvertColor(JArray jarr)
        {
            int r = jarr[0].Value<int>();
            int g = jarr[1].Value<int>();
            int b = jarr[2].Value<int>();
            int a = jarr[3].Value<int>();

            return Color.FromArgb(a, r, g, b);
        }
    }
}
