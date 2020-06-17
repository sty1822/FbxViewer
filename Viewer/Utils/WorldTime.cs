using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Viewer.Utils
{
    class CWorldTime : DesignPattern.CSingletonPattern<CWorldTime>
    {

        #region Public Method
        public override bool Init()
        {
            return true;
        }

        public void StartTime(Action updateMethod, int fps)
        {
            UpdateMethod += updateMethod;
            mClock.Init(Update, fps);
            mClock.Start();
        }

        public void Update()
        {
            mRandom = new Random(AccTime);

            UpdateMethod();
        }

        public void Closing()
        {
            mClock.Stop();
        }

        public int GetRandom<T>()
        {
            return mRandom.Next();
        }

        public int GetRandom(int max)
        {
            return mRandom.Next(max);
        }

        public int GetRandom(int min, int max)
        {
            return mRandom.Next(min, max);
        }

        public float GetRandomFloat()
        {
            return (float)mRandom.NextDouble();
        }

        #endregion
        
        #region Variable
        public double Delta
        {
            get { return mClock.Delta; }
        }

        public int AccTime
        {
            get { return mClock.AccTime; }
        }

        private event Action UpdateMethod;
        private CClock mClock = new CClock();
        private System.Random mRandom = null;
        #endregion
    }
}
