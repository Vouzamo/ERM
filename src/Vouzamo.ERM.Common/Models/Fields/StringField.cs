namespace Vouzamo.ERM.Common
{
    public class StringField : Field<string>
    {
        public override string Type => "string";

        public int MinLength { get; set; } = 0;
        public int MaxLength { get; set; } = 255;
        
        protected StringField() : base()
        {

        }

        public StringField(string key, string name, bool mandatory = false, bool enumerable = false) : base(key, name, mandatory, enumerable)
        {

        }
    }
}
