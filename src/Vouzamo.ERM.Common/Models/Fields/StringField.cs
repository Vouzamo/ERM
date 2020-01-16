namespace Vouzamo.ERM.Common
{
    public class StringField : Field<string>
    {
        public override string Type => "string";
        
        protected StringField() : base()
        {

        }

        public StringField(string key, string name, bool mandatory = false, bool enumerable = false) : base(key, name, mandatory, enumerable)
        {

        }
    }
}
