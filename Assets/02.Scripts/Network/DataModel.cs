using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common
{

    public class DataModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderKind Order { get; set; } = default;
        public List<Item> Items { get; set; } = new List<Item>();
    }

    public class Item
    {
        public string Name { get; set; } = default;
        public object Value { get; set; }
    }

    public enum OrderKind
    {
        ADVISE,
        UNADVISE,
        SET,
        GET
    }

    public static class Utility
    {
        public static OrderKind StringToOrderKind(string kind)
        {
            return (OrderKind)Enum.Parse(typeof(OrderKind), kind);
        }

        public static string DataModelToString(DataModel model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
