namespace StaffTrain.FwClass.NavigatorTools.Convertor
{
    public interface IDataConverter
    {
        object ConvertTo(object o,params object[] @params);

        object ConvertFrom(object o, params object[] @params);
    }
}
