using System;

namespace ShuHai.Unity
{
    public class TargetTypeAttribute : Attribute
    {
        public Type Value;

        public TargetTypeAttribute(Type value) { Value = value; }
    }
}