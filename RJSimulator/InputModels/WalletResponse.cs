using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ChainCrims.InputModels
{
    public partial class WalletResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("ethereum")]
        public Ethereum Ethereum { get; set; }
    }

    public partial class Ethereum
    {
        [JsonProperty("transfers")]
        public List<Transfer> Transfers { get; set; }
    }

    public partial class Transfer
    {
        [JsonProperty("block")]
        public Block Block { get; set; }

        [JsonProperty("addressTo")]
        public AddressBase AddressTo { get; set; }

        [JsonProperty("addressFrom")]
        public AddressBase AddressFrom { get; set; }

        [JsonProperty("currency")]
        public Currency Currency { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("amountInUSD")]
        public double AmountInUsd { get; set; }

        [JsonProperty("transaction")]
        public Transaction Transaction { get; set; }
    }

    public partial class AddressBase
    {
        [JsonProperty("address")]
        public string addressBase { get; set; }
    }

    public partial class Block
    {
        [JsonProperty("timestamp")]
        public Timestamp Timestamp { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }

    public partial class Timestamp
    {
        [JsonProperty("time")]
        public DateTimeOffset Time { get; set; }
    }

    public partial class Currency
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Transaction
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }

    public partial class WalletResponse
    {
        public static WalletResponse FromJson(string json) => JsonConvert.DeserializeObject<WalletResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this WalletResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}

