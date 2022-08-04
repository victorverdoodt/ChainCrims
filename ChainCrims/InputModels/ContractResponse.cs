using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChainCrims.InputModels
{
    public partial class ContractResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Ethereum
    {
        [JsonProperty("smartContractEvents")]
        public List<SmartContractEventElement> SmartContractEvents { get; set; }
    }

    public partial class SmartContractEventElement
    {
        [JsonProperty("block")]
        public Block Block { get; set; }

        [JsonProperty("transaction")]
        public Transaction Transaction { get; set; }

        [JsonProperty("eventIndex")]
        public string EventIndex { get; set; }

        [JsonProperty("arguments")]
        public List<Argument> Arguments { get; set; }

        [JsonProperty("smartContractEvent")]
        public SmartContractEventSmartContractEvent SmartContractEvent { get; set; }

        [JsonProperty("date")]
        public Date Date { get; set; }
    }

    public partial class Argument
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("argument")]
        public string ArgumentArgument { get; set; }

        [JsonProperty("index")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Index { get; set; }

        [JsonProperty("argumentType")]
        public ArgumentType ArgumentType { get; set; }
    }

    public partial class Timestamp
    {
        [JsonProperty("unixtime")]
        public long Unixtime { get; set; }
    }

    public partial class Date
    {
        [JsonProperty("date")]
        public DateTimeOffset DateDate { get; set; }
    }

    public partial class SmartContractEventSmartContractEvent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("signatureHash")]
        public string SignatureHash { get; set; }
    }

    public enum ArgumentType { Address, Bytes32, Uint256 };

    public partial class ContractResponse
    {
        public static ContractResponse FromJson(string json) => JsonConvert.DeserializeObject<ContractResponse>(json, Converter.Settings);
    }

    internal class ArgumentTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ArgumentType) || t == typeof(ArgumentType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "address":
                    return ArgumentType.Address;
                case "bytes32":
                    return ArgumentType.Bytes32;
                case "uint256":
                    return ArgumentType.Uint256;
            }
            throw new Exception("Cannot unmarshal type ArgumentType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ArgumentType)untypedValue;
            switch (value)
            {
                case ArgumentType.Address:
                    serializer.Serialize(writer, "address");
                    return;
                case ArgumentType.Bytes32:
                    serializer.Serialize(writer, "bytes32");
                    return;
                case ArgumentType.Uint256:
                    serializer.Serialize(writer, "uint256");
                    return;
            }
            throw new Exception("Cannot marshal type ArgumentType");
        }

        public static readonly ArgumentTypeConverter Singleton = new ArgumentTypeConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
