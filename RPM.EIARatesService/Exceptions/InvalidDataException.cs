using System;

namespace RPM.EIARatesService.Exceptions
{
    /// <summary>
    /// Custom exception for handling errors
    /// </summary>
    public class RPMException : Exception
    {
        public RPMException()
        {
        }

        public RPMException(string message)
            : base(message)
        {
        }

        public RPMException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
