using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;

using Viewer.Files;

namespace Viewer.Asset
{
    class CAssetFactory
    {
        #region Public Method

        public static CAsset Load<T>(CFileInfo fi) where T : CAsset, new()
        {
            // 1. Create
            CAsset asset = new T();
            asset.FileInfo = fi;

            // 2. Load
            if(CFileSystem.Inst.LoadFile(asset) == false)
            {
                MessageBox.Show("Asset 로딩 실패");
                return asset;
            }

            // 3. Init
            if(asset.Init() == false)
            {
                MessageBox.Show("Asset 초기화 실패");
                return asset;
            }

            return asset;
        }

        public static CAsset Load<T>(string filename) where T : CAsset, new()
        {
            return Load<T>(CFileInfo.SetFileInfo(filename));
        }

        public static CAsset Load<T>(JObject jobj) where T : CAsset, new()
        {
            // 1. Create
            CAsset asset = new T();
            //미구현

            return asset;
        }

        public static CAsset Create<T>() where T : CAsset, new()
        {
            string name = string.Format("asset_new{0}", mAssetCounter++);
            return Create<T>(name);
        }

        public static CAsset Create<T>(string name) where T : CAsset, new()
        {
            // 1. Create
            CAsset asset = new T();

            string filename = string.Format(".\\asset\\{0}{1}", name, asset.Extension);
            asset.FileInfo = CFileInfo.SetFileInfo(filename);

            // 2. Init
            if(asset.Init() == false)
            {
                MessageBox.Show("Asset 초기화 실패");
                return asset;
            }

            return asset;
        }
        #endregion

        #region Variable
        private static int mAssetCounter = 0;
        #endregion
    }
}
