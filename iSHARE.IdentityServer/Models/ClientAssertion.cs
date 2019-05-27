﻿using System;

namespace iSHARE.IdentityServer.Models
{
    public class ClientAssertion
    {
        public ClientAssertion(string subject, string audience) : this()
        {
            Subject = subject;
            Issuer = subject;
            Audience = audience;
        }

        public ClientAssertion()
        {
            JwtId = Guid.NewGuid().ToString("N");
            IssuedAt = DateTime.UtcNow;
            Expiration = DateTime.UtcNow.AddSeconds(30);
        }

        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string JwtId { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime Expiration { get; set; }
    }
}
