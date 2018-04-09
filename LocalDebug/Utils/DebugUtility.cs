using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LocalDebug.Utils
{
    internal class DebugUtility
    {
        public static string EntityToString(object o, int numTabs = 0)
        {
            if (o == null) return String.Empty;

            var sb = new StringBuilder();
            string tabs = new string('\t', numTabs);
            
            foreach (var pi in o.GetType().GetProperties())
            {
                if (pi.PropertyType.IsValueType || typeof(String).IsAssignableFrom(pi.PropertyType))
                {
                    sb.AppendLine($"{tabs}\"{pi.Name}\": \"{pi.GetValue(o)}\"");
                }
                else if (typeof(IList).IsAssignableFrom(pi.PropertyType))
                {
                    var list = (IList)pi.GetValue(o);

                    sb.AppendLine($"{tabs}\"{pi.Name}\": ");

                    if (list != null)
                    {
                        foreach (var e in list)
                        {
                            sb.AppendLine(Utils.DebugUtility.EntityToString(e, numTabs + 1));
                        }
                    }
                }
                else
                {
                    sb.AppendLine($"{tabs}\"{pi.Name}\": ");
                    sb.AppendLine(Utils.DebugUtility.EntityToString(pi.GetValue(o), numTabs + 1));
                }
            }

            return sb.ToString();
        }
    }
}
