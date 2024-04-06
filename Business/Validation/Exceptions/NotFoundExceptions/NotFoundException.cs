namespace Business.Validation.Exceptions.NotFoundExceptions
{
    public abstract class NotFoundException : MarketException
    {
        protected NotFoundException(string message)
            : base(message)
        {
        }
    }
}