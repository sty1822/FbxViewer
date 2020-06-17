using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;
using OpenTK;
using Viewer.Files;



namespace Viewer.Asset
{
    class CModel
    {
        public CModel()
        {
            BoneCount = 0;
            Mesh = new CMesh();
            Materials = new Dictionary<string, CMaterial>();
            Anim = new CAnimation();
        }

        public bool Init(string filename)
        {
            //
            CFbxConverter conv = new CFbxConverter();
            JObject jmodel = conv.Import(filename);
            Deserialize(jmodel);

            Mesh.Init();
            Anim.Init();


            return true;
        }

        public void Progress(Matrix4 mat)
        {
            if(Anim != null)
                Anim.Progress(mat);
        }
        
        public CMaterial GetMaterial(string subset_name)
        {
            return null;
        }

        private bool ImportFbx(string filename)
        {

            return true;
        }

        public bool Deserialize(object data)
        {
            JObject jm = data as JObject;
            if (jm == null)
                return false;

            if (jm["BoneCount"] != null)
                BoneCount = jm["BoneCount"].Value<int>();

            if(jm["Mesh"] != null)
            {
                Mesh.Deserialize(jm["Mesh"]);
            }

            //
            JArray jmats = jm["Materials"].Value<JArray>();
            foreach(JObject jmat in jmats)
            {
                string linked_subsetname = jmat["LinkedSubset"].Value<string>().ToLower();
                if(Materials.ContainsKey(linked_subsetname) == false)
                {
                    string mat_type = jmat["Type"].Value<string>();
                    CMaterial material = CMaterial.Create(mat_type, linked_subsetname);

                    if (material.Deserialize(jmat) == false)
                        continue;

                    Materials.Add(linked_subsetname, material);
                }
            }

            // anim
            JObject janimCtrl = jm["AnimCtrl"].Value<JObject>();
            JArray janims = janimCtrl["Anims"].Value<JArray>();
            if(janims.Count != 0)
            {
                Anim.Deserialize(janims.First.Value<JObject>());
            }

            return true;
        }

        public int BoneCount { get; set; }
        public Dictionary<string, CMaterial> Materials { get; private set; }
        public CMesh Mesh { get; private set; }
        public CAnimation Anim { get; private set; }

        
    }
}
