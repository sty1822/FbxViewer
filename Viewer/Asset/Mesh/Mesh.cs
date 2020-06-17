using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Newtonsoft.Json.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using Viewer.Files;

namespace Viewer.Asset
{
    class CMesh : CAsset
    {
        #region Public Method
        public override bool Serialize(ref object writer)
        {
            if (writer is BinaryWriter)
            {
                BinaryWriter bw = writer as BinaryWriter;

                bw.Write(VertexCount);
                foreach (CVertex v in Vertices)
                    v.Serialize(ref writer);

                bw.Write(IndexCount);
                foreach (int index in Indices)
                    bw.Write(index);

                bw.Write(Subsets.Count);
                foreach (CSubset subset in Subsets)
                    subset.Serialize(ref writer);

            }
            else
                return false;

            return true;
        }

        public override bool Deserialize(object data)
        {
            if (data is BinaryReader)
            {
                BinaryReader br = data as BinaryReader;

                int vc = br.ReadInt32();
                for (int i = 0; i < vc; ++i)
                {
                    CVertex v = new CVertex();
                    if (v.Deserialize(br) == true)
                        Vertices.Add(v);
                }

                int ic = br.ReadInt32();
                for (int i = 0; i < ic; ++i)
                {
                    int index = br.ReadInt32();
                    Indices.Add(index);
                }

                int sc = br.ReadInt32();
                for (int i = 0; i < sc; ++i)
                {
                    CSubset subset = new CSubset();
                    if (subset.Deserialize(br) == true)
                        Subsets.Add(subset);
                }

            }
            else if (data is JObject)
            {
                JObject jmesh = data as JObject;
                foreach(JObject jv in jmesh["Vertices"])
                {
                    CVertex v = new CVertex();
                    v.Deserialize(jv);
                    Vertices.Add(v);
                }

                foreach(int index in jmesh["Indices"])
                {
                    Indices.Add(index);
                }

                foreach(JObject js in jmesh["Subsets"])
                {
                    CSubset subset = new CSubset();
                    subset.Deserialize(js);
                    Subsets.Add(subset);
                }
            }
            else
                return false;

            return true;
        }

        public CMesh()
        {
            FileInfo = null;

            Subsets = new List<CSubset>();
            Vertices = new List<CVertex>();
            Indices = new List<int>();
        }

        public override bool Init()
        {
            CreateVBO();

            TriangleCount = 0;
            foreach (CSubset subset in Subsets)
                TriangleCount += subset.TriangleCount;

            return base.Init();
        }

        public bool HaveSubset(string subsetName)
        {
            if (Subsets.Count <= 0)
                return false;

            foreach(CSubset subset in Subsets)
            {
                if (subset.Name.ToLower() == subsetName.ToLower())
                    return true;
            }

            return false;
        }

        public bool BindVBOs()
        {
            // vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBOHandle);

            // position
            GL.VertexAttribPointer((int)Location.Position, 3, VertexAttribPointerType.Float, false,
                                    CVertex.SizeInBytes, CVertex.OffsetAtPosition);
            GL.EnableVertexAttribArray((int)Location.Position);

            // normal
            GL.VertexAttribPointer((int)Location.Normal, 3, VertexAttribPointerType.Float, false,
                                    CVertex.SizeInBytes, CVertex.OffsetAtNormal);
            GL.EnableVertexAttribArray((int)Location.Normal);

            // texcoord
            GL.VertexAttribPointer((int)Location.Texcoord, 2, VertexAttribPointerType.Float, false,
                                    CVertex.SizeInBytes, CVertex.OffsetAtTexcoord);
            GL.EnableVertexAttribArray((int)Location.Texcoord);

            // bone index (주의: Int타입의 속성을 선택할 때는 VertexAttribPointer()가 아닌
            // VertexAttribIPointer()를 사용할 것!
            GL.VertexAttribIPointer((int)Location.BoneId, 1, VertexAttribIntegerType.Int,
                                    CVertex.SizeInBytes, new IntPtr(CVertex.OffsetAtBoneIndex));
            GL.EnableVertexAttribArray((int)Location.BoneId);

            // index
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mIndexBufferHandle);

            return true;
        }

        public void UnbindVBOs()
        {
            GL.DisableVertexAttribArray((int)Location.Position);
            GL.DisableVertexAttribArray((int)Location.Normal);
            GL.DisableVertexAttribArray((int)Location.Texcoord);
            GL.DisableVertexAttribArray((int)Location.BoneId);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Progress(Matrix4 mat)
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region Private Method
        private void CreateVBO()
        {
            if (VertexCount <= 0)
                return;

            // vbo
            GL.GenBuffers(1, out mVBOHandle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBOHandle);
            GL.BufferData(BufferTarget.ArrayBuffer,
                            new IntPtr(VertexCount * CVertex.SizeInBytes),
                            Vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // indices
            GL.GenBuffers(1, out mIndexBufferHandle);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mIndexBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                            new IntPtr(IndexCount * sizeof(uint)),
                            Indices.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        #endregion

        #region Variable
        public override CFileInfo FileInfo { get; set; }
        public override string Extension { get { return CFileSystem.Inst.MeshExt.ToString(); } }

        public int VertexCount { get { return Vertices.Count; } }
        public int IndexCount { get { return Indices.Count; } }

        public int TriangleCount { get; private set; }
        public List<CSubset> Subsets { get; private set; }
        public List<CVertex> Vertices { get; private set; }
        public List<int> Indices { get; private set; }

        private int mIndexBufferHandle = 0;
        private int mVBOHandle = 0;

        private enum Location
        {
            Position = 0,   // layout (location = 0) in vec3 position;
            Normal = 1,     // layout (location = 1) in vec3 normal;
            Texcoord = 2,   // layout (location = 2) in vec2 texcoord;
            BoneId = 3,     // layout (location = 3) in int bone_id
        }
        #endregion
    }
}
