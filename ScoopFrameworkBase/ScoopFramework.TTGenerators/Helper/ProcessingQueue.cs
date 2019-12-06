using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ScoopFramework.TTGenerators.Helper
{
    class ProcessingQueueManager
    {
        static ProcessingQueueManager instance = new ProcessingQueueManager();
        public static ProcessingQueueManager Instance { get { return instance; } }
        ProcessingQueueManager()
        {

        }

        Dictionary<string, WeakReference> queues = new Dictionary<string, WeakReference>();
        internal void Register(IProcessingQueue queue)
        {
            lock (this)
                queues[queue.Name] = new WeakReference(queue);
        }
        public IProcessingQueue GetQueue(string name)
        {
            lock (this)
            {
                WeakReference wr = null;

                if (queues.TryGetValue(name, out wr))
                {
                    if (wr.IsAlive)
                        return wr.Target as IProcessingQueue;
                    queues.Remove(name);
                }
                return null;
            }
        }
        void PurgeReferences()
        {
            lock (this)
                foreach (var item in queues.Keys.ToArray())
                {
                    if (!queues[item].IsAlive) queues.Remove(item);
                }
        }

        internal void Unregister(IProcessingQueue processingQueue)
        {
            lock (this)
                queues.Remove(processingQueue.Name);
        }
    }
    public interface IProcessingQueue
    {
        string Name { get; }
        int RemainingCount { get; }
        bool IsProcessing { get; }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class ProcessingQueue<TItem>
    {
        public static ProcessingQueue<TItem> operator +(ProcessingQueue<TItem> c1, TItem c2)
        {
            c1.Enqueue(c2);
            return c1;
        }

        private BlockingPipe<TItem> _queue;

      //  private BlockingCollection<TItem> _queue;

        private Thread _thread;

        private CancellationTokenSource _cancelSource;
        protected CancellationToken CancellationToken { get { return _cancelSource.Token; } }



        public bool IsProcessing { get; private set; }

        private int _processedItemCount;

        public int ProcessedItemCount
        {
            get
            {
                return _processedItemCount;
            }
        }


        public int ItemCount
        {
            get
            {
                return _queue.Count;
            }
        }

        int _maxDOP = 1;

        public int MaxDegreeofParallelizm
        {
            get
            {
                return _maxDOP;
            }
            set
            {
                if (_maxDOP < 1 || _maxDOP > Environment.ProcessorCount)
                {
                    throw new ArgumentException("Invalid MaxDegreeofParallelizm value");
                }

                _maxDOP = value;
            }
        }

        private readonly AutoResetEvent _completeHandle = new AutoResetEvent(false);

        public WaitHandle CompleteHandle
        {
            get { return _completeHandle; }
        }

        public void Complete()
        {
            if (!_queue.IsCompleted)
            {
                _queue.CompleteAdding();
            }
        }

        public ProcessingQueue(int boundedcapacity = int.MaxValue)
            : this(boundedcapacity, CancellationToken.None)
        {
        }

        public ProcessingQueue(int boundedcapacity, CancellationToken token)
        {
             _queue = new BlockingPipe<TItem>(boundedcapacity);
            //_queue = new BlockingCollection<TItem>(boundedcapacity);

            _cancelSource = CancellationTokenSource.CreateLinkedTokenSource(token);

            _thread = new Thread(ProcessLoop) { IsBackground = true };
            _thread.Start();
        }

        private void ProcessLoop(object state)
        {
            //Log.Info("Queue is started.");
            IsProcessing = true;
            try
            {
                ProcessQueue();
            }
            catch (OperationCanceledException)
            {
                //   Log.Info("Queue Canceled");
            }
            catch (ThreadAbortException)
            {
                //     Log.Info("Queue Aborted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //     Log.Exception(ex);
            }
            finally
            {
                IsProcessing = false;
                OnExit(_queue.GetItems());
            }
        }

        ~ProcessingQueue()
        {
        }

        private void ProcessQueue()
        {
            while (true)
            {
                var list = _queue.GetConsumingEnumerable(CancellationToken);

                if (MaxDegreeofParallelizm == 1)
                {
                    foreach (var item in list)
                    {
                        ProcessInternal(item);
                    }
                }
                else
                {

                    list.RunParallel()
                        .AsLongRunning()
                        .WithToken(CancellationToken)
                        .WithMaxDegreeofParallelizm(MaxDegreeofParallelizm)
                        .Run(ProcessInternal);
                }

                _queue = new BlockingPipe<TItem>(_queue.MaxCapacity);
             //   _queue = new BlockingCollection<TItem>(_queue.BoundedCapacity);
                _completeHandle.Set();
            }
        }

        private void ProcessInternal(TItem item)
        {
            Interlocked.Add(ref _processedItemCount, 1);
            ProcessItem(item);

        }

        protected virtual void OnExit(IEnumerable<TItem> leftitems)
        {

        }

        public void Enqueue(TItem msg)
        {
            _queue.Add(msg, CancellationToken);
        }

        public bool Enqueue(TItem msg, int timeout)
        {
            return _queue.TryAdd(msg, timeout, CancellationToken);
        }

        protected void Enqueue(TItem[] msg)
        {
            foreach (var item in msg)
            {
                _queue.Add(item, CancellationToken);
            }
        }

        protected abstract void ProcessItem(TItem item);

        public void Dispose()
        {
            if (_thread != null)
            {
                if (_thread.IsAlive)
                {
                    _cancelSource.Cancel();
                }
                IsProcessing = false;
                _thread = null;
            }
        }
    }

    public class BlockingPipe<TItem>
    {
        public static BlockingPipe<TItem> operator +(BlockingPipe<TItem> c1, TItem c2)
        {
            c1.Add(c2);
            return c1;
        }

        private readonly Queue<TItem> _queue;
        private volatile int _queueCount = 0;
        private readonly int _maxCount = Int32.MaxValue;

        private bool _isComplete;

        public bool IsCompleted
        {
            get { return _isComplete; }
        }

        public int MaxCapacity
        {
            get { return _maxCount; }
        }

        public int Count
        {
            get { return _queue.Count; }
        }

        public BlockingPipe(int boundedcapacity)
        {
            _maxCount = boundedcapacity;
            _queue = new Queue<TItem>(100);
        }

        public void Add(TItem msg)
        {
            Add(msg, CancellationToken.None);
        }

        public void Add(TItem msg, CancellationToken cancellationtoken)
        {
            TryAdd(msg, Timeout.Infinite, cancellationtoken);
        }

        internal bool TryAdd(TItem msg, int timeout, CancellationToken cancellationToken)
        {
            if (_isComplete)
            {
                throw new InvalidOperationException("Queue Complete");
            }

            cancellationToken.ThrowIfCancellationRequested();

            Monitor.Enter(_queue);

            var istimeout = false;

            while (_queueCount >= _maxCount && !istimeout)
            {
                cancellationToken.ThrowIfCancellationRequested();
                istimeout = !Monitor.Wait(_queue, timeout);
            }

            if (!istimeout)
            {
                _queueCount++;
                _queue.Enqueue(msg);
            }

            Monitor.Pulse(_queue);
            Monitor.Exit(_queue);

            return !istimeout;
        }

        internal void CompleteAdding()
        {
            lock (_queue)
            {
                _isComplete = true;
                Monitor.Pulse(_queue);
            }
        }
        public IEnumerable<TItem> GetItems()
        {
            return _queue;
        }
        internal IEnumerable<TItem> GetConsumingEnumerable(CancellationToken cancellationToken)
        {
            while (true)
            {
                //   if (!Monitor.IsEntered(_queue))
                {
                    Monitor.Enter(_queue);
                }

                while (_queueCount == 0)
                {
                    if (_isComplete)
                    {
                        yield break;
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    Monitor.Wait(_queue, 100);
                }

                var item = _queue.Dequeue();
                _queueCount--;

                Monitor.Pulse(_queue);
                Monitor.Exit(_queue);

                cancellationToken.ThrowIfCancellationRequested();

                yield return item;
            }
        }
    }


    public class Dispatcher : ProcessingQueue<Action>
    {

        public Dispatcher()
        {

        }
        protected override void ProcessItem(Action item)
        {
            item();
        }
    }
    public interface ITask
    {
        CancellationToken CancellationToken { get; }

    }


}
