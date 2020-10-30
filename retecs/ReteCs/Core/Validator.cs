using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs.core
{
    public class Validator
    {
        public static bool IsValidData(Data data)
        {
            return IsValidId(data.Id);
        }

        public static bool IsValidId(string id)
        {
            var regex = new Regex(@"/^[\w-]{3,}@[0-9]+\.[0-9]+\.[0-9]+$/");
            return regex.IsMatch(id);
        }

        public static (bool success, string message) Validate(string id, Data data)
        {
            var id1 = id.Split("@");
            var id2 = data.Id.Split("@");
            var msg = new List<string>();
            if (!IsValidData(data))
            {
                msg.Add("Data is not suitable");
            }

            if (!string.Equals(id, data.Id))
            {
                msg.Add("IDs not equal");
            }

            if (id1.FirstOrDefault() != id2.FirstOrDefault())
            {
                msg.Add("Names don\'t match");
            }

            if (id1.Length < 2 || id2.Length < 2 || !string.Equals( id1[1], id2[1]))
            {
                msg.Add("Versions don\'t match");
            }

            return (msg.Any(), string.Join(", ", msg));
        }
    }
}