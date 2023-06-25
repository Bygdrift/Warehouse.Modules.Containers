using Bygdrift.Warehouse.Attributes;

namespace Module
{
    public class Settings
    {
        [ConfigSetting(NotSet = NotSet.ThrowError)]
        public string ScheduleExpression { get; set; }

        [ConfigSetting(NotSet = NotSet.ThrowError)]
        public string ResourceGroup { get; set; }

        [ConfigSetting]
        public string Container1Name { get; set; }

        [ConfigSetting]
        public string Container1Image { get; set; }
        
        [ConfigSetting]
        public string Container1Variables { get; set; }

        [ConfigSetting]
        public string Container2Name { get; set; }

        [ConfigSetting]
        public string Container2Image { get; set; }
        
        [ConfigSetting]
        public string Container2Variables { get; set; }

        [ConfigSetting]
        public string Container3Name { get; set; }

        [ConfigSetting]
        public string Container3Image { get; set; }

        [ConfigSetting]
        public string Container3Variables { get; set; }
    }
}
