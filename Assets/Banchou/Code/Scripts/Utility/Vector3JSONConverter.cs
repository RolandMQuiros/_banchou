using UnityEngine;
using Newtonsoft.Json;
using System;

public class Vector3JSONConverter : JsonConverter<Vector3> {
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer) {
        float? x = null;
        float? y = null;
        float? z = null;
        
        if (reader.TokenType == JsonToken.StartObject) {
            string propertyName = null;
            while (reader.Read()) {
                switch (reader.TokenType) {
                    case JsonToken.PropertyName:
                        propertyName = (string)reader.Value;
                        break;
                    case JsonToken.Float:
                    case JsonToken.Integer:
                        float value = Convert.ToSingle(reader.Value);
                        switch (propertyName) {
                            case "X": x = value; break;
                            case "Y": y = value; break;
                            case "Z": z = value; break;
                        }
                        break;
                }
            }
        }

        if (x.HasValue && y.HasValue && z.HasValue) {
            return new Vector3(x.Value, y.Value, z.Value);
        }

        return existingValue;
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer) {
        writer.WriteStartObject();
        writer.WritePropertyName("X");
        writer.WriteValue(value.x);
        writer.WritePropertyName("Y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("Z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }
}
