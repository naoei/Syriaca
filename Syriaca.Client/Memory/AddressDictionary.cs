using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Syriaca.Client.Memory
{
    /// <summary>
    /// A <see cref="Dictionary{TKey,TValue}"/> containing the name of the address and the <see cref="Address"/> value.
    /// </summary>
    public class AddressDictionary : Dictionary<string, Address>
    {
        /// <summary>
        /// Parses a string of text containing all of the memory offsets and converts it into an <see cref="AddressDictionary"/>.
        /// Note that the file format will follow this pattern:
        /// 
        /// <code>
        /// [Address name]
        /// offsets: 0x01 | 0x02
        /// valueType: int
        ///
        /// # ...
        /// </code>
        /// </summary>
        /// <param name="contents">The contents, usually from a file.</param>
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
                        currentEntry.Type = value.Replace(" ", "");

                        break;
                }
            }
            
            if (currentEntry != null)
                result.Add(currentEntry.Name, currentEntry);

            return result;
        }
    }
}