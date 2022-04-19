// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK;
using Microsoft.Performance.SDK.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SampleCustomDataSource.Tables.Metadata
{
    //
    // This is a sample Metadata table for .txt files
    // Metadata tables are used to expose information about the data being processed, not the actual data being processed.
    // Metadata could be "number of events in the file," "file size," or any other number of things that describes the data being processed.
    // In this sample table, we expose three columns: File Name, Line Count and Word Count.
    //

    //
    // In order for the CustomDataSourceBase to understand your metadata table, 
    // pass in isMetadataTable = true in the TableDescriptor constructor for this table.
    //

    [Table]             
    //
    // Have the MetadataTable inherit the custom TableBase class
    //
    public sealed class FileStatsMetadataTable
        : TableBase
    {
        public FileStatsMetadataTable(IReadOnlyList<Tuple<Timestamp, ThreeColumns>> lines)
            : base(lines)
        {
        }

        public static TableDescriptor TableDescriptor = new TableDescriptor(
            Guid.Parse("{40AF86E5-0DF8-47B1-9A01-1D6C3529B75B}"), // The GUID must be unique across all tables
            "File Stats",                                         // The MetadataTable must have a name
            "Statistics for text files",                          // The MetadataTable must have a description
            TableDescriptor.DefaultCategory,                      // A category is optional. It useful for grouping different types of tables in the viewer's UI.
            true);                                                // Marks this table as a metadata table

        //
        // Declare columns here. You can do this using the ColumnConfiguration class. 
        // It is possible to declaratively describe the table configuration as well. Please refer to our Advanced Topics Wiki for more information.
        //
        // The Column metadata describes each column in the table. 
        // Each column must have a unique GUID and a unique name. The GUID must be unique globally, but the name only unique within the table.
        //
        // The UIHints provides some hints to a viewer (such as WPA) on how to render the column. 
        // In this sample, we are simply saying to allocate at least 80 units of width.
        //

        private static readonly ColumnConfiguration FirstColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("{2604E009-F47D-4A22-AA4F-B148D1C26553}"), "FirstCol", "It is first column."),
            new UIHints { Width = 80 });

        private static readonly ColumnConfiguration SecondColumn = new ColumnConfiguration(
           new ColumnMetadata(new Guid("{C499AF57-64D1-47A9-8550-CF24D6C9615D}"), "SecondCol", "It is second column."),
           new UIHints { Width = 80 });


        public override void Build(ITableBuilder tableBuilder)
        {

            var lines = Projection.Index(Lines);
            var columns = Projection.Project(lines, a => a.Item2);



            tableBuilder.SetRowCount(Lines.Count)
                .AddColumn(FirstColumn, columns.Compose(a => a.FirstColumn))
                .AddColumn(SecondColumn, columns.Compose(a => a.FirstColumn));
        }
    }
}
