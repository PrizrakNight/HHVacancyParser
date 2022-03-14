using System;
using System.Collections.Generic;
using System.Linq;

namespace HHVacancyParser.Core.Maths
{
    public static class DecimalEx
    {
        /// <summary>
        /// Calculates the standard deviation using the formula: q = √E(Xi - M)^2 / n - 1
        /// </summary>
        /// <returns>Standard deviation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static double GetSampleStandardDeviation(this IEnumerable<decimal> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (values.Any() == false)
                return 0;

            var count = values.Count();
            var castedValues = values.Select(x => (double)x);
            var avg = castedValues.Average();
            var sumSquaredDifferences = castedValues.Sum(x => Math.Pow(x - avg, 2));

            return Math.Sqrt(sumSquaredDifferences / count - 1);
        }

        /// <summary>
        /// Calculates the standard deviation using the formula: q = √E(Xi - M)^2 / n
        /// </summary>
        /// <returns>Standard deviation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static double GetPopulationStandardDeviation(this IEnumerable<decimal> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (values.Any() == false)
                return 0;

            var count = values.Count();
            var castedValues = values.Select(x => (double)x);
            var avg = castedValues.Average();
            var sumSquaredDifferences = castedValues.Sum(x => Math.Pow(x - avg, 2));

            return Math.Sqrt(sumSquaredDifferences / count);
        }

        /// <summary>
        /// Calculates the coefficient of variation using the formula: cv = q / u
        /// <para>q - population standard deviation</para>
        /// <para>u - arithmetic mean of elements</para>
        /// </summary>
        /// <returns>Coefficient of variation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static double GetCoefficientOfVariation(this IEnumerable<decimal> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            var avg = values.Select(x => (double)x).Average();

            if (avg == 0)
                return 0;

            var populationStandardDeviation = GetPopulationStandardDeviation(values);

            return populationStandardDeviation / avg;
        }
    }
}
