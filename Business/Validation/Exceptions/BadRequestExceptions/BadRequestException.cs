namespace Business.Validation.Exceptions.BadRequestExceptions
{
    public abstract class BadRequestException : MarketException
    {
        protected BadRequestException(string message)
            : base(message)
        {
        }
    }
}