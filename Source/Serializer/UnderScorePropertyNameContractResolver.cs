using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace Serializer
{
    public class UnderScorePropertyNameContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            var regex = new Regex("^([A-Z][^A-Z]*)+$");

            var match = regex.Match(propertyName);

            var parts = match.Groups[1].Captures
                .Cast<Capture>()
                .Select(x => x.Value)
                .Select(x => x.ToLower())
                .ToArray();

            return string.Join("_", parts);
        }
    }
}