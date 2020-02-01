using System;

namespace Vouzamo.ERM.Common.Exceptions
{
    public class CustomException : Exception
    {
        public virtual string Code { get; }
        public virtual string Debug { get; }

        public CustomException() : base()
        {
            
        }

        public CustomException(string message) : base(message)
        {
            
        }

        public CustomException(string message, Exception inner) : base(message, inner)
        {

        }

        public CustomException(string code, string message, string debug = null) : this(message)
        {
            Code = code;
            Debug = debug;
        }

        public CustomException(string code, string message, Exception inner, string debug = null) : this(message, inner)
        {
            Code = code;
            Debug = debug;
        }
    }

    public static class CustomExceptions
    {
        public static CustomException UnknownError(Exception ex, string message = "Unknown error") => new CustomException("ERR_UNKNOWN", message, ex, ex.StackTrace);
        public static CustomException StorageError(string debug, Exception ex, string message = "Unknown error with storage provider") => new CustomException("ERR_STORAGE", message, ex, debug);
        public static CustomException GuidNotFoundError(Guid id, string type) => new CustomException("1002", $"Could not find {type} with id: '{id}'");
    }
}
