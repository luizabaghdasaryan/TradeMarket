namespace Business.Validation.Exceptions.NotFoundExceptions
{
    public sealed class ReceiptNotFoundException : NotFoundException
    {
        public ReceiptNotFoundException(int id)
            : base($"Receipt by the id of {id} not found") 
        { 
        }
    }
}