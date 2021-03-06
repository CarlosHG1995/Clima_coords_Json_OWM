﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clima_coords_Json_OWM
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class IpStackProxy
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("continent_code")]
        public string ContinentCode { get; set; }

        [JsonProperty("continent_name")]
        public string ContinentName { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_name")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Zip { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("geoname_id")]
        public long GeonameId { get; set; }

        [JsonProperty("capital")]
        public string Capital { get; set; }

        [JsonProperty("languages")]
        public Language[] Languages { get; set; }

        [JsonProperty("country_flag")]
        public Uri CountryFlag { get; set; }

        [JsonProperty("country_flag_emoji")]
        public string CountryFlagEmoji { get; set; }

        [JsonProperty("country_flag_emoji_unicode")]
        public string CountryFlagEmojiUnicode { get; set; }

        [JsonProperty("calling_code")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CallingCode { get; set; }

        [JsonProperty("is_eu")]
        public bool IsEu { get; set; }
    }

    public partial class Language
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("native")]
        public string Native { get; set; }
    }

    public partial class IpStackProxy
    {
        public static IpStackProxy FromJson(string json) => JsonConvert.DeserializeObject<IpStackProxy>(json, Clima_coords_Json_OWM.Converter.Settings);
    }

    public static class Serialize1
    {
        public static string ToJson(this IpStackProxy self) => JsonConvert.SerializeObject(self, Clima_coords_Json_OWM.Converter.Settings);
    }

    internal static class Converter1
    {
        public static readonly JsonSerializerSettings Settings1 = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
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

    public partial class IpStackProxy
    {
        public async static Task<IpStackProxy> RecuperaTiempo(string la_ip)
        {
            HttpClient http = new HttpClient();
            var rta = await http.GetAsync("http://api.ipstack.com/" + la_ip.ToString() +
                        "?access_key=406badb299329af9ea3bdceffc3558d0&format=1");
            var resul = await rta.Content.ReadAsStringAsync();
            IpStackProxy data = IpStackProxy.FromJson(resul);
            return data;

            /*  HttpClient http = new HttpClient();
              var rta = await http.GetAsync("http://api.ipstack.com/"+la_ip.ToString()+"?access_key=406badb299329af9ea3bdceffc3558d0&format=1");
              var resul = await rta.Content.ReadAsStringAsync();
              IpStackProxy data = IpStackProxy.FromJson(resul);
              return data;*/
        }
    }
}
