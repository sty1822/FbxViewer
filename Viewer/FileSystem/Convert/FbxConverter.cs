using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Newtonsoft.Json.Linq;
using OpenTK;

namespace Viewer.Files
{
    class CFbxConverter
    {
        #region Public Method
        // Fbx -> Json
        public JObject Import(string filename)
        {
            JObject jmodel = new JObject();

            
            using (mData = new CliFbx.FbxData())
            {
                if (mData.Import(filename) == false)
                {
                    MessageBox.Show(mData.DisplayInfo);
                    return null;
                }

                mPath = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar;

                JObject jmesh = new JObject();
                JArray jmaterials = new JArray();
                JObject janimctrl = new JObject();

                if(ConvertMesh(jmesh, jmaterials) == false)
                {
                    // error
                }
                if(ConvertAnim(janimctrl) == false)
                {
                    // error
                }

                jmodel.Add("BoneCount", mData.BoneHierarchy.Count);
                jmodel.Add("Mesh", jmesh);
                jmodel.Add("Materials", jmaterials);
                jmodel.Add("AnimCtrl", janimctrl);

            }

            return jmodel;
        }

        
        #endregion

        #region Private Method
        private bool ConvertMesh(JObject jmesh, JArray jmaterials)
        {
            JArray jindices = new JArray();
            JArray jvertices = new JArray();
            JArray jsubsets = new JArray();

            ConvertVertices(jvertices, jindices);
            ConvertSubsetAndMaterial(jsubsets, jmaterials);

            jmesh.Add("Vertices", jvertices);
            jmesh.Add("Indices", jindices);
            jmesh.Add("Subsets", jsubsets);

            return true;
        }

        private bool ConvertAnim(JObject janimCtrl)
        {
            JArray janims = new JArray();

            if (mData.BoneHierarchy.Count <= 0)
                return false;

            // Fbx파일 하나당 Anim 하나로 처리합니다.
            JObject janim = new JObject();

            janim.Add("Frames", mData.FrameCount);

            JArray jbones = new JArray();

            foreach(CliFbx.CNode sourNode in mData.BoneHierarchy)
            {
                JObject jbone = new JObject();

                jbone.Add("Idx", sourNode.Index);
                jbone.Add("PI", sourNode.ParentIndex);
                jbone.Add("Name", sourNode.Name);

                JArray jframes = new JArray();
                for(int key = 0; key < mData.FrameCount; ++key)
                {
                    JObject jkey = new JObject();

                    // translate
                    if (sourNode.LclTransFrames.ContainsKey(key) == true)
                        jkey.Add("LT", ConvertVector3(sourNode.LclTransFrames[key]));
                    else
                        jkey.Add("LT", ConvertVector3(sourNode.LclTranslate));

                    // rotate
                    if (sourNode.LclRotFrames.ContainsKey(key) == true)
                        jkey.Add("LR", ConvertVector3(sourNode.LclRotFrames[key]));
                    else
                        jkey.Add("LR", ConvertVector3(sourNode.LclRotation));

                    // scale
                    if (sourNode.LclScalingFrames.ContainsKey(key) == true)
                        jkey.Add("LS", ConvertVector3(sourNode.LclScalingFrames[key]));
                    else
                        jkey.Add("LS", ConvertVector3(sourNode.LclScaling));

                    jframes.Add(jkey);
                }
                jbone.Add("Frames", jframes);

                jbones.Add(jbone);
            }

            janim.Add("Bones", jbones);

            JObject jidle = new JObject();
            jidle.Add("Name", "Anim");
            jidle.Add("File", janim);

            janims.Add(jidle);
            janimCtrl.Add("Anims", janims);

            return true;
        }

        private void ConvertVertices(JArray jvertices, JArray jindices)
        {
            // Optimize
            if(mData.FrameCount > 0)
            {
                // Animation이 있으면 정점을 Bone의 공간으로 이동시켜 저장
                JArray tempJvertices = new JArray();
                IndexingVertices(jindices, tempJvertices);

                // Animation을 위해 영향받는 본 공간으로 변환
                foreach(JObject tempJVertex in tempJvertices)
                {
                    Matrix4 matInverse = Matrix4.Identity;
                    int bone_index = tempJVertex["BI"].Value<int>();
                    if (bone_index >= 0)
                        matInverse = ConvertMatrix(mData.BoneHierarchy[bone_index].GlobalBindposeInverse);

                    JObject jvertex = new JObject();

                    Vector3 pos = CJsonConverter.ConvertVector3(tempJVertex["Pos"].Value<JArray>());
                    Vector3 nor = CJsonConverter.ConvertVector3(tempJVertex["Nor"].Value<JArray>());

                    pos = OpenTK.Vector3.TransformPosition(pos, matInverse);
                    nor = OpenTK.Vector3.TransformNormal(nor, matInverse);

                    jvertex.Add("Pos", CJsonConverter.ConvertVector3(pos));
                    jvertex.Add("Nor", CJsonConverter.ConvertVector3(nor));
                    jvertex.Add("BI", tempJVertex["BI"]);
                    jvertex.Add("UV", tempJVertex["UV"]);

                    jvertices.Add(jvertex);
                }

            
            }
            else
            {
                // Animation이 없으면 중복 정점 처리 및 인덱싱만...
                IndexingVertices(jindices, jvertices);
            }
            
        }

        private void IndexingVertices(JArray jindices, JArray jvertices)
        {
            // Optimize & Indexing
            foreach(CliFbx.CVertex sourVertex in mData.Vertices)
            {
                bool dontAdd = false;
                int destIndex = 0;

                //
                foreach(JObject tempVertex in jvertices)
                {
                    if(Equal(sourVertex, tempVertex))
                    {
                        dontAdd = true;
                        jindices.Add(destIndex);
                        break;
                    }
                    destIndex++;
                }

                if (dontAdd)
                    continue;

                jvertices.Add(ConvertVertex(sourVertex));
                jindices.Add(jvertices.Count - 1);
            }
        }

        private void ConvertSubsetAndMaterial(JArray jsubsets, JArray jmaterials)
        {
            int matOffset = 0;

            foreach(CliFbx.CMaterial sourMaterial in mData.Materials)
            {
                // subset
                JObject jsubset = new JObject();
                jsubset.Add("Name", sourMaterial.MaterialName);
                jsubset.Add("Offset", matOffset);
                jsubset.Add("TriCnt", sourMaterial.TriangleCount);
                jsubsets.Add(jsubset);

                // material
                JObject jmat = new JObject();
                jmat.Add("Type", Asset.MaterialType.Phong.ToString());
                jmat.Add("Texture", mPath + System.IO.Path.GetFileName(sourMaterial.TextureName));
                jmat.Add("LinkedSubset", sourMaterial.MaterialName);
                jmaterials.Add(jmat);

                //
                matOffset += sourMaterial.TriangleCount * 3;
            }
        }

        private static JObject ConvertVertex(CliFbx.CVertex fv)
        {
            JObject jv = new JObject();

            jv.Add("Pos", ConvertVector3(fv.Position));
            jv.Add("Nor", ConvertVector3(fv.Normal));
            jv.Add("UV", ConvertVector2(fv.UV));

            int boneIndex = -1;
            if (fv.Weights.Count > 0)
                boneIndex = fv.Weights[0].BlendingIndex;
            jv.Add("BI", boneIndex);

            return jv;
        }

        private static bool Equal(CliFbx.CVertex fv, JObject jv)
        {
            if(!float.Equals(jv["Pos"][0].Value<float>(), (float)fv.Position.X) ||
                !float.Equals(jv["Pos"][1].Value<float>(), (float)fv.Position.Y) ||
                !float.Equals(jv["Pos"][2].Value<float>(), (float)fv.Position.Z) ||
                !float.Equals(jv["Nor"][0].Value<float>(), (float)fv.Normal.X) ||
                !float.Equals(jv["Nor"][1].Value<float>(), (float)fv.Normal.Y) ||
                !float.Equals(jv["Nor"][2].Value<float>(), (float)fv.Normal.Z) ||
                !float.Equals(jv["UV"][0].Value<float>(), (float)fv.UV.X) ||
                !float.Equals(jv["UV"][1].Value<float>(), (float)fv.UV.Y))
            {
                return false;
            }

            if(fv.Weights.Count > 0)
            {
                if (jv["BI"].Value<int>() != fv.Weights[0].BlendingIndex)
                    return false;
            }

            return true;

        }

        private static JArray ConvertVector3(CliFbx.Vector3 v)
        {
            JArray jv = new JArray();
            jv.Add((float)v.X);
            jv.Add((float)v.Y);
            jv.Add((float)v.Z);
            return jv;
        }

        private static JArray ConvertVector2(CliFbx.Vector2 v)
        {
            JArray jv = new JArray();
            jv.Add((float)v.X);
            jv.Add((float)v.Y);
            return jv;
        }

        private static OpenTK.Matrix4 ConvertMatrix(CliFbx.Matrix4 sour)
        {
            OpenTK.Matrix4 dest = new OpenTK.Matrix4();

            dest.Row0 = new OpenTK.Vector4((float)sour.Row0.X,
                                           (float)sour.Row0.Y,
                                           (float)sour.Row0.Z,
                                           (float)sour.Row0.W);

            dest.Row1 = new OpenTK.Vector4((float)sour.Row1.X,
                                           (float)sour.Row1.Y,
                                           (float)sour.Row1.Z,
                                           (float)sour.Row1.W);

            dest.Row2 = new OpenTK.Vector4((float)sour.Row2.X,
                                           (float)sour.Row2.Y,
                                           (float)sour.Row2.Z,
                                           (float)sour.Row2.W);

            dest.Row3 = new OpenTK.Vector4((float)sour.Row3.X,
                                           (float)sour.Row3.Y,
                                           (float)sour.Row3.Z,
                                           (float)sour.Row3.W);

            return dest;
        }
        #endregion

        #region Variable

        private CliFbx.FbxData mData = null;
        private string mPath = "";

        #endregion
    }
}
