using System;
using System.Runtime.Serialization;

namespace OrganizationRepository.Exceptions
{
    [Serializable]
    internal class UserExceptions : Exception
    {
        public UserExceptions()
        {
        }

        public UserExceptions(string message) : base(message)
        {
        }

        public UserExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}