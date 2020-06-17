using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Viewer.Files
{
    class CJsonFormatter : IFormatter
    {

        #region Public Method
        public bool SaveFile(IFileable f)
        {
            if (f == null)
                return false;

            ISerializable iser = f as ISerializable;

            if (iser == null)
                return false;

            object jobj = new JObject();

            if (iser.Serialize(ref jobj) == false)
                return false;

            var setting = new JsonSerializerSettings();
            setting.Formatting = Formatting.Indented; // 들여쓰기, 줄내림(None : 용량 반으로 줄어듬)
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 순환 참조 객체 무시
            string json = JsonConvert.SerializeObject(jobj, setting);

            File.WriteAllText(f.FileInfo.FullFileName, json);

            return true;

        }

        public bool LoadFile(IFileable f)
        {
            try
            {
                string str = File.ReadAllText(f.FileInfo.FullFileName);

                JObject jobj = JObject.Parse(str);

                ISerializable iser = f as ISerializable;
                if (iser == null)
                    return false;

                if (iser.Deserialize(jobj) == false)
                    return false;

            }
            catch(Exception e)
            {

            }

            return true;
        }
        #endregion
    }
}
