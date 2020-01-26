namespace Vouzamo.ERM.Common.Converters
{
    public interface IConverter
    {
        TTo Convert<TFrom, TTo>(TFrom source);
    }
}
