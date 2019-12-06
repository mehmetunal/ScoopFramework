using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScoopFramework.TTGenerators.Helper
{
    public abstract class LongTask : ILongTask
    {
        public static LongTask EmptyTask = new NoneTask();

        [ThreadStatic]
        static LongTask _current;
        public static LongTask Current
        {
            get { return _current ?? EmptyTask; }
        }
        public static LongTask Start(Action<LongTask> task)
        {
            return Start(task, CancellationToken.None);
        }
        public static LongTask Start(Action<LongTask> task,CancellationToken token)
        {
            return new ActionTask(task, token);
        }

        class NoneTask : LongTask
        {
            protected override void RunTask()
            {
            }
        }
        public LongTask()
        {

        }

        private string _status;

        private double _per;
        
        private Task _task;
        
        private CancellationTokenSource _cancelsource;

        private CancellationToken _cancelToken;
            
        public LongTask(CancellationToken canceltoken)
        {
            _cancelToken = canceltoken;
        }
        public double Percentage
        {
            get { return _per; }
            set
            {
                if (_per != value)
                {
                    _per = value;
                    OnNotifyPropertyChanged("Percentage");
                }
            }
        }

        protected void ThrowIsCanceled()
        {
            if (_cancelsource != null)
            {
                _cancelsource.Token.ThrowIfCancellationRequested();
            }
                
        }

        protected bool IsCancellationRequested
        {
            get
            {
                return _cancelsource != null && _cancelsource.IsCancellationRequested;
            }
        }

        public IAsyncResult WaitHandler { get { return _task; } }

        

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;

                    OnNotifyPropertyChanged("Status");
                }
            }
        }
        public void Run()
        {
            if (IsRunning)
                throw new Exception("Task is already running!!");
            _cancelsource = CancellationTokenSource.CreateLinkedTokenSource(_cancelToken);
            _task = new System.Threading.Tasks.Task(delegate
                {
                    try
                    {

                        _cancelsource.Token.ThrowIfCancellationRequested();
                        _current = this;
                        RunTask();
                    }
                    finally
                    {
                        OnNotifyPropertyChanged("WaitHandler");
                        OnNotifyPropertyChanged("IsRunning");
                        _task = null;
                    }
                }, _cancelsource.Token);
            _task.Start();

            OnNotifyPropertyChanged("WaitHandler");
            OnNotifyPropertyChanged("IsRunning");
        }

        class ActionTask:LongTask
        {
            Action<LongTask> _run;
            public ActionTask(Action<LongTask> run,CancellationToken token):base(token)
            {
                _run = run;
                Run();
            }
            protected override void RunTask()
            {
                _run(this);
            }
        }
        protected abstract void RunTask();
     

        protected void OnNotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(prop));
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        public bool IsRunning
        {
            get { return _task != null; }
        }

        

        public void Abort()
        {
            if (IsRunning)
            {
                _cancelsource.Cancel();
            }
        }


        public CancellationToken CancellationToken
        {
            get { return _cancelsource.Token; }
        }
    }

    
    public interface ILongTask : System.ComponentModel.INotifyPropertyChanged
    {
        double Percentage { get; }
        string Status { get; }
        bool IsRunning { get; }
        IAsyncResult WaitHandler { get; }
        void Abort();
        CancellationToken CancellationToken { get; }
    }

}
