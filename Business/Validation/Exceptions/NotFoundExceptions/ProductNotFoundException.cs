namespace Business.Validation.Exceptions.NotFoundExceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id)
            : base($"Product by the id of {id} not found") 
        { 
        }
    }
}