using ErrorOr;
using MediatR;
using Moq;
using UavPathOptimization.Application.Common.Authentication;
using UavPathOptimization.Application.Common.Persistence.User;
using UavPathOptimization.Application.UseCases.Authentication.Commands.Register;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Tests.Application.UseCases.Authentication.Queries.Register;

[TestClass]
public class RegisterCommandHandlerTests
{
    private Mock<IJwtTokenGenerator> _tokenGeneratorMock;
    private Mock<IMediator> _mediatorMock;
    private RegisterCommandHandler _handler;

    [TestInitialize]
    public void Initialize()
    {
        _tokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new RegisterCommandHandler(_tokenGeneratorMock.Object, _mediatorMock.Object);
    }

    [TestMethod]
    public async Task Handle_ValidRequest_ReturnsAuthenticationResult()
    {
        // Arrange
        var request = new RegisterCommand
        (
            "testuser",
            "testuser@example.com",
            "password"
        );

        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var token = "testtoken";

        _mediatorMock.Setup(mock => mock.Send(It.IsAny<AddUserToDbCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ErrorOr<Guid>());

        _tokenGeneratorMock
            .Setup(mock => mock.GenerateToken(It.IsAny<User>()))
            .Returns<User>(inputUser =>
            {
                // Verify that the input user matches the expected user object
                Assert.AreEqual(user.UserName, inputUser.UserName);
                Assert.AreEqual(user.Email, inputUser.Email);

                // Return the predefined token
                return token;
            });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.IsError);
        Assert.IsInstanceOfType(result.Value, typeof(AuthenticationResult));
        Assert.AreEqual(user.UserName, result.Value.User.UserName);
        Assert.AreEqual(user.Email, result.Value.User.Email);
        Assert.AreEqual(token, result.Value.Token);
    }
}
