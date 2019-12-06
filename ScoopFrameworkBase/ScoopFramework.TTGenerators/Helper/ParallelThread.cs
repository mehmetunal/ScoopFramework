using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoopFramework.TTGenerators.Helper
{
    public class ParallelThread<T>
    {
        public int ProcessedItems { get; set; }
        int _numOfParallel;
        Action<T> _act1;
        Action<T, object> _act2;
        public ParallelThread(IEnumerable<T> array, int numOfParallel, Action<T> act)
        {
            var ie = array.GetEnumerator();
            _act1 = act;
            _numOfParallel = numOfParallel;

            var tf = Task.Factory;
            var tasks = new Task[numOfParallel];
            for(int i = 0; i < numOfParallel; i++)
            {
                tasks[i] = tf.StartNew(delegate()
                {
                    Run(ie);
                });
            }

            while (!Task.WaitAll(tasks, 1000))
            {

            }
        }
        public ParallelThread(IEnumerable<T> array, int numOfParallel, object val, Action<T, object> act)
        {
            var ie = array.GetEnumerator();
            _act2 = act;
            _numOfParallel = numOfParallel;

            var tf = Task.Factory;
            var tasks = new Task[numOfParallel];
            for (int i = 0; i < numOfParallel; i++)
            {
                tasks[i] = tf.StartNew(delegate()
                {
                    Run2(ie, val);
                });
            }

            while (!Task.WaitAll(tasks, 1000))
            {

            }
        }

        public void Run(IEnumerator<T> ie)
        {
            while (true)
            {
                T item;
                lock (ie)
                {
                    if (!ie.MoveNext()) break;
                    item = ie.Current;
                    ProcessedItems++;
                }
                _act1(item);
            }
        }

        public void Run2(IEnumerator<T> ie, object obj)
        {
            while (true)
            {
                T item;
                lock (ie)
                {
                    if (!ie.MoveNext()) break;
                    item = ie.Current;
                    ProcessedItems++;
                }
                _act2(item, obj);
            }
        }
    }
}
