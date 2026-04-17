using Application;
using Application.Contracts;
using Application.CQRS;
using Application.DTOs;

using FluentValidation.TestHelper;

using Moq;

using Xunit;

namespace AuthCQRSTests;

public class TenantRegistrationValidatorTests
{
    private readonly TenantRegistrationDtoValidator _validator = new();

    [Theory]
    [InlineData("valid-slug")]
    [InlineData("tenant-123")]
    public void Slug_ShouldNotHaveValidationError_WhenFormatIsValid(string slug)
    {
        var model = new TenantRegistrationDto
        (Name: "", Slug: slug, PlanId: 0, Password: "", Email: "", TenantDomain: "");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }

    [Theory]
    [InlineData("Invalid Slug")]  
    [InlineData("-invalid-")]       
    [InlineData("UpperCase")]      
    public void Slug_ShouldHaveValidationError_WhenFormatIsInvalid(string slug)
    {
        var model = new TenantRegistrationDto
        (Name: "", Slug: slug, PlanId: 0, Password: "", Email: "", TenantDomain: "");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Theory]
    [InlineData("test@domain.com")]
    public void Email_ShouldBeValid_WhenFormatIsCorrect(string email)
    {
        var model = new TenantRegistrationDto
        (Name: "", Slug: "", PlanId: 0, Password: "", Email: email, TenantDomain: "");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}

public class RegisterTenantHandlerTests
{
    private readonly Mock<ISaasServices> _serviceMock;
    private readonly RegisterTenantHandler _handler;

    public RegisterTenantHandlerTests()
    {
        _serviceMock = new Mock<ISaasServices> { DefaultValue = DefaultValue.Mock };
        _handler = new RegisterTenantHandler(_serviceMock.Object);
    }

    [Fact]
    public async Task Handle_RegisterTenantAdmin_ShouldCallUserServiceWithCorrectData()
    {
        var dto = new TenantRegistrationDto
(Name: "", Slug: "tenant-x", PlanId: 0, Password: "", Email: "admin@tenant.com", TenantDomain: "");
        var command = new RegisterTenantAdminCommand(dto);
        var expectedResponse = new ProvisioningStatusDto(Guid.NewGuid(), "In Progress", "Started");

        _serviceMock.Setup(s => s.UserService.RegisterTenantAdmin(dto))
                    .ReturnsAsync(expectedResponse);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedResponse, result);
        _serviceMock.Verify(s => s.UserService.RegisterTenantAdmin(dto), Times.Once);
    }

    [Fact]
    public async Task Handle_TenantCreatedEvent_ShouldTriggerMigration()
    {
        var notification = new TenantCreatedEvent(Guid.NewGuid(), "Server=localhost;Database=TenantDB;");

        await _handler.Handle(notification, CancellationToken.None);

        _serviceMock.Verify(s => s.DbMigrationService.MigrateTenantDatabaseAsync(notification.ConnectionString), Times.Once);
    }
}