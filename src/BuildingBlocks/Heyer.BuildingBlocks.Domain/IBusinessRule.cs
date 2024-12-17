using FluentResults;

namespace Heyer.BuildingBlocks.Domain;

public interface IBusinessRule {
    Result Challenge();
}