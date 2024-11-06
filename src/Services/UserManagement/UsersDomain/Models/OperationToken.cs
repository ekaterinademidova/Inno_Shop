using System.ComponentModel;
using UsersDomain.Enums.Extensions;

namespace UsersDomain.Models
{
    public class OperationToken : Entity<OperationTokenId>
    {
        public UserId UserId { get; set; } = default!;
        public Guid Code { get; set; } = default!;
        public OperationType OperationType { get; set; } = OperationType.EmailConfirmation;
        public DateTime Expiration { get; set; } = default!;
        public bool IsValid() => DateTime.UtcNow <= Expiration;

        public static OperationToken Create(UserId userId, OperationType operationType, int expirationMinutes = 60)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId.Value.ToString());
            if (!EnumTraits<OperationType>.IsValid((operationType)))
                throw new InvalidEnumArgumentException("The operation type \"" + operationType.ToString() + "\" does not exist.");
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(expirationMinutes);

            var token = new OperationToken
            {
                Id = OperationTokenId.Of(Guid.NewGuid())
            };

            token.SetUserId(userId);
            token.SetCode(Guid.NewGuid());
            token.SetOperationType(operationType);
            token.SetExpiration(DateTime.UtcNow.AddMinutes(expirationMinutes));

            return token;
        }
        public void SetUserId(UserId userId)
        {
            UserId = userId;
        }

        public void SetCode(Guid code)
        {
            Code = code;
        }

        public void SetOperationType(OperationType operationType)
        {
            OperationType = operationType;
        }

        public void SetExpiration(DateTime expiration)
        {
            Expiration = expiration;
        }
    }
}
