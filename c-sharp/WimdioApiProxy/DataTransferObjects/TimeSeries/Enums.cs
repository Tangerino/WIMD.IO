

namespace WimdioApiProxy.v2.DataTransferObjects.TimeSeries
{
    public enum DataOperation
    {
        Sum,
        Max,
        Min,
        Avg,
        Count
    }

    public enum TimeInterval
    {
        Day,
        Month,
        Year,
        None
    }
}
