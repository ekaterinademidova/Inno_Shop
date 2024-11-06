using UsersApplication.Dtos;
using UsersApplication.Products.Commands.UpdateProduct;
using UsersDomain.Constants;
using UsersDomain.Errors;

namespace UsersApplication.Tests.CommandHandlers.Products.UpdateProduct
{
    public class UpdateProductCommandValidatorTests :
        ValidationTestBase<UpdateProductCommand, UpdateProductResult, UpdateProductCommandValidator>
    {
        public UpdateProductCommandValidatorTests() : base(new UpdateProductCommandValidator())
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

        [Fact]
        public void Should_Be_Invalid_For_Empty_Name()
        {
            var command = CreateTestCommand(name: "");


            var errors = new List<string>
            {
                DomainErrorCodes.Product.EmptyName,
                DomainErrorCodes.Product.ShortName
            };

            ShouldHaveExpectedErrors(command, errors.ToArray());
        }

        [Fact]
        public void Should_Be_Invalid_For_Short_Name()
        {
            var command = CreateTestCommand(name: new string('a', 3));

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.Product.ShortName,
                $"Name may not be shorter than {4} characters");
        }

        [Fact]
        public void Should_Be_Invalid_For_Long_Name()
        {
            var command = CreateTestCommand(name: new string('a', MaxLengths.Product.Name + 1));

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.Product.LongName,
                $"Name may not be longer than {MaxLengths.Product.Name} characters");
        }

        [Fact]
        public void Should_Be_Invalid_For_Empty_Image_File()
        {
            var command = CreateTestCommand(imageFile: "");

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.Product.EmptyImageFile,
                "ImageFile may not be empty");
        }

        [Fact]
        public void Should_Be_Invalid_For_Price_Less_Than_Zero()
        {
            var command = CreateTestCommand(price: 0);

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.Product.PriceLessThanZero,
                $"Price must be greater than 0");
        }

        private static UpdateProductCommand CreateTestCommand(
            Guid? id = null,
            string? name = null,
            string? description = null,
            string? imageFile = null,
            decimal? price = null,
            int? quantity = null)
        {
            return new UpdateProductCommand(new ProductDto
            {
                Id = id ?? Guid.NewGuid(),
                Name = name ?? "TestProductName",
                Description = description ?? "TestProductDescription",
                ImageFile = imageFile ?? "TestProductImageFile",
                Price = price ?? 20.50M,
                Quantity = quantity ?? 10
            });
        }
    }
}
