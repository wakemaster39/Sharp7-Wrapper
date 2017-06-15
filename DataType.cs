using System;
using System.Collections.Generic;
using System.Text;

namespace Sharp7
{
    public enum DataType
    {
        Inputs = S7Consts.S7AreaPE,
        Outputs = S7Consts.S7AreaPA,
        Merkers = S7Consts.S7AreaMK,
        DB = S7Consts.S7AreaDB,
        Counters = S7Consts.S7AreaCT,
        Timers = S7Consts.S7AreaTM
    }
}
