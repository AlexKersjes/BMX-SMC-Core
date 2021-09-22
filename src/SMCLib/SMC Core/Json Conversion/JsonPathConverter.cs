using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

// From https://stackoverflow.com/questions/33088462/can-i-specify-a-path-in-an-attribute-to-map-a-property-in-my-class-to-a-child-pr
namespace SMC_Core
{
    public class JsonPathConverter : JsonConverter
    {
        /// <inheritdoc />
        public override object ReadJson
        (
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer
        )
        {
            JObject jo = JObject.Load(reader);
            object targetObj = existingValue ?? Activator.CreateInstance(objectType);

            foreach (FieldInfo prop in objectType.GetFields())
            {
                JsonPropertyAttribute pathAttribute = prop.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();
                JsonConverterAttribute converterAttribute = prop.GetCustomAttributes(true).OfType<JsonConverterAttribute>().FirstOrDefault();

                string jsonPath = pathAttribute?.PropertyName ?? prop.Name;
                JToken token = jo.SelectToken(jsonPath);

                if (token != null && token.Type != JTokenType.Null)
                {
                    bool done = false;

                    if (converterAttribute != null)
                    {
                        object[] args = converterAttribute.ConverterParameters ?? Array.Empty<object>();
                        JsonConverter converter = Activator.CreateInstance(converterAttribute.ConverterType, args) as JsonConverter;
                        if (converter != null && converter.CanRead)
                        {
                            using (StringReader sr = new StringReader(token.ToString()))
                            using (JsonTextReader jr = new JsonTextReader(sr))
                            {
                                object value = converter.ReadJson(jr, prop.FieldType, prop.GetValue(targetObj), serializer);
                                if (!prop.IsInitOnly)
                                {
                                    prop.SetValue(targetObj, value);
                                }
                                done = true;
                            }
                        }
                    }

                    if (!done)
                    {
                        object value = token.ToObject(prop.FieldType, serializer);
                        prop.SetValue(targetObj, value);
                    }
                }
            }

            foreach (PropertyInfo prop in objectType.GetProperties())
            {
                JsonPropertyAttribute pathAttribute = prop.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();
                JsonConverterAttribute converterAttribute = prop.GetCustomAttributes(true).OfType<JsonConverterAttribute>().FirstOrDefault();

                string jsonPath = pathAttribute?.PropertyName ?? prop.Name;
                JToken token = jo.SelectToken(jsonPath);

                if (token != null && token.Type != JTokenType.Null)
                {
                    bool done = false;

                    if (converterAttribute != null)
                    {
                        object[] args = converterAttribute.ConverterParameters ?? Array.Empty<object>();
                        JsonConverter converter = Activator.CreateInstance(converterAttribute.ConverterType, args) as JsonConverter;
                        if (converter != null && converter.CanRead)
                        {
                            using (StringReader sr = new StringReader(token.ToString()))
                            using (JsonTextReader jr = new JsonTextReader(sr))
                            {
                                object value = converter.ReadJson(jr, prop.PropertyType, prop.GetValue(targetObj), serializer);
                                if (prop.CanWrite)
                                {
                                    prop.SetValue(targetObj, value);
                                }
                                done = true;
                            }
                        }
                    }

                    if (!done)
                    {
                        if (prop.CanWrite)
                        {
                            object value = token.ToObject(prop.PropertyType, serializer);
                            prop.SetValue(targetObj, value);
                        }
                        else
                        {
                            using (StringReader sr = new StringReader(token.ToString()))
                            {
                                serializer.Populate(sr, prop.GetValue(targetObj));
                            }
                        }
                    }
                }
            }

            return targetObj;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            // CanConvert is not called when [JsonConverter] attribute is used
            return objectType.GetCustomAttributes(true).OfType<JsonPathConverter>().Any();
        }


        private void SetNestedProperty(string[] nesting, JObject lastLevel, PropertyInfo prop, object value, JsonSerializer serializer)
        {

            for (int i = 0; i < nesting.Length; i++)
            {
                if (i == nesting.Length - 1)
                {
                    object propertyValue = prop.GetValue(value);
                    JToken test = JToken.FromObject(propertyValue);

                    if (test.Type != JTokenType.Object)
                    {
                        lastLevel[nesting[i]] = new JValue(propertyValue);
                    }

                    else
                    {
                        IEnumerable<PropertyInfo> properties = propertyValue.GetType().GetRuntimeProperties().Where(p => p.CanRead && p.CanWrite);
                        FieldInfo[] fields = propertyValue.GetType().GetFields();
                        lastLevel[nesting[i]] = new JObject();
                        lastLevel = (JObject)lastLevel[nesting[i]];
                        foreach (PropertyInfo nestedProp in properties)
                        {
                            JsonPropertyAttribute att = nestedProp.GetCustomAttributes(true)
                        .OfType<JsonPropertyAttribute>()
                        .FirstOrDefault();

                            string nestedJsonPath = att != null ? att.PropertyName : nestedProp.Name;

                            if (serializer.ContractResolver is DefaultContractResolver)
                            {
                                DefaultContractResolver resolver = (DefaultContractResolver)serializer.ContractResolver;
                                nestedJsonPath = resolver.GetResolvedPropertyName(nestedJsonPath);
                            }

                            string[] nestedNesting = nestedJsonPath.Split('.');

                            SetNestedProperty(nestedNesting, lastLevel, nestedProp, propertyValue, serializer);

                        }

                        foreach (FieldInfo nestedProp in fields)
                        {
                            JsonPropertyAttribute att = nestedProp.GetCustomAttributes(true)
                        .OfType<JsonPropertyAttribute>()
                        .FirstOrDefault();

                            string nestedJsonPath = att != null ? att.PropertyName : nestedProp.Name;

                            if (serializer.ContractResolver is DefaultContractResolver)
                            {
                                DefaultContractResolver resolver = (DefaultContractResolver)serializer.ContractResolver;
                                nestedJsonPath = resolver.GetResolvedPropertyName(nestedJsonPath);
                            }

                            string[] nestedNesting = nestedJsonPath.Split('.');

                            SetNestedField(nestedNesting, lastLevel, nestedProp, propertyValue, serializer);

                        }
                    }
                }
                else
                {
                    if (lastLevel[nesting[i]] == null)
                    {
                        lastLevel[nesting[i]] = new JObject();
                    }

                    lastLevel = (JObject)lastLevel[nesting[i]];
                }
            }
        }


        private void SetNestedField(string[] nesting, JObject lastLevel, FieldInfo prop, object value, JsonSerializer serializer)
        {

            for (int i = 0; i < nesting.Length; i++)
            {
                if (i == nesting.Length - 1)
                {
                    object propertyValue = prop.GetValue(value);
                    JToken test = JToken.FromObject(propertyValue);

                    if (test.Type != JTokenType.Object)
                    {
                        lastLevel[nesting[i]] = new JValue(propertyValue);
                    }

                    else
                    {
                        IEnumerable<PropertyInfo> properties = propertyValue.GetType().GetRuntimeProperties();
                        FieldInfo[] fields = propertyValue.GetType().GetFields();
                        lastLevel[nesting[i]] = new JObject();
                        lastLevel = (JObject)lastLevel[nesting[i]];
                        foreach (FieldInfo nestedProp in fields)
                        {
                            JsonPropertyAttribute att = nestedProp.GetCustomAttributes(true)
                        .OfType<JsonPropertyAttribute>()
                        .FirstOrDefault();

                            string nestedJsonPath = att != null ? att.PropertyName : nestedProp.Name;

                            if (serializer.ContractResolver is DefaultContractResolver)
                            {
                                DefaultContractResolver resolver = (DefaultContractResolver)serializer.ContractResolver;
                                nestedJsonPath = resolver.GetResolvedPropertyName(nestedJsonPath);
                            }

                            string[] nestedNesting = nestedJsonPath.Split('.');

                            SetNestedField(nestedNesting, lastLevel, nestedProp, propertyValue, serializer);

                        }

                        foreach (PropertyInfo nestedProp in properties)
                        {
                            JsonPropertyAttribute att = nestedProp.GetCustomAttributes(true)
                        .OfType<JsonPropertyAttribute>()
                        .FirstOrDefault();

                            string nestedJsonPath = att != null ? att.PropertyName : nestedProp.Name;

                            if (serializer.ContractResolver is DefaultContractResolver)
                            {
                                DefaultContractResolver resolver = (DefaultContractResolver)serializer.ContractResolver;
                                nestedJsonPath = resolver.GetResolvedPropertyName(nestedJsonPath);
                            }

                            string[] nestedNesting = nestedJsonPath.Split('.');

                            SetNestedProperty(nestedNesting, lastLevel, nestedProp, propertyValue, serializer);

                        }
                    }
                }
                else
                {
                    if (lastLevel[nesting[i]] == null)
                    {
                        lastLevel[nesting[i]] = new JObject();
                    }

                    lastLevel = (JObject)lastLevel[nesting[i]];
                }
            }
        }



        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IEnumerable<PropertyInfo> properties = value.GetType().GetRuntimeProperties();
            FieldInfo[] fields = value.GetType().GetFields();
            JObject main = new JObject();
            foreach (PropertyInfo prop in properties)
            {
                JsonPropertyAttribute att = prop.GetCustomAttributes(true)
                    .OfType<JsonPropertyAttribute>()
                    .FirstOrDefault();

                string jsonPath = att != null ? att.PropertyName : prop.Name;

                if (serializer.ContractResolver is DefaultContractResolver)
                {
                    DefaultContractResolver resolver = (DefaultContractResolver)serializer.ContractResolver;
                    jsonPath = resolver.GetResolvedPropertyName(jsonPath);
                }

                string[] nesting = jsonPath.Split('.');
                JObject lastLevel = main;

                SetNestedProperty(nesting, lastLevel, prop, value, serializer);

            }


            foreach (FieldInfo prop in fields)
            {
                JsonPropertyAttribute att = prop.GetCustomAttributes(true)
                    .OfType<JsonPropertyAttribute>()
                    .FirstOrDefault();

                string jsonPath = att != null ? att.PropertyName : prop.Name;

                if (serializer.ContractResolver is DefaultContractResolver)
                {
                    DefaultContractResolver resolver = (DefaultContractResolver)serializer.ContractResolver;
                    jsonPath = resolver.GetResolvedPropertyName(jsonPath);
                }

                string[] nesting = jsonPath.Split('.');
                JObject lastLevel = main;

                SetNestedField(nesting, lastLevel, prop, value, serializer);

            }
            serializer.Serialize(writer, main);
        }
    }
}