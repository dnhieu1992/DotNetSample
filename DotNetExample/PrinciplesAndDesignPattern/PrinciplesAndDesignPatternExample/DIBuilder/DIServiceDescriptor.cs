namespace PrinciplesAndDesignPatternExample.DIBuilder
{
    public class DIServiceDescriptor
    {
        public Type TInterfaceType { get; }
        public Type ImplementationType { get; }
        public DILifetime Lifetime { get; }
        public object Implementation { get; set; }

        public DIServiceDescriptor(Type interfaceType, Type implementationType, DILifetime lifetime)
        {
            this.TInterfaceType = interfaceType;
            this.ImplementationType = implementationType;
            this.Lifetime = lifetime;
        }

        public DIServiceDescriptor(Type implementationType, DILifetime lifetime)
        {
            this.ImplementationType = implementationType;
            this.Lifetime = lifetime;
        }
    }
}
