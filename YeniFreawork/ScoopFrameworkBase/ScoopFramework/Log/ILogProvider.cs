using System;
using ScoopFramework.Enums;

namespace ScoopFramework.Log
{
    public interface ILogProvider
    {
        /// <summary>
        /// Logs an error to the log file.
        /// </summary>
        /// <param name="origin">The origin of the error</param>
        /// <param name="message">The message</param>
        /// <param name="details">Optional error details</param>
        void Error(string origin, string message, System.Exception details = null);
        void Message(string url, Guid createdby, EnumTraceLogProviderMethod method, EnumTraceLogProviderDurum durum, string message, string messageDetail,Guid? logTableId);
    }
}
