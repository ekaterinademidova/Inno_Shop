using System.ComponentModel;
using UsersDomain.Enums.Extensions;

namespace UsersDomain.Models
{
    public class OperationToken : Entity<OperationTokenId>
    {
        public UserId UserId { get; private set; } = default!;
        public Guid Code { get; private set; } = default!;
        public OperationType OperationType { get; private set; } = OperationType.EmailConfirmation;
        public DateTime Expiration { get; private set; } = default!;
        public bool IsValid() => DateTime.UtcNow <= Expiration;
        private OperationToken(UserId userId, Guid code, OperationType operationType, DateTime expiration)
        {
            Id = OperationTokenId.Of(Guid.NewGuid());
            UserId = userId;
            Code = code;
            OperationType = operationType;
            Expiration = expiration;
        }

        public static OperationToken Create(UserId userId, OperationType operationType, int expirationMinutes)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(expirationMinutes);
            if (!EnumTraits<OperationType>.IsValid((operationType)))
                throw new InvalidEnumArgumentException("The operation type \"" + operationType.ToString() + "\" does not exist.");

            var code = Guid.NewGuid();
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);
            return new OperationToken(userId, code, operationType, expiration);
        }        
    }
}
