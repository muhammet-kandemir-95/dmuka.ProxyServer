using System;
using System.Collections.Generic;
using System.Threading;

namespace dmuka.Semaphore
{
    public class ActionQueue : IDisposable
    {
        #region Constructors
        public ActionQueue(int coreCount)
        {
            this.CoreCount = coreCount;
            if (this.CoreCount > 1)
            {
                for (int i = 0; i < this.CoreCount; i++)
                {
                    this._threads.Add(new Thread(() =>
                    {
                        while (this._disposed == false)
                        {
#if DEBUG
                            runAction();
#endif
#if DEBUG == false
                            try
                            {
                                runAction();
                            }
                            catch 
                            { }
#endif
                        }
                    }));
                }
            }
        }
        #endregion

        #region Variables
        /// <summary>
        /// Thread count
        /// </summary>
        public int CoreCount { get; private set; }

        /// <summary>
        /// Queue of action
        /// </summary>
        Queue<Action> _actions = new Queue<Action>();

        bool _disposed = false;
        /// <summary>
        /// Was class dispose?
        /// </summary>
        public bool Disposed
        {
            get
            {
                return this._disposed;
            }
        }

        bool _started = false;
        /// <summary>
        /// Was queue start?
        /// </summary>
        public bool Started
        {
            get
            {
                return this._started;
            }
        }

        /// <summary>
        /// Thread's list. This list count is core count.
        /// </summary>
        List<Thread> _threads = new List<Thread>();
        #endregion

        #region Methods
        /// <summary>
        /// This function each action on queue
        /// </summary>
        private void runAction()
        {
            Action firstAction = null;
            lock (this._actions)
                if (this._actions.Count > 0)
                    firstAction = this._actions.Dequeue();

            if (firstAction != null)
                firstAction();
            else
                Thread.Sleep(1);
        }

        /// <summary>
        /// Run all cores(Threads)
        /// </summary>
        public void Start()
        {
            if (this._disposed == true)
                throw new ObjectDisposedException("ActionQueue");
            if (this._started == true)
                throw new Exception("State is open!");

            lock (this._threads)
            {
                foreach (var thread in this._threads)
                    thread.Start();
            }
        }

        /// <summary>
        /// This function is for add new action to queue.
        /// </summary>
        /// <param name="action">Sync Action</param>
        public void AddAction(Action action)
        {
            if (this._disposed == true)
                throw new ObjectDisposedException("ActionQueue");

            if (this.CoreCount > 1)
            {
                lock (this._actions)
                    this._actions.Enqueue(action);
            }
            else
                action();
        }

        public void Dispose()
        {
            if (this._disposed == true)
                throw new ObjectDisposedException("ActionQueue");

            this._disposed = true;
            lock (this._threads)
            {
                foreach (var thread in this._threads)
                    thread.Abort();
            }
            lock (this._actions)
            {
                this._actions.Clear();
            }
        }
        #endregion
    }
}
