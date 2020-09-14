using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIN.GYGL.ODBC;

namespace myConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");

            string strKsBh = "112";
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine("AND ( RU_DP_ID='" + strKsBh + "'");      //科室编号
            if (strKsBh != WebConfig.GetValue("SupplyRoomID"))
            {
                sbSql.AppendLine("OR RU_DP_ID= '" + WebConfig.GetValue("SupplyRoomID") + "' )");               //科室编号
            }
            string ruDp_Id = "22323";
            sbSql.AppendLine("    AND (RU_INFO_MST.RU_DP_ID='" + ruDp_Id + "' OR RU_INFO_MST.RU_DP_ID='" + WebConfig.GetValue("SupplyRoomID") + "') ");
            sbSql.AppendLine(" AND (RU_EDIT_FLAG = 0 or RU_EDIT_FLAG is null) ");

            sbSql.AppendLine("     C.RU_DP_ID<> '" + WebConfig.GetValue("SupplyRoomID") + "'");
            sbSql.AppendLine("left JOIN PACKAGE_INFO_TBL D   ON C.RDD_RU_ID = D.PK_PACKAGE_ID  AND RRD_RU_ID = D.PK_RU_ID");
            sbSql.AppendLine("join RU_INFO_MST on RU_ID=RRD_RU_ID and RU_DP_ID='" + WebConfig.GetValue("SupplyRoomID") + "' and RU_ID='" + ruDp_Id+"'");

            sbSql.AppendLine(" and   UR_DEPART_ID = '" + WebConfig.GetValue("SupplyRoomID") + "'");
            sbSql.AppendLine("WHERE");
            sbSql.AppendLine("    UR_DEPART_ID = '" + WebConfig.GetValue("SupplyRoomID") + "'");
            sbSql.AppendLine(" AND (RU_Disable =0 or RU_Disable is null)");

            sbSql.AppendLine("    AND (RU_INFO_MST.RU_DP_ID='" + ruDp_Id + "' OR RU_INFO_MST.RU_DP_ID='" + WebConfig.GetValue("SupplyRoomID") + "') ");

            string ksbh = WebConfig.GetValue("SupplyRoomID");
            //科室代码
            if (ruDp_Id != "" && ruDp_Id != ksbh)
            {
                sbSql.AppendLine("    AND (RU_INFO_MST.RU_DP_ID='" + ruDp_Id + "' OR RU_INFO_MST.RU_DP_ID='" + WebConfig.GetValue("SupplyRoomID") + "') ");
            }

        }
    }
}

namespace SIN.GYGL.ODBC
{
    public class WebConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            return name;
        }
    }
}
