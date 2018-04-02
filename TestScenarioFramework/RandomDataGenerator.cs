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

        public decimal GetDecimal(decimal max)
        {
            return GetDecimal(0m, max);
        }

        public decimal GetDecimal(decimal min, decimal max)
        {
            return Math.Round(min + (decimal)(_rnd.NextDouble() * (double)(max - min)), 2);
        }

        public double GetDouble(double max)
        {
            return GetDouble(0d, max);
        }

        public double GetDouble(double min, double max)
        {
            return min + (_rnd.NextDouble() * (max - min));
        }

        public int GetInteger(int max)
        {
            return _rnd.Next(max);
        }

        public int GetInteger(int min, int max)
        {
            return _rnd.Next(min, max);
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
