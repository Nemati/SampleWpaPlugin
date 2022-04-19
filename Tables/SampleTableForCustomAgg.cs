// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK;
using Microsoft.Performance.SDK.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SampleCustomDataSource.Tables
{
    //
    // This is a sample regular table that counts the characters in each word of a .txt file.
    // It also provides a column (word) that can be grouped on.
    //

    //
    // Add a Table attribute in order for the CustomDataSourceBase to understand your table.
    // 

    [Table]                      

    //
    // Have the MetadataTable inherit the custom TableBase class
    //

    public sealed class SampleTableForCustomAgg
        : TableBase
    {
        public SampleTableForCustomAgg(IReadOnlyList<Tuple<Timestamp, ThreeColumns>> lines)
            : base(lines)
        {
        }

        public static TableDescriptor TableDescriptor => new TableDescriptor(
            Guid.Parse("{E122471E-25A6-4F7F-BE6C-E62774FD0410}"), // The GUID must be unique across all tables
            "Custom Aggregation example.",                                         // The Table must have a name
            "To test",                               // The Table must have a description
            "Test");                                             // A category is optional. It useful for grouping different types of tables in the viewer's UI.

        //
        // Declare columns here. You can do this using the ColumnConfiguration class. 
        // It is possible to declaratively describe the table configuration as well. Please refer to our Advanced Topics Wiki page for more information.
        //
        // The Column metadata describes each column in the table. 
        // Each column must have a unique GUID and a unique name. The GUID must be unique globally; the name only unique within the table.
        //
        // The UIHints provides some hints to a viewer (such as WPA) on how to render the column. 
        // In this sample, we are simply saying to allocate at least 80 units of width.
        //

        private static readonly ColumnConfiguration FirstColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("{91C69594-4079-4478-B1A0-3FEC01641D8D}"), "FirstColumn", "First."),
            new UIHints { Width = 80 });

        private static readonly ColumnConfiguration SecondColumn = new ColumnConfiguration(
           new ColumnMetadata(new Guid("{A669FC83-BF61-4604-8BB2-44E66FCA7062}"), "SecondColumn", "Seond."),
           new UIHints { Width = 80 });

        private static readonly ColumnConfiguration ThirdColumn = new ColumnConfiguration(
           new ColumnMetadata(new Guid("{A669FC83-BF63-4604-8BB3-44E66FCA7063}"), "ThirdColumn", "Third."),
           new UIHints { Width = 80 });

        private static readonly ColumnConfiguration TimeColumn = new ColumnConfiguration(
         new ColumnMetadata(new Guid("{54B4A016-9F78-4BAE-A0AB-AFDFBF33C3F1}"), "Time", "The time when the number is written to the file."),
         new UIHints { Width = 80 });

        public override void Build(ITableBuilder tableBuilder)
        {
            var baseProjection = Projection.Index(this.Lines);            
            var firstColumn = baseProjection.Compose(x => x.Item2.FirstColumn);
            var secondColumn = baseProjection.Compose(x => x.Item2.SecondColumn);
            var thirdColumn = baseProjection.Compose(x => x.Item2.ThirdColumn);
            var timeProjection = baseProjection.Compose(x => x.Item1);

            IProjectionWithCustomAggregation<int, int> projectionWithCustomAggregation = new ProjectionWithCustomAggregation<int, int>(firstColumn, thirdColumn);

            var config = new TableConfiguration("Custom Aggregation1")
            {
                Columns = new[]
              {
                    FirstColumn,
                    TableConfiguration.PivotColumn,
                    SecondColumn,
                    ThirdColumn,
                    TableConfiguration.GraphColumn,
                    TimeColumn,
                },
            };

            config.AddColumnRole(ColumnRole.StartTime, TimeColumn);

            tableBuilder.AddTableConfiguration(config)
                .SetDefaultTableConfiguration(config)
                .SetRowCount(this.Lines.Count)
                .AddColumnWithAggregation(FirstColumn, projectionWithCustomAggregation)
                .AddColumn(SecondColumn, secondColumn)
                .AddColumn(ThirdColumn, thirdColumn)
                .AddColumn(TimeColumn, timeProjection);
        }
    }
}
