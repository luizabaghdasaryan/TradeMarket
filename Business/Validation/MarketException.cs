using System;

namespace Business.Validation
{
    public abstract class MarketException : Exception
    {
        protected MarketException(string message)
            : base(message)
        {
        }
    }
}