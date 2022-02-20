namespace PrinciplesAndDesignPatternExample
{
    public class ExampleService : IExampleService
    {
        public Guid GenerateId()
        {
            return Guid.NewGuid();
        }
    }
}
