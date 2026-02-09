namespace BuildingBlocks.Application;

public class InvalidCommandException(List<string> errors) : Exception(errors.FirstOrDefault())
{
    public List<string> Errors { get; } = errors;
}