using System;

namespace Convolved.Funnel
{
    static class Ensure
    {
        public static void ArgumentGreater<T>(T arg, T min, string paramName)
            where T : IComparable<T>
        {
            if (arg.CompareTo(min) < 0)
                throw new ArgumentException(string.Format("Parameter '{0}' must be greater than {1}.",
                    paramName, min));
        }

        public static void ArgumentNotNull(object arg, string paramName)
        {
            if (arg == null)
                throw new ArgumentNullException(paramName);
        }

        public static void ArgumentNotNullOrEmpty(string arg, string paramName)
        {
            if (string.IsNullOrEmpty(arg))
                throw new ArgumentException(string.Format("Parameter '{0}' cannot be null or empty.",
                    paramName));
        }
    }
}