using System;
using System.Reflection.Emit;
using System.Reflection;

namespace Simpler
{
    public class CreateProxyType : InOutTask<CreateProxyType.Input, CreateProxyType.Output>
    {
        public class Input
        {
            public Type Type { get; set; }
        }

        public class Output
        {
            public TypeBuilder TypeBuilder { get; set; }
        }

        public override void Execute()
        {
            var domain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName("SimplerProxies");
            var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);

            var typeName = String.Concat(In.Type.FullName, "Proxy");
            Out.TypeBuilder =  moduleBuilder.DefineType(
                typeName,
                TypeAttributes.Public,
                In.Type
            );
        }
    }
}
