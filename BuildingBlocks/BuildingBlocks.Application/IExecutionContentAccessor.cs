namespace BuildingBlocks.Application;

public interface IExecutionContentAccessor
{
   Guid UserId { get;  } 
   
   bool IsAvailable { get;  }
}