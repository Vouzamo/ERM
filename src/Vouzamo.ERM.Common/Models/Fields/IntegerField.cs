namespace Vouzamo.ERM.Common
{
    public class IntegerField : Field<int>
    {
        public override string Type => "int";

        public int MinValue { get; set; } = int.MinValue;
        public int MaxValue { get; set; } = int.MaxValue;

        protected IntegerField() : base()
        {

        }

        public IntegerField(string key, string name, bool mandatory = false, bool enumerable = false, bool localizable = true) : base(key, name, mandatory, enumerable, localizable)
        {

        }
}
}
