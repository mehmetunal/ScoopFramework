namespace ScoopFramework.Log
{
    public class Logger
    {
        public Logger()
        {
            LogProvider= new LocalLogProvider();
            TraceLogProvider = new TraceLogProvider();
        }
        public LocalLogProvider LogProvider { get; private set; }
        public static TraceLogProvider TraceLogProvider { get; set; }
    }
}
