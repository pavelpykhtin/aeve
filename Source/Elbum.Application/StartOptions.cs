using CommandLine;

namespace Elbum.Application
{
    public class StartOptions
    {
        [Option(DefaultValue = "http://localhost:80", HelpText = "Aeve service adress [scheme://host:port]")]
        public string Aeve { get; set; }

        [Option(DefaultValue = "http://localhost:88", HelpText = "Proxy address [scheme://host:port]")]
        public string Proxy { get; set; }
    }
}