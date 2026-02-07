using Common;
using DataLayer;
using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Web;

namespace BusinessLayer
{
    public class BL_ReelSplitPrinting
    {
        DL_ReelSplitPrinting dlobj = null;

        public DataTable BindReelBarcode(string sSiteCode)
        {
            DataTable dtReelBarcode = new DataTable();
            dlobj = new DL_ReelSplitPrinting();
            try
            {
                dtReelBarcode = dlobj.BindReelBarcode(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReelBarcode;
        }

        public DataTable ValidateReelBarcode(string sReelID, string sSiteCode)
        {
            DataTable dtReelBarcode = new DataTable();
            dlobj = new DL_ReelSplitPrinting();
            try
            {
                dtReelBarcode = dlobj.ValidateReelBarcode(sReelID, sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReelBarcode;
        }

        public string ChildLabelPrint(string _ReelID, string sPrintedBy, decimal dUpdatedQty, string sLineCode, string sSiteCode)
        {
            dlobj = new DL_ReelSplitPrinting();
            string sResult = string.Empty;

            try
            {
                DataTable dt = dlobj.ChildLabelPrinting(_ReelID, dUpdatedQty, sPrintedBy, sSiteCode, sLineCode);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return "N~No result returned from server. Please try again.";
                }

                DataRow row = dt.Rows[0];

                sResult = row[0].ToString();
                if (sResult.StartsWith("N~") || sResult.StartsWith("ERROR~"))
                {
                    return sResult;
                }

                sResult = SafeGetString(row, "RESULT");
                string childBarcode = SafeGetString(row, "CHILD_BARCODE");
                string childPrn = SafeGetString(row, "CHILD_PRN");
                string parentBarcode = SafeGetString(row, "PARENT_BARCODE");
                string parentPrn = SafeGetString(row, "PARENT_PRN");

                if (string.IsNullOrWhiteSpace(childBarcode) || string.IsNullOrWhiteSpace(childPrn))
                {
                    return "N~Split successful, but label data (Child) is missing";
                }

                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, MethodBase.GetCurrentMethod().Name,
                    $"Split Success. Child: {childBarcode}, Parent: {parentBarcode}");

                DownloadZippedPRNs(childBarcode, childPrn, parentBarcode, parentPrn, sPrintedBy, sLineCode, sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name, ex.Message);
                sResult = "ERROR~" + ex.Message;
            }

            return sResult;
        }

        private string SafeGetString(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName)) return string.Empty;
            return row[columnName] != DBNull.Value ? row[columnName].ToString() : string.Empty;
        }

        private void DownloadZippedPRNs(string childBarcode, string childPrn, string parentBarcode, string parentPrn,
            string sUserID, string sLineCode, string sSiteCode)
        {
            string cleanChildBarcode = CleanFileName(childBarcode.Split(' ')[0]); // Take first part if space exists

            string zipFileName = $"BCI_{CleanFileName(sUserID)}_{CleanFileName(sLineCode)}_{CleanFileName(sSiteCode)}_{cleanChildBarcode}.zip";

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Buffer = true;
            response.ContentType = "application/zip";
            response.AddHeader("content-disposition", $"attachment;filename={zipFileName}");
            response.Cache.SetCacheability(HttpCacheability.NoCache);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    AddFileToZip(archive, $"{CleanFileName(childBarcode)}.prn", childPrn);

                    if (!string.IsNullOrWhiteSpace(parentPrn) && !string.IsNullOrWhiteSpace(parentBarcode))
                    {
                        AddFileToZip(archive, $"{CleanFileName(parentBarcode)}.prn", parentPrn);
                    }
                }

                memoryStream.Position = 0;
                memoryStream.CopyTo(response.OutputStream);
            }

            response.Flush();
            response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Graceful exit
        }

        private void AddFileToZip(ZipArchive archive, string fileName, string content)
        {
            var entry = archive.CreateEntry(fileName);
            using (var streamWriter = new StreamWriter(entry.Open()))
            {
                streamWriter.Write(content);
            }
        }

        private string CleanFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "Unknown";
            return name.Replace('/', '-').Replace(':', '-').Replace('*', '-').Replace('?', '-').Replace('\\', '-').Trim();
        }
    }
}