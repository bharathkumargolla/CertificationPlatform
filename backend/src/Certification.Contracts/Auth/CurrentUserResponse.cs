namespace Certification.Contracts.Auth;

public sealed class CurrentUserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public IReadOnlyCollection<string> Roles { get; init; } = [];
}
