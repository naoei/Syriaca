using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Syriaca.Client.Memory
{
    public class AddressDictionary : Dictionary<string, Address>
    {
        public static AddressDictionary Parse(string contents)
        {
            var lines = contents.Split(Environment.NewLine);

            var result = new AddressDictionary();
            Address currentEntry = null;

            foreach (var line in lines)
            {
                if (line.StartsWith('#'))
                    continue;

                if (!line.Any())
                {
                    result.Add(currentEntry.Name, currentEntry);
                    currentEntry = null;

                    continue;
                }

                if (line.StartsWith('['))
                {
                    currentEntry = new Address { Name = line[1..^1].Trim() };

                    continue;
                }

                var colonIndex = line.IndexOf(':');
                var property = line.Substring(0, colonIndex);
                var value = line.Substring(colonIndex + 1);

                switch (property)
                {
                    case "offsets":
                        currentEntry.Offsets = value.Replace(" ", "").Replace("0x", "").Split('|')
                           .Select(o => int.Parse(o, NumberStyles.HexNumber)).ToArray();

                        break;

                    case "valueType":
                        currentEntry.Type = value;

                        break;
                }
            }
            
            if (currentEntry != null)
                result.Add(currentEntry.Name, currentEntry);

            return result;
        }
    }
}