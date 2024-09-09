using Common;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.Reports
{
    public class DeviceVsGBComparisonReportDL
    {
        DBManager oDbm;
        public DeviceVsGBComparisonReportDL()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }

        public DataTable BindFgItemCode()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(oDbm.ConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_DEVICE_VS_GB_REPORT";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDFGITEMCODE");

                    con.Open();

                    SqlDataReader sqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    dt.Load(sqlDataReader);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }

            return dt;
        }

        public DataTable GetHeaderDetail(string fgItemCode)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(oDbm.ConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_DEVICE_VS_GB_REPORT";
                    cmd.Parameters.AddWithValue("@TYPE", "BINDHEADERDETAIL");
                    cmd.Parameters.AddWithValue("@FGITEMCODE", fgItemCode);

                    con.Open();

                    SqlDataReader sqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    dt.Load(sqlDataReader);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }

            return dt;
        }

        public DataTable GetReport(string fgItemCode, string fromDate, string toDate)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(oDbm.ConnectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_DEVICE_VS_GB_REPORT";
                    cmd.Parameters.AddWithValue("@TYPE", "GETREPORT");
                    cmd.Parameters.AddWithValue("@FGITEMCODE", fgItemCode);
                    cmd.Parameters.AddWithValue("@FROMDATE", fromDate);
                    cmd.Parameters.AddWithValue("@TODATE", toDate);

                    con.Open();

                    SqlDataReader sqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    dt.Load(sqlDataReader);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }

            return dt;
        }
    }
}
