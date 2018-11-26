using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Helpers
{
    public static class Globals
    {
        private static List<KeyValuePair<string, int>> _INPUT_INDEXES;
        private static List<KeyValuePair<string, int>> _OUTPUT_INDEXES;

        public static string OPC_SERVER_NAME = "Matrikon.OPC.AllenBradleyPLCs.1";
        public static string[] INPUT_TAGS;
        public static List<KeyValuePair<string, int>> INPUT_INDEXES
        {
            get
            {
                return _INPUT_INDEXES;
            }
        }
        public static string[] OUTPUT_TAGS;
        public static List<KeyValuePair<string, int>> OUTPUT_INDEXES
        {
            get
            {
                return _OUTPUT_INDEXES;
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

        public static void SetUpTagsAndIndexes()
        {
             string[] _INPUT_TAGS = {
            "AB:RYAN_HIEFFICIENCYBAR:DINT:IPLC_STATUS.VALUE",           // 1 - PLC/iPLC_STATUS
            "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ID.VALUE",      // 2 - ORDER/ID
            "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ISTATUS.VALUE",      // 3 - ORDER/STATUS
            "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ITRAY_NUMBER.VALUE",      // 4 - ORDER/TRAY_NUMBER
            "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.IQR_CODE.VALUE",      // 5 - ORDER/QR_CODE
        };
            _INPUT_INDEXES = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("iPLC_STATUS", 1),
                new KeyValuePair<string, int>("ORDER_ID", 2),
                new KeyValuePair<string, int>("ORDER_STATUS", 3),
                new KeyValuePair<string, int>("TRAY_NUMBER", 4),
                new KeyValuePair<string, int>("QR_CODE", 5),
            };
            string[] _OUTPUT_TAGS = {
                "AB:RYAN_HIEFFICIENCYBAR:DINT:IPLC_STATUS.VALUE",           // 1 - PLC/iPLC_STATUS
            "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ID.VALUE",      // 2 - ORDER/ID
            "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ISTATUS.VALUE",      // 3 - ORDER/STATUS
        };
            _OUTPUT_INDEXES = new List<KeyValuePair<string, int>>() {
                new KeyValuePair<string, int>("iPLC_STATUS", 1),
                new KeyValuePair<string, int>("ORDER_ID", 2),
                new KeyValuePair<string, int>("ORDER_STATUS", 3),
                };

            INPUT_TAGS = _INPUT_TAGS;
            OUTPUT_TAGS = _OUTPUT_TAGS;
        }
    }
}
