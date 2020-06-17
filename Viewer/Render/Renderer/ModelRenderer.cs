using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Viewer.Asset;

namespace Viewer.Render
{
    class CModelRenderer
    {
        public bool Init()
        {
            GL.GenBuffers(1, out mUBOModel);

            return true;
        }

        

        public void Render(CModel model, CCamera cam, CShaderSystem shader, bool transparent)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);

            if (model == null)
                return;

            CMesh mesh = model.Mesh;

            if (mesh.BindVBOs() == false)
                return;

            // start shader
            if(shader.UseProgram(SupportShader.Model) == true)
            {
                UniformModel(model);

                foreach (CSubset subset in mesh.Subsets)
                {

                    CMaterial material = null;
                    if(model.Materials.Count > 0 && model.Materials.ContainsKey(subset.Name))
                    {
                        material = model.Materials[subset.Name];
                    }

                    if(material == null)
                    {
                        if (transparent == true) // 재질이 없으면 불투명할 대만 그린다.
                            continue;

                        // Material Routine Func Uniform
                        int func_index = (int)FSMaterialRoutine.funcBasic;
                        GL.UniformSubroutines(ShaderType.FragmentShader, 1, ref func_index);
                    }
                    else
                    {
                        if (transparent != material.IsTransparent())
                            continue;

                        material.GLModeSetting();
                        material.Bind();
                        material.Uniform();
                    }

                    // Draw
                    int offset = subset.IndexOffset * sizeof(uint);
                    int elementCount = subset.TriangleCount * 3;

                    GL.DrawElements(PrimitiveType.Triangles, elementCount,
                                DrawElementsType.UnsignedInt, offset);

                }
            }
            shader.ReleaseProgram();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Blend);

            mesh.UnbindVBOs();

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.DepthTest);

        }

        private void UniformModel(CModel model)
        {
            UBOModelData ubo_model = new UBOModelData();
            ubo_model.ModelMatrix = Matrix4.Identity;

            GL.BindBuffer(BufferTarget.UniformBuffer, mUBOModel);
            GL.BufferData(BufferTarget.UniformBuffer, new IntPtr(UBOModelData.SizeInBytes), ref ubo_model,
                            BufferUsageHint.DynamicDraw);

            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, (int)UniformBinding.model_block, mUBOModel);

            //bool animated = true; // todo : 외부에서 읽어오자
            bool animated = false; // todo : 외부에서 읽어오자

            // Animated Routine Function Uniform
            int func_index = animated ?
                        (int)VSAnimatedRoutine.funcAnimated :
                        (int)VSAnimatedRoutine.funcDontAnimated;
            GL.UniformSubroutines(ShaderType.VertexShader, 1, ref func_index);

            // Animated Matrix Uniform
            if(animated)
            {
                unsafe
                {
                    fixed(Matrix4* pmat = model.Anim.AnimatedMatrix.ToArray())
                    {
                        // row 기준이 아닌 colum기준으로 matrix를 바구면 3번재 인자를 false로...
                        GL.UniformMatrix4((int)Location.bone_matrices, model.Anim.AnimatedMatrix.Count,
                            true, ref pmat->Row0.X);
                    }
                }
            }
        }

        private struct UBOModelData
        {
            public Matrix4 ModelMatrix;
            public static int SizeInBytes = 16 * 4;
        }
        private enum VSAnimatedRoutine
        {
            funcAnimated = 1,
            funcDontAnimated = 2,
        }

        private enum FSMaterialRoutine
        {
            funcBasic = 1,
            funcDiffuse = 2,
            funcPhong = 3,
        }

        public enum UniformBinding
        {
            cam_trans_block = 0,
            model_block = 1,
            phong_mat_block = 2,
        }

        private enum Location
        {
            bone_matrices = 13,
        }

        private int mUBOModel = 0;
    }
}
