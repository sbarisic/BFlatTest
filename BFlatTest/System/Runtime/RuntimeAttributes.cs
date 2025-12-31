
namespace System.Runtime.Versioning
{
    sealed class TargetFrameworkAttribute : Attribute
    {
        readonly string positionalString;

        public TargetFrameworkAttribute(string positionalString)
        {
            this.positionalString = positionalString;
        }

        public string PositionalString
        {
            get { return positionalString; }
        }

        // This is a named argument
        public string FrameworkDisplayName { get; set; }
    }
}
