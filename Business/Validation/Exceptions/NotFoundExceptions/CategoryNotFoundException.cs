namespace Business.Validation.Exceptions.NotFoundExceptions
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(int id)
            : base($"Category by the id of {id} not found") 
        { 
        }
    }
}