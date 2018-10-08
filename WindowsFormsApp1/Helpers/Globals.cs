using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Helpers
{
    public static class Globals
    {
        public static string OPC_SERVER_NAME = "Matrikon.OPC.AllenBradleyPLCs.1";
        public static string[] INPUT_TAGS = {
            "AB:RYAN_HIEFFICIENCYBAR:DINT:IPLC_STATUS.VALUE",           // 0 - PLC/iPLC_STATUS
        };
        public static string[] OUTPUT_TAGS = {
            "AB:RYAN_HIEFFICIENCYBAR:DINT:IPLC_STATUS.VALUE",           // 0 - PLC/iPLC_STATUS
        };
        public static List<KeyValuePair<string, int>> INPUT_INDEXES
        {
            get
            {
                var _iNPUT_INDEXES = new List<KeyValuePair<string, int>>() {
                new KeyValuePair<string, int>("iPLC_STATUS", 1),
                //new KeyValuePair<string, int>("B", 2),
                };
                return _iNPUT_INDEXES;
            }
        }
        public static List<KeyValuePair<string, int>> OUTPUT_INDEXES
        {
            get
            {
                var _oUTPUT_INDEXES = new List<KeyValuePair<string, int>>() {
                new KeyValuePair<string, int>("iPLC_STATUS", 1),
                //new KeyValuePair<string, int>("B", 2),
                };
                return _oUTPUT_INDEXES;
            }
        }

        internal static int GetTotalNumberOfInputTags()
        {
            int tagsNumber = 0;
            tagsNumber += INPUT_TAGS.Count();
            return tagsNumber;
        }
        internal static int GetTotalNumberOfOutputTags()
        {
            int tagsNumber = 0;

            tagsNumber += OUTPUT_TAGS.Count();
            return tagsNumber;
        }

        //public static string INPUT_TAG_1_NAME = "AB:TESTCOMMS_DB_PLC:BOOL:OPC_INPUT_BOOL.VALUE";
        //public static string INPUT_TAG_2_NAME = "AB:TESTCOMMS_DB_PLC:DINT:OPC_INPUT_INT.VALUE";
        //public static string OUTPUT_TAG_1_NAME = "AB:TESTCOMMS_DB_PLC:BOOL:OPC_OUTPUT_BOOL.VALUE";
        //public static string OUTPUT_TAG_2_NAME = "AB:TESTCOMMS_DB_PLC:DINT:OPC_OUTPUT_INT.VALUE";
        //public static string OUTPUT_TAG_3_NAME = "AB:TESTCOMMS_DB_PLC:DINT:OPC_RANDOMOUTPUT.VALUE";
    }
}
