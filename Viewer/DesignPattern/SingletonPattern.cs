using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Viewer.DesignPattern
{
    abstract class CSingletonPattern<T> where T : class, new()
    {
        public void EnsureThis() { /* 명시적으로 초기화 되었습니다 */ }
        public abstract bool Init();
        public static T Inst
        {
            get
            {
                lock(padlock)
                {
                    if(mInstance == null)
                    {
                        mInstance = new T();
                    }

                    return mInstance;
                }
            }
        }

        private static T mInstance = null;
        private static readonly object padlock = new object();
    }
}
