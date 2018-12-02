using OPCtoMongoDBService.Models;
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
        public static List<Tag> INPUT_TAGS;
        public static List<Tag> OUTPUT_TAGS;

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
            int index = 0;
            INPUT_TAGS = new List<Tag>();
            INPUT_TAGS.Add(new Tag(++index, "iPLC_STATUS", "AB:RYAN_HIEFFICIENCYBAR:DINT:IPLC_STATUS.VALUE"));
            INPUT_TAGS.Add(new Tag(++index, "ORDER_ID", "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ID.VALUE"));
            INPUT_TAGS.Add(new Tag(++index, "ORDER_STATUS", "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ISTATUS.VALUE"));
            INPUT_TAGS.Add(new Tag(++index, "TRAY_NUMBER", "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ITRAY_NUMBER.VALUE"));
            INPUT_TAGS.Add(new Tag(++index, "QR_CODE", "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.IQR_CODE.VALUE"));

            int totalDrinks = 4;
            int totalGarnishes = 3;
            int totalIngredients = 8;
            for (int drinkIndex = 1; drinkIndex <= totalDrinks; drinkIndex++)
            {
                INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_ID",
                    "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".ID.VALUE"));
                INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_ICE",
                    "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".IICE.VALUE"));
                INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_PREP",
                    "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".IPREP.VALUE"));
                INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_QUANTITY",
                    "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".IQUANTITY.VALUE"));
                INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_STATUS",
                    "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".ISTATUS.VALUE"));

                for (int garnishIndex = 1; garnishIndex <= totalGarnishes; garnishIndex++)
                {
                    INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_GARNISH_" + garnishIndex + "_ID",
                        "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".STGARNISH_" + garnishIndex + ".ID.VALUE"));
                    INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_GARNISH_" + garnishIndex + "_RATIO",
                        "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".STGARNISH_" + garnishIndex + ".IRATIO.VALUE"));
                }

                INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_GLASS_ID",
                    "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".STGLASS.ID.VALUE"));
                INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_GLASS_STATUS",
                    "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".STGLASS.ISTATUS.VALUE"));

                for (int ingredientIndex = 1; ingredientIndex <= totalIngredients; ingredientIndex++)
                {
                    INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_INGREDIENT_" + ingredientIndex + "_ID",
                        "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".STINGREDINET_" + ingredientIndex + ".ID.VALUE"));
                    INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_INGREDIENT_" + ingredientIndex + "_PLACE_NO",
                        "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".STINGREDINET_" + ingredientIndex + ".IPLACENUMBER.VALUE"));
                    INPUT_TAGS.Add(new Tag(++index, "DRINK_" + drinkIndex + "_INGREDIENT_" + ingredientIndex + "_UNIT",
                        "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.STDRINK_" + drinkIndex + ".STINGREDINET_" + ingredientIndex + ".IUNIT.VALUE"));
                }
            }

            index = 0;
            OUTPUT_TAGS = new List<Tag>();
            OUTPUT_TAGS.Add(new Tag(++index, "iPLC_STATUS",
                        "AB:RYAN_HIEFFICIENCYBAR:DINT:IPLC_STATUS.VALUE"));
            OUTPUT_TAGS.Add(new Tag(++index, "ORDER_ID",
                        "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ID.VALUE"));
            OUTPUT_TAGS.Add(new Tag(++index, "ORDER_STATUS",
                        "AB:RYAN_HIEFFICIENCYBAR:ST_ORDER:OPC_ORDER.ISTATUS.VALUE"));
        }
    }
}
