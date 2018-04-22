using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using TestScenarioFramework.Attributes;

namespace TestScenarioFramework
{
    /// <summary>
    /// Default class for generating random data.
    /// </summary>
    public class RandomDataGenerator
    {
        private const int DefaultListMultiplicity = 10;

        private Random _rnd;

        /// <summary>
        /// Initializes a new RandomDataGenerator instance.
        /// </summary>
        public RandomDataGenerator()
        {
            _rnd = new Random();
        }

        public void PopulateObjectFields(object obj, Func<Type, object> initNewObject)
        {
            Type t = obj.GetType();

            foreach (var pi in t.GetProperties())
            {
                var att = pi.GetCustomAttribute<TestScenarioMemberAttribute>();
                if (att != null && att.Exclude) continue;

                //Console.WriteLine(pi.PropertyType);
                switch (pi.PropertyType.ToString())
                {
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        pi.SetValue(obj, GetInteger(100));
                        break;

                    case "System.Decimal":
                        {
                            decimal min = att != null ? Convert.ToDecimal(att.Min) : 0m;
                            decimal max = att != null ? Convert.ToDecimal(att.Max) : 0m;

                            pi.SetValue(
                                obj,
                                max != 0m ? GetDecimal(min, max) : GetDecimal(max));

                            break;
                        }
                    case "System.Single":
                    case "System.Double":
                        {
                            double min = att != null ? Convert.ToDouble(att.Min) : 0d;
                            double max = att != null ? Convert.ToDouble(att.Max) : 0d;

                            pi.SetValue(
                                obj,
                                max != 0d ? GetDouble(min, max) : GetDouble(max));

                            break;
                        }
                    case "System.DateTime":
                        {
                            DateTime min = DateTime.MinValue;
                            DateTime max = DateTime.MinValue;

                            if (att != null)
                            {
                                DateTime.TryParseExact(
                                    Convert.ToString(att.Min),
                                    "yyyy-MM-dd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out min);

                                DateTime.TryParseExact(
                                    Convert.ToString(att.Max),
                                    "yyyy-MM-dd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out max);
                            }

                            if (min == DateTime.MinValue) min = DateTime.Now.AddMonths(-1);
                            if (max == DateTime.MinValue) max = DateTime.Now;

                            pi.SetValue(obj, GetDateTime(min, max));
                            break;

                        }

                    case "System.String":

                        pi.SetValue(obj, GetString(10));
                        break;

                    default:

                        if (pi.PropertyType.IsValueType)
                            break;

                        if (typeof(IList).IsAssignableFrom(pi.PropertyType))
                        {
                            // Create new enumerable
                            var list = (IList)Activator.CreateInstance(pi.PropertyType);
                            Type innerType = pi.PropertyType.GetGenericArguments()[0];
                            int numElements = att != null ? att.Multiplicity : DefaultListMultiplicity;

                            // Create some list elements
                            for (int i = 0; i < numElements; i++)
                            {
                                list.Add(initNewObject(innerType));
                            }

                            pi.SetValue(obj, list);
                        } 
                        else
                        {
                            pi.SetValue(obj, initNewObject(pi.PropertyType));
                        }
                        //else
                        //    Console.WriteLine("nah" + pi.Name);

                        break;
                }

            }
        }

        private string GetString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[_rnd.Next(chars.Length)]);
            }

            return sb.ToString();
        }

        private decimal GetDecimal(decimal max)
        {
            return GetDecimal(0m, max);
        }

        private decimal GetDecimal(decimal min, decimal max)
        {
            return Math.Round(min + (decimal)(_rnd.NextDouble() * (double)(max - min)), 2);
        }

        private double GetDouble(double max)
        {
            return GetDouble(0d, max);
        }

        private double GetDouble(double min, double max)
        {
            return min + (_rnd.NextDouble() * (max - min));
        }

        private int GetInteger(int max)
        {
            return _rnd.Next(max);
        }

        private int GetInteger(int min, int max)
        {
            return _rnd.Next(min, max);
        }

        private DateTime GetDateTime(DateTime max)
        {
            long ticks = (long)(_rnd.NextDouble() * max.Ticks);
            return new DateTime(ticks);
        }

        private DateTime GetDateTime(DateTime min, DateTime max)
        {
            long ticks = min.Ticks + (long)(_rnd.NextDouble() * (max.Ticks - min.Ticks));
            return new DateTime(ticks);
        }

    }
}
