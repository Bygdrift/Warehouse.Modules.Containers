using Bygdrift.Warehouse.Attributes;

namespace Module
{
    public class Settings
    {
        [ConfigSetting(Default = "0 0 1 * * *")]
        public string ScheduleExpression { get; set; }
    }
}
