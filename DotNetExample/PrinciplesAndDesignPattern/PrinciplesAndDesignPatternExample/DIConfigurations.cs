using PrinciplesAndDesignPatternExample.DIBuilder;

namespace PrinciplesAndDesignPatternExample
{
    public class DIConfigurations
    {
        public static void Config()
        {
            var services = new DIServiceCollection();
            services.AddSingleton<IExampleService, ExampleService>();

        } 
    }
}
