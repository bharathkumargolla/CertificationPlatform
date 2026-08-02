namespace Certification.Domain.Common;

public interface IAuditable
{
    DateTime CreatedAtUtc { get; }

    string? CreatedBy { get; }

    DateTime? ModifiedAtUtc { get; }

    string? ModifiedBy { get; }
}
