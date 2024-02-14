using CBRE.Localization;
using System;

namespace CBRE.Providers
{
    public class ProviderNotFoundException : ProviderException
    {
        public ProviderNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ProviderNotFoundException() : base(Local.LocalString("exception.provider_not_found"))
        {
        }

        public ProviderNotFoundException(string message) : base(message)
        {
        }
    }
}
