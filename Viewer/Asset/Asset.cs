using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Viewer.Files;
using OpenTK;

namespace Viewer.Asset
{
    abstract class CAsset : ISerializable, IFileable
    {
        #region Public Method
        public virtual bool Serialize(ref object writer)
        {
            return false;
        }
        public virtual bool Deserialize(object data)
        {
            return true;
        }

        public abstract void Progress(Matrix4 mat);

        
        public virtual bool Init()
        {
            return true;
        }
        #endregion

        #region Variable
        public abstract CFileInfo FileInfo { get; set; }
        public abstract string Extension { get; }
        #endregion

    }
}
