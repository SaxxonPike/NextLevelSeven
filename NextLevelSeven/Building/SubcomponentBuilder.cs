namespace NextLevelSeven.Building
{
    public sealed class SubcomponentBuilder : BuilderBase
    {
        private string _value;

        internal SubcomponentBuilder()
        {
        }

        public SubcomponentBuilder Subcomponent(string value)
        {
            _value = value;
            return this;
        }

        public override string ToString()
        {
            return _value ?? string.Empty;
        }
    }
}