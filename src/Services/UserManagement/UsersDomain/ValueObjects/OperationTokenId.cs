namespace UsersDomain.ValueObjects
{
    public record OperationTokenId
    {
        public Guid Value { get; }
        private OperationTokenId(Guid value) => Value = value;
        public static OperationTokenId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("OperationTokenId cannot be empty.");
            }

            return new OperationTokenId(value);
        }
    }
}
