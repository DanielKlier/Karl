using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Karl.Settings
{
    internal class IniParser
    {
        private static readonly Regex CommentRegex;
        private static readonly Regex SectionRegex;
        private static readonly Regex KeyValueRegex;

        static IniParser()
        {
            CommentRegex = new Regex(@"^#\s*(?<comment>.*)$");
            SectionRegex = new Regex(@"^\[\s*(?<section>\w+)\s*\]$");
            KeyValueRegex = new Regex("^(?<key>\\w+)\\s*=\\s*(\\\"(?<value>[^\\\"]*)\"|(?<value>[^,\\\"]*))$");
        }

        public void Parse(Stream stream, Action<string, string, string> keyValuePairAction)
        {
            var reader = new StreamReader(stream);

            string currentSection = "Default";

            for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                string comment;
                if(MatchComment(line, out comment)) continue;

                KeyValuePair<string, string> keyValuePair;

                if(MatchSection(line, ref currentSection)) continue;
                else if (MatchKeyValuePair(line, out keyValuePair))
                {
                    keyValuePairAction(currentSection, keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        private bool MatchKeyValuePair(string line, out KeyValuePair<string, string> keyValuePair)
        {
            var match = KeyValueRegex.Match(line);
            if (match.Success)
            {
                var key = match.Groups["key"].Value;
                var value = match.Groups["value"].Value;
                keyValuePair = new KeyValuePair<string, string>(key, value);
            }
            else
            {
                keyValuePair = new KeyValuePair<string, string>("", "");
            }
            return match.Success;
        }

        private bool MatchSection(string line, ref string currentSection)
        {
            var match = SectionRegex.Match(line);
            if(match.Success)
                currentSection = match.Groups["section"].Value;
            return match.Success;
        }

        private bool MatchComment(string line, out string comment)
        {
            var match = CommentRegex.Match(line);
            comment = match.Success ? match.Groups["comment"].Value : null;
            return match.Success;
        }
    }
}