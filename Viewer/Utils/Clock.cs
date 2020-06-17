using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics; // Stopwatch
using System.Windows.Forms; // Timer

namespace Viewer.Utils
{
    class CClock
    {
        #region Public Method
        public void Init(Action updateMethod, int fps)
        {
            UpdateMethod += updateMethod;
            mTimer.Interval = 1000 / fps;
            mTimer.Tick += new EventHandler(TimerTick);
        }

        public void ChangeUpdateMethod(Action updateMethod)
        {
            UpdateMethod += updateMethod;
        }

        public void Stop()
        {
            mTimer.Stop();
        }

        public void Start()
        {
            mTimer.Start();
        }
        #endregion

        #region Private Method
        private void TimerTick(object sender, EventArgs e)
        {
            mSW.Stop();
            mDeltaTime = mSW.ElapsedMilliseconds;
            mAccTime += mDeltaTime;
            mSW.Reset();
            mSW.Start();

            if(UpdateMethod != null)
            {
                UpdateMethod();
            }
        }
        #endregion

        #region Variable
        public double Delta
        {
            // ms
            get { return mDeltaTime; }
        }
        public int AccTime
        {
            // ms
            get { return (int)mAccTime; }
        }
        public event Action UpdateMethod;
        
        private double mAccTime = 0.0;
        private double mDeltaTime = 0.0;
        private Stopwatch mSW = new Stopwatch();
        private Timer mTimer = new Timer();
        #endregion
    }
}
