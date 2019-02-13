using System;

namespace ShuHai.Unity
{
    public class TargetTypesAttribute : Attribute
    {
        public Type[] Values;

        public TargetTypesAttribute(params Type[] values) { Values = values; }
    }
}