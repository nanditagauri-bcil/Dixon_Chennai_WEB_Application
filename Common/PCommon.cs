using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class PCommon
    {
        public static object BAUDRATE;
        public static object DATABITS;
        public static BcilLib.BcilLogger mBcilLogger;
        public static string PORTNAME;

        public static string sSiteCode { get; set; }
        public static string sUseNetworkPrinter { get; set; }
        public static string sPrinterPort { get; set; }
        public static string sClientPrintingPath { get; set; }
        public static string sServerSidePrintingPath { get; set; }
        public static string sFileNam { get; set; }
        public static string sFilePrintingPath { get; set; }

        private static readonly char[] Base34Chars = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ".ToCharArray();

        /// <summary>
        /// USED IN SQL DB PROVIEDR
        /// </summary>
        private static string _strsqlcon = ConfigurationManager.ConnectionStrings["INE_Conn"].ConnectionString;
        public static string[] myList;

        public static string StrSqlCon
        {
            get { return _strsqlcon; }
            set
            {
                _strsqlcon = value;
            }
        }

        //public static DataTable ConvertCSVtoDataTable(string strFilePath)
        //{
        //    DataTable dt = new DataTable();
        //    using (StreamReader sr = new StreamReader(strFilePath))
        //    {
        //        string[] headers = sr.ReadLine().Split(',');
        //        foreach (string header in headers)
        //        {
        //            dt.Columns.Add(header);
        //        }
        //        while (!sr.EndOfStream)
        //        {
        //            string[] rows = sr.ReadLine().Split(',');
        //            DataRow dr = dt.NewRow();
        //            for (int i = 0; i < headers.Length; i++)
        //            {
        //                dr[i] = rows[i].Trim().TrimEnd().TrimStart();
        //            }
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    return dt;
        //}

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string headerLine = sr.ReadLine();
                string[] headers = headerLine.Split(',');

                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Trim());
                }

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    string[] values = SplitCsvLine(line, headers.Length);

                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length && i < values.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(values[i].Trim()))
                        {
                            dr[i] = values[i].Trim();
                        }
                        else
                        {
                            dr[i] = DBNull.Value;
                        }
                    }

                    dt.Rows.Add(dr);

                    //string[] rows = sr.ReadLine().Split(',');
                    //DataRow dr = dt.NewRow();

                    //for (int i = 0; i < headers.Length && i < rows.Length; i++)
                    //{
                    //    dr[i] = rows[i].Trim();
                    //}

                    //dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        // Helper method to split a CSV line into an array of values
        private static string[] SplitCsvLine(string line, int expectedValues)
        {
            string[] values = new string[expectedValues];
            int currentIndex = 0;
            bool inQuotes = false;
            int start = 0;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values[currentIndex++] = RemoveQuotes(line.Substring(start, i - start));
                    start = i + 1;
                }
            }

            values[currentIndex++] = RemoveQuotes(line.Substring(start));

            // If the number of values is less than expected, fill the remaining values with empty strings
            while (currentIndex < expectedValues)
            {
                values[currentIndex++] = string.Empty;
            }

            return values;
        }

        // Helper method to remove surrounding quotes from a string
        private static string RemoveQuotes(string value)
        {
            if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"')
            {
                return value.Substring(1, value.Length - 2);
            }
            return value;
        }

        public static DataTable ConvertCSVtoDataTable_ToolData(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    if (header == "CALIBRATIONDATE" || header == "ALERTDATE")
                    {
                        dt.Columns.Add(header, typeof(DateTime));
                    }
                    else
                    {
                        dt.Columns.Add(header);
                    }
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        if (i == 10 || i == 11)
                        {
                            try
                            {
                                dr[i] = Convert.ToDateTime(rows[i].Trim().TrimEnd().TrimStart());
                            }
                            catch (Exception)
                            {
                                if (i == 10)
                                {
                                    throw new Exception("File contains invalid value in the column Calibration date,Please check the date ");
                                }
                                else
                                {
                                    throw new Exception("File contains invalid value in the column aleart date column, Please check the date");
                                }
                            }
                        }
                        else
                        {
                            dr[i] = rows[i].Trim().TrimEnd().TrimStart();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static DataSet ConvertCSVtoDataTable_multiColumn(string strFilePath)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dtWifiBT = new DataTable();
            DataSet ds = new DataSet();
            DataTable dtIMEI = new DataTable();
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                if (str != "")
                {
                    string[] rows = str.Split(',');
                    DataRow dr = dtWifiBT.NewRow();
                    DataRow dr1 = dtIMEI.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                        dr1[i] = rows[i];
                    }
                    if (dr.ToString() == "")
                    {
                        continue;
                    }
                    dtIMEI.Rows.Add(dr1);
                }
            }
            dtIMEI.TableName = "IMEIData";
            dtIMEI.AcceptChanges();
            dtWifiBT.TableName = "WIFIData";
            dtWifiBT.AcceptChanges();
            //dtGoogleKey.TableName = "GoogleKey";
            //dtGoogleKey.AcceptChanges();
            dtIMEI.Columns.RemoveAt(1);
            dtIMEI.Columns.RemoveAt(1);
            dtIMEI.AcceptChanges();
            //dtIMEI.Columns.RemoveAt(1);
            //dtIMEI.AcceptChanges();

            dtWifiBT.Columns.RemoveAt(0);
            dtWifiBT.AcceptChanges();
            //dtWifiBT.Columns.RemoveAt(2);
            //dtWifiBT.AcceptChanges();

            //dtGoogleKey.Columns.RemoveAt(0);
            //dtGoogleKey.AcceptChanges();
            //dtGoogleKey.Columns.RemoveAt(0);
            //dtGoogleKey.AcceptChanges();
            //dtGoogleKey.Columns.RemoveAt(0);
            //dtGoogleKey.AcceptChanges();

            ds.DataSetName = "DS";

            ds.Tables.Add(dtIMEI);
            ds.AcceptChanges();

            ds.Tables.Add(dtWifiBT);
            ds.AcceptChanges();
            //ds.Tables.Add(dtGoogleKey);
            //ds.AcceptChanges();
            return ds;
        }

        public static DataTable ConvertArrayDataTableSingleColumn(ArrayList sIMEI)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SERIAL_NUMBER");
            dt.Columns.Add("GRPONSN");
            dt.Columns.Add("MAC");
            dt.Columns.Add("MAC_2");
            dt.Columns.Add("KEY_PART_NO");
            dt.Columns.Add("WIFI");
            dt.Columns.Add("WIRELESS_SSID");
            dt.Columns.Add("PRE_PASSWORD");
            dt.Columns.Add("IMEI_1");
            dt.Columns.Add("ACS_DATA");
            dt.Columns.Add("HDCP");
            dt.Columns.Add("COL1");
            dt.Columns.Add("COL2");
            dt.Columns.Add("COL3");
            dt.Columns.Add("COL4");
            dt.Columns.Add("COL5");
            dt.Columns.Add("COL6");
            dt.Columns.Add("COL7");
            dt.Columns.Add("COL8");
            dt.Columns.Add("COL9");
            dt.Columns.Add("BT_MAC");
            for (int f = 0; f < sIMEI.Count; f++)
            {
                dt.Rows.Add(sIMEI[f].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            return dt;
        }

        public static string ToCSV(DataTable dt)
        {
            string csv = string.Empty;
            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for CSV file.
                csv += column.ColumnName + ',';
            }
            //Add new line.
            csv += "\r\n";
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }
                //Add new line.
                csv += "\r\n";
            }
            return csv;
        }

        public static string DataEncrypt(string clearText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return clearText;
        }

        #region Custom AplhaNumeric Serial Conversion
        public static string ConvertToCustomAlphaNumeric(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            // Try parsing the input string to an integer (ignoring leading zeros)
            if (!long.TryParse(input, out long number) || number < 0)
                return null;

            int targetLength = input.Length;

            // Base conversion
            string result = "";
            if (number == 0)
            {
                result = "0";
            }
            else
            {
                while (number > 0)
                {
                    int remainder = (int)(number % 34);
                    result = Base34Chars[remainder] + result;
                    number /= 34;
                }
            }

            return result;
        }
        #endregion
    }
}
