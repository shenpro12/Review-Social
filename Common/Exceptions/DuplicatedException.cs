using System;

namespace review.Common.Exceptions
{
    public class DuplicatedException : Exception
    {
        public DuplicatedException()
        {
        }

        public DuplicatedException(string message) : base(message)
        {
        }

        public DuplicatedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}