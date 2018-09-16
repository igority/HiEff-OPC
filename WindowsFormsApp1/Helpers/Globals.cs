using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Helpers
{
    public static class Globals
    {
        public static string OPC_SERVER_NAME = "Matrikon.OPC.AllenBradleyPLCs.1";
        public static string INPUT_TAG_1_NAME = "AB:TESTCOMMS_DB_PLC:BOOL:OPC_INPUT_BOOL.VALUE";
        public static string INPUT_TAG_2_NAME = "AB:TESTCOMMS_DB_PLC:DINT:OPC_INPUT_INT.VALUE";
        public static string OUTPUT_TAG_1_NAME = "AB:TESTCOMMS_DB_PLC:BOOL:OPC_OUTPUT_BOOL.VALUE";
        public static string OUTPUT_TAG_2_NAME = "AB:TESTCOMMS_DB_PLC:DINT:OPC_OUTPUT_INT.VALUE";
        public static string OUTPUT_TAG_3_NAME = "AB:TESTCOMMS_DB_PLC:DINT:OPC_RANDOMOUTPUT.VALUE";
    }
}
