using Vouzamo.ERM.Common.Serialization;

namespace Vouzamo.ERM.Common.Converters
{

    public class JsonSerializationConverter : IConverter
    {
        protected IJsonSerializer Serializer { get; }

        public JsonSerializationConverter(IJsonSerializer serializer)
        {
            Serializer = serializer;
        }

        public TTo Convert<TFrom, TTo>(TFrom source)
        {
            var json = Serializer.Serialize(source);

            return Serializer.Deserialize<TTo>(json);
        }
    }
}
