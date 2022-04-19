using Microsoft.Performance.SDK.Processing;
using System;


namespace SampleAddIn
{
    internal class P90 : IAggregationModeClass
    {
        public P90()
        {
            AggregationName = "P90";
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
            int p90Index = (size - 1) - size / 10;
            TProjectionResult p90 = sortedPNumbers[p90Index];

            return p90;
        }
    }
}
