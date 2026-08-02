namespace Certification.Domain.Common;

public interface ISoftDelete
{
    bool IsDeleted { get; }

    DateTime? DeletedAtUtc { get; }

    string? DeletedBy { get; }
}
