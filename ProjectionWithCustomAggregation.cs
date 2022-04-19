using Microsoft.Performance.SDK.Processing;
using SampleAddIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SampleCustomDataSource
{
    public class ProjectionWithCustomAggregation<T, TAgg> : IProjectionWithCustomAggregation<int, int>
    {
        IReadOnlyList<IAggregationModeClass> aggregationModeClasses;
        private readonly IProjection<int, int> projection;
        private readonly IProjection<int, string> keyProjection;

        public ProjectionWithCustomAggregation(IProjection<int, int> projection, IProjection<int, string> keyProjection)
        {
            this.projection = projection;
            this.keyProjection = keyProjection;

            aggregationModeClasses = new List<IAggregationModeClass>()
            {
                new P50(),
                new P50WithKey(),
                new P90()
            };
        }

        public int this[int value] => projection[value];

        public Type AggregateType => typeof(int);

        public Type SourceType => typeof(int);

        public Type ResultType => typeof(int);

        public IReadOnlyList<IAggregationModeClass> ExtraAggregationMode => aggregationModeClasses;

        public int AggrgateRows(int[] rowIndices, int start, int count, IAggregationModeClass aggregationModeClass)
        {
            return (int)aggregationModeClass.AggrgateRows<int, string>(rowIndices, start, count,
                projection, keyProjection, (a, b) => a + b, 0);
        }
    }
}
