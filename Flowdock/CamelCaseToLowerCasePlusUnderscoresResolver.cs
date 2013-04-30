namespace Flowdock
{
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Serialization;

    public class CamelCaseToLowerCasePlusUnderscoresResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return Regex.Replace(propertyName, @"(\p{Ll})(\p{Lu})", "$1_$2", RegexOptions.Compiled).ToLower();
        }
    }
}