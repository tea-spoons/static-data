
namespace TeaSpoons.StaticData
{
    using System;

    public class MissingValueException : Exception
    {
        public override string Message => "A required value was not found in the document. For optional values, please supply a value to the defaultValue parameter.";

        public MissingValueException()
        {
        }
    }
}
