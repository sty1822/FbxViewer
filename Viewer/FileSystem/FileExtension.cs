using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viewer.Files
{
    class CFileExtension
    {
        public CFileExtension(string ext)
        {
            Ext = ext;
        }
        public bool Is(string ext)
        {
            if (ext == Ext)
                return true;

            return false;
        }

        public override string ToString()
        {
            return Ext;
        }

        public string Ext { get; private set; }

        private CFileExtension()
        {
            // 생성 불가
        }
    }

    /// <summary>
    /// 파티클 시스템 파일
    /// </summary>
    class CPsExtension : CFileExtension
    {
        public CPsExtension()
            : base(".ps") { }
    }

    /// <summary>
    /// Fbx 모델 파일
    /// </summary>
    class CFbxExtension : CFileExtension
    {
        public CFbxExtension()
            : base(".fbx") { }
    }

    /// <summary>
    /// Jpeg 파일
    /// </summary>
    class CJpgExtension : CFileExtension
    {
        public CJpgExtension()
            : base(".jpg") { }
    }

    /// <summary>
    /// Tga 파일
    /// </summary>
    class CTgaExtension : CFileExtension
    {
        public CTgaExtension()
            : base(".tga") { }
    }

    /// <summary>
    /// png 파일
    /// </summary>
    class CPngExtension : CFileExtension
    {
        public CPngExtension()
            : base(".png") { }
    }

    /// <summary>
    /// glsl vertex shader 파일
    /// </summary>
    class CVSExtension : CFileExtension
    {
        public CVSExtension()
            : base(".vs") { }
    }

    /// <summary>
    /// glsl fragment shader 파일
    /// </summary>
    class CFSExtension : CFileExtension
    {
        public CFSExtension()
            : base(".fs") { }
    }

    /// <summary>
    /// json 파일
    /// </summary>
    class CJsonExtension : CFileExtension
    {
        public CJsonExtension()
            : base(".json") { }
    }

    class CAnimExtension : CFileExtension
    {
        public CAnimExtension()
            : base(".anim") { }
    }

    class CGameObjectExtension : CFileExtension
    {
        public CGameObjectExtension()
            : base(".go") { }
    }

    class CMeshExtension : CFileExtension
    {
        public CMeshExtension()
            : base(".mesh") { }
    }

    class CModelExtension : CFileExtension
    {
        public CModelExtension()
            : base(".model") { }
    }
}
