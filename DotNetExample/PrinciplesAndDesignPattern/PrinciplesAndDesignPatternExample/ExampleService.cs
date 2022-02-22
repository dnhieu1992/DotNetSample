namespace PrinciplesAndDesignPatternExample
{
    public class ExampleService : IExampleService
    {
        public Guid GenerateId()
        {
            return Guid.NewGuid();
        }

        public void CallSample(List<ISample> samples)
        {
            samples.ForEach(x => { });
        }
    }
}
