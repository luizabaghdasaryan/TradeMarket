namespace Business.Validation.Exceptions.NotFoundExceptions
{
    public sealed class CustomerNotFoundException : NotFoundException
    {
        public CustomerNotFoundException(int id)
            : base($"Customer by the id of {id} not found") 
        { 
        }
    }
}