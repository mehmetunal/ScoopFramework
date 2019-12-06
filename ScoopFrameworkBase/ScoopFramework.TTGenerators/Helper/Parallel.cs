using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ScoopFramework.TTGenerators.Helper
{
    public static class ParallelExtentions
    {
        static public Parallel<T> RunParallel<T>(this IEnumerable<T> ie)
        {
            return new Parallel<T>(ie);
        }

    }
    public class Parallel<T>
    {
       
        private IEnumerable<T> _items;

        private CancellationToken _cancelToken;

        private bool _islongrunnig;

        private int _maxDOP;
        internal Parallel(IEnumerable<T> items)
        {
            _cancelToken = CancellationToken.None;
            _items = items;
            _maxDOP = Environment.ProcessorCount;
        }
        
        public Parallel<T> WithToken(CancellationToken token)
        {
            _cancelToken = token;
            return this;
        }
        public Parallel<T> AsLongRunning()
        {
            _islongrunnig = true;
            return this;
        }
        public Parallel<T> WithMaxDegreeofParallelizm(int max)
        {
            _maxDOP = max;
            return this;
        }

        public void Run( Action<T> action)
        {
            var c = new CountdownEvent(_maxDOP);

            var ie = _items.GetEnumerator();
            for (int i = 0; i < _maxDOP; i++)
            {
                Task.Factory.StartNew(a =>
                {
                    try
                    {
                        T current = default(T);
                        while (true)
                        {
                            lock (ie)
                            {
                                if (!ie.MoveNext())
                                    break;

                                current = ie.Current;
                            }
                            action(current);
                        }
                    }
                    finally
                    {

                        c.Signal(1);
                    }
                }, _islongrunnig ? TaskCreationOptions.LongRunning:TaskCreationOptions.None,_cancelToken);
            };

            c.Wait();
        }
        public IEnumerable<OUT> Select<OUT>( Func<T, OUT> action)
        {
            var c = new CountdownEvent(_maxDOP);
            var ret = new System.Collections.Concurrent.ConcurrentStack<OUT>();
            var ie = _items.GetEnumerator();
            for (int i = 0; i < _maxDOP; i++)
            {
                 Task.Factory.StartNew(delegate 
                {
                    try
                    {
                        T current = default(T);
                        while (true)
                        {
                            lock (ie)
                            {
                                if (!ie.MoveNext())
                                    break;

                                current = ie.Current;
                            }
                            ret.Push(action(current));
                        }
                    }
                    finally
                    {
                        c.Signal(1);
                    }
                }, _islongrunnig ? TaskCreationOptions.LongRunning : TaskCreationOptions.None);
            };

            c.Wait();
            return ret;
        }

    }
}
