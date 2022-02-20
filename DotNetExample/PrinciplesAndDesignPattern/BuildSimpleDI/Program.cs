// See https://aka.ms/new-console-template for more information
using BuildSimpleDI;
using BuildSimpleDI.DIBuilder;

var services = new DIServiceCollection();
services.AddSingleton<ISayHelloService, SayHelloService>();
services.AddSingleton<IExampleService, ExampleService>();

var exampleService = services.GetService<IExampleService>();
exampleService.PrintToScreen();


