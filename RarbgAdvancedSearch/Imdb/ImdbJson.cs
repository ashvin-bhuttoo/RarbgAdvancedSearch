namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ImdbJson
    {
        [JsonProperty("@context", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Context { get; set; }

        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Image { get; set; }

        [JsonProperty("genre", NullValueHandling = NullValueHandling.Ignore)]
        public Genre? Genre { get; set; }

        [JsonProperty("contentRating", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentRating { get; set; }

        [JsonProperty("actor", NullValueHandling = NullValueHandling.Ignore)]
        public ActorUnion? Actor { get; set; }

        [JsonProperty("director", NullValueHandling = NullValueHandling.Ignore)]
        public ActorUnion? Director { get; set; }

        [JsonProperty("creator", NullValueHandling = NullValueHandling.Ignore)]
        public ActorUnion? Creator { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("datePublished", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DatePublished { get; set; }

        [JsonProperty("keywords", NullValueHandling = NullValueHandling.Ignore)]
        public string Keywords { get; set; }

        [JsonProperty("aggregateRating", NullValueHandling = NullValueHandling.Ignore)]
        public Rating AggregateRating { get; set; }

        [JsonProperty("review", NullValueHandling = NullValueHandling.Ignore)]
        public Review Review { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public string Duration { get; set; }

        [JsonProperty("trailer", NullValueHandling = NullValueHandling.Ignore)]
        public Trailer Trailer { get; set; }
    }

    public partial class ActorElement
    {
        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public TypeEnum? Type { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class Rating
    {
        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("ratingCount", NullValueHandling = NullValueHandling.Ignore)]
        public long? RatingCount { get; set; }

        [JsonProperty("bestRating", NullValueHandling = NullValueHandling.Ignore)]
        public string BestRating { get; set; }

        [JsonProperty("worstRating", NullValueHandling = NullValueHandling.Ignore)]
        public string WorstRating { get; set; }

        [JsonProperty("ratingValue", NullValueHandling = NullValueHandling.Ignore)]
        public string RatingValue { get; set; }
    }

    public partial class Review
    {
        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("itemReviewed", NullValueHandling = NullValueHandling.Ignore)]
        public ItemReviewed ItemReviewed { get; set; }

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public Author Author { get; set; }

        [JsonProperty("dateCreated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DateCreated { get; set; }

        [JsonProperty("inLanguage", NullValueHandling = NullValueHandling.Ignore)]
        public string InLanguage { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("reviewBody", NullValueHandling = NullValueHandling.Ignore)]
        public string ReviewBody { get; set; }

        [JsonProperty("reviewRating", NullValueHandling = NullValueHandling.Ignore)]
        public Rating ReviewRating { get; set; }
    }

    public partial class Author
    {
        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public TypeEnum? Type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class ItemReviewed
    {
        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    public partial class Trailer
    {
        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("embedUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string EmbedUrl { get; set; }

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public Thumbnail Thumbnail { get; set; }

        [JsonProperty("thumbnailUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ThumbnailUrl { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("uploadDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? UploadDate { get; set; }
    }

    public partial class Thumbnail
    {
        [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("contentUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ContentUrl { get; set; }
    }

    public enum TypeEnum { Organization, Person };

    public partial struct ActorUnion
    {
        public ActorElement ActorElement;
        public ActorElement[] ActorElementArray;

        public static implicit operator ActorUnion(ActorElement ActorElement) => new ActorUnion { ActorElement = ActorElement };
        public static implicit operator ActorUnion(ActorElement[] ActorElementArray) => new ActorUnion { ActorElementArray = ActorElementArray };
    }

    public partial struct Genre
    {
        public string String;
        public string[] StringArray;

        public static implicit operator Genre(string String) => new Genre { String = String };
        public static implicit operator Genre(string[] StringArray) => new Genre { StringArray = StringArray };
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ActorUnionConverter.Singleton,
                TypeEnumConverter.Singleton,
                GenreConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ActorUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ActorUnion) || t == typeof(ActorUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<ActorElement>(reader);
                    return new ActorUnion { ActorElement = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<ActorElement[]>(reader);
                    return new ActorUnion { ActorElementArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type ActorUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (ActorUnion)untypedValue;
            if (value.ActorElementArray != null)
            {
                serializer.Serialize(writer, value.ActorElementArray);
                return;
            }
            if (value.ActorElement != null)
            {
                serializer.Serialize(writer, value.ActorElement);
                return;
            }
            throw new Exception("Cannot marshal type ActorUnion");
        }

        public static readonly ActorUnionConverter Singleton = new ActorUnionConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Organization":
                    return TypeEnum.Organization;
                case "Person":
                    return TypeEnum.Person;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Organization:
                    serializer.Serialize(writer, "Organization");
                    return;
                case TypeEnum.Person:
                    serializer.Serialize(writer, "Person");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }

    internal class GenreConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Genre) || t == typeof(Genre?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Genre { String = stringValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<string[]>(reader);
                    return new Genre { StringArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type Genre");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Genre)untypedValue;
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.StringArray != null)
            {
                serializer.Serialize(writer, value.StringArray);
                return;
            }
            throw new Exception("Cannot marshal type Genre");
        }

        public static readonly GenreConverter Singleton = new GenreConverter();
    }
}
