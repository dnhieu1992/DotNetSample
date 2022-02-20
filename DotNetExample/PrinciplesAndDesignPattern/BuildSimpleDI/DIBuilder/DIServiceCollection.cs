namespace BuildSimpleDI.DIBuilder
{
    public class DIServiceCollection
    {
        private IList<DIServiceDescriptor> serviceDescriptors = new List<DIServiceDescriptor>();
        public void AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface
        {
            serviceDescriptors.Add(new DIServiceDescriptor(typeof(TInterface), typeof(TImplementation), DILifetime.Singleton));
            GetService<TInterface>();
        }

        public void AddTransient<TInterface, TImplementation>() where TImplementation : TInterface
        {
            serviceDescriptors.Add(new DIServiceDescriptor(typeof(TInterface), typeof(TImplementation), DILifetime.Transient));
            GetService<TInterface>();
        }

        public void AddScope<TInterface, TImplementation>() where TImplementation : TInterface
        {
            serviceDescriptors.Add(new DIServiceDescriptor(typeof(TInterface), typeof(TImplementation), DILifetime.Scoped));
            GetService<TInterface>();
        }

        public T GetService<T>()
        {
            return (T)Create(typeof(T));
        }

        private object Create(Type type)
        {
            var serviceDescriptor = serviceDescriptors.FirstOrDefault(serviceDescriptor => serviceDescriptor.TInterfaceType == type);

            if (serviceDescriptor == null)
            {
                throw new Exception($"Service of type {type.Name} isn't registered");
            }

            var resolvedType = serviceDescriptor.ImplementationType ?? serviceDescriptor.TInterfaceType;

            if (serviceDescriptor.Implementation != null)
            {
                return serviceDescriptor.Implementation;
            }

            if (resolvedType.IsAbstract || resolvedType.IsInterface)
            {
                throw new Exception("Cannot create a instant of abstract classses or interfaces.");
            }

            //Get default constructor
            var ctor = resolvedType.GetConstructors()[0];
            //get paramters 
            var defaultParams = ctor.GetParameters();
            var parameters = defaultParams.Select(param => Create(param.ParameterType)).ToArray();
            var implementation = Activator.CreateInstance(resolvedType, parameters);
            if (serviceDescriptor.Lifetime == DILifetime.Singleton)
            {
                serviceDescriptor.Implementation = implementation;
            }

            return implementation;
        }
    }
}
