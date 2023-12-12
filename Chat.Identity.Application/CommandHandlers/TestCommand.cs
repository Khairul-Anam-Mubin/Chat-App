using Chat.Framework.CQRS;
using Chat.Framework.Enums;
using Chat.Framework.Results;

namespace Chat.Identity.Application.CommandHandlers;

public class TestCommand : ICommand
{
    public string Value { get; set; }

    public TestCommand()
    {
        Value = "Test Command";
    }
}

public class TestCommandResponse
{
    public string Value { get; set; }

    public TestCommandResponse()
    {
        Value = "Test Response";
    }
}
    
public class TestCommandHandler : ICommandHandler<TestCommand, TestCommandResponse>
{
    public async Task<IResult<TestCommandResponse>> HandleAsync(TestCommand request)
    {
        await Task.CompletedTask;
        return new Result<TestCommandResponse>
        {
            Status = ResponseStatus.Success,
            Message = "Done",
            Response = new TestCommandResponse()
        };
    }
}