﻿using System;
using System.Runtime.Serialization;

namespace iSHARE.IdentityServer.Delegation
{
    [Serializable]
    public class DelegationPolicyFormatException : Exception
    {
        public DelegationPolicyFormatException()
        {
        }

        public DelegationPolicyFormatException(string message) : base(message)
        {
        }

        public DelegationPolicyFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DelegationPolicyFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}