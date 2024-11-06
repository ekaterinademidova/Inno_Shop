using UsersApplication.Products.Commands.DeleteProduct;
using UsersDomain.Errors;

namespace UsersApplication.Tests.CommandHandlers.Products.DeleteProduct
{
    public class DeleteProductCommandValidatorTests :
        ValidationTestBase<DeleteProductCommand, DeleteProductResult, DeleteProductCommandValidator>
    {
        public DeleteProductCommandValidatorTests() : base(new DeleteProductCommandValidator())
        {
        }

        [Fact]
        public void Should_Be_Valid()
        {
            var command = CreateTestCommand();

            ShouldBeValid(command);
        }

        [Fact]
        public void Should_Be_Invalid_For_Empty_Id()
        {
            var command = CreateTestCommand(id: Guid.Empty);

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.Product.EmptyId,
                "Product id may not be empty");
        }

        private static DeleteProductCommand CreateTestCommand(
            Guid? id = null)
        {
            return new DeleteProductCommand(id ?? Guid.NewGuid());
        }
    }
}
