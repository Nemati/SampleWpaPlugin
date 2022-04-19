using Microsoft.Performance.SDK.Processing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleAddIn
{
    internal class P50WithKey : IAggregationModeClass
    {
        public P50WithKey()
        {
            AggregationName = "P50ofSumWithColumnThreeAsKey";
            AggregationType = typeof(int);
        }

        public Type AggregationType { get; }

        public string AggregationName { get; }

        public object AggrgateRows<TProjectionResult, TKeyProjectionResult>(int[] rowIndices, int start, int count,
            IProjection<int, TProjectionResult> projection,
            IProjection<int, TKeyProjectionResult> keyProjection,
            Func<TProjectionResult, TProjectionResult, TProjectionResult> aggegationFuncBaseOnKeyColumn,
            TProjectionResult defaultTProjectionResult)
        {
            TProjectionResult[] value = new TProjectionResult[count];

            TKeyProjectionResult[] key = new TKeyProjectionResult[projection != null ? count : 0];

            for (int index = start; index < start + count; index++)
            {
                value[index - start] = projection[rowIndices[index]];
            }

            if (value == null || value.Length == 0)
                throw new System.Exception("Median of empty array not defined.");

            if (keyProjection != null)
            {
                for (int index = start; index < start + count; index++)
                {
                    key[index - start] = keyProjection[rowIndices[index]];
                }
            }

            if (key == null || key.Length == 0)
                throw new System.Exception("Median of empty array not defined.");

            IDictionary<TKeyProjectionResult, TProjectionResult> keyValuePairs =
                new Dictionary<TKeyProjectionResult, TProjectionResult>();

            for (int index = 0; index < count; index++)
            {
                if (!keyValuePairs.ContainsKey(key[index]))
                {
                    keyValuePairs.Add(key[index], defaultTProjectionResult);
                }

                TProjectionResult agg = aggegationFuncBaseOnKeyColumn(keyValuePairs[key[index]], value[index]);

                keyValuePairs[key[index]] = agg;
            }

            keyValuePairs.Values.ToArray();
            //make sure the list is sorted, but use a new array
            TProjectionResult[] sortedPNumbers = keyValuePairs.Values.ToArray();
            Array.Sort(sortedPNumbers);

            //get the median
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            TProjectionResult median = sortedPNumbers[mid];

            return median;
        }
    }
}
