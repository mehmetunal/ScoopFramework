using System;

namespace ScoopFramework.Exception
{
    internal class NotImplementedInPasException : NotImplementedException
    {
        public string ReasonWhy { get; set; }
        public NotImplementedInPasException() { }

        public NotImplementedInPasException(string message)
            : base(message) { }

        public NotImplementedInPasException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }

    internal class PercolatorException : System.Exception
    {
        public PercolatorException() { }

        public PercolatorException(string message)
            : base(message) { }

        public PercolatorException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }

    internal sealed class PercolatorQueryExeption : PercolatorException
    {
        public string ScoopQuery { get; private set; }
        public PercolatorQueryExeption() { }

        public PercolatorQueryExeption(string message)
            : base(message) { }

        public PercolatorQueryExeption(string message, string query)
            : base(message)
        {
            ScoopQuery = query;
        }
    }
}
