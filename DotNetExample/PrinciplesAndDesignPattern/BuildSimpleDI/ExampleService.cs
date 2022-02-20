// See https://aka.ms/new-console-template for more information
using BuildSimpleDI;

public class ExampleService : IExampleService
{
    private readonly ISayHelloService _sayHelloService;
    public ExampleService(ISayHelloService sayHelloService)
    {
        _sayHelloService = sayHelloService;
    }
    public void PrintToScreen()
    {
        _sayHelloService.SayHello("Hello world!");
    }
}