using System;
using System.Collections.Generic;
using System.Text;

namespace TestScenarioFramework
{
    public class RandomDataGenerator
    {
        private Random _rnd;

        public RandomDataGenerator()
        {
            _rnd = new Random();
        }

        public string GetString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[_rnd.Next(chars.Length)]);
            }

            return sb.ToString();
        }

        public int GetInteger(int maxValue)
        {
            return _rnd.Next(maxValue);
        }

        public int GetInteger(int minValue, int maxValue)
        {
            return _rnd.Next(minValue, maxValue);
        }

        public DateTime GetDateTime(DateTime max)
        {
            long ticks = (long)(_rnd.NextDouble() * max.Ticks);
            return new DateTime(ticks);
        }

        public DateTime GetDateTime(DateTime min, DateTime max)
        {
            long ticks = min.Ticks + (long)(_rnd.NextDouble() * (max.Ticks - min.Ticks));
            return new DateTime(ticks);
        }

    }
}
