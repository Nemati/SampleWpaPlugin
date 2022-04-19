using Microsoft.Performance.SDK.Processing;
using System;

namespace SampleAddIn
{
    internal class P50 : IAggregationModeClass
    {
        public P50()
        {
            AggregationName = "P50";
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

            //make sure the list is sorted, but use a new array
            TProjectionResult[] sortedPNumbers = (TProjectionResult[])value.Clone();
            Array.Sort(sortedPNumbers);

            //get the median
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            TProjectionResult median = sortedPNumbers[mid];

            return median;
        }
    }
}
