using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCustomDataSource
{

    public class ThreeColumns
    {
        public ThreeColumns(int first, int second, string third)
        {
            FirstColumn = first;
            SecondColumn = second;
            ThirdColumn = third;
        }

        public int FirstColumn { get; }

        public int SecondColumn { get; }

        public string ThirdColumn { get; }
    }
}
