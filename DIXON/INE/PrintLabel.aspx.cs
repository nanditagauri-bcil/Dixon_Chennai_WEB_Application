//using Common;
//using Ionic.Zip;
//using System;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;

//namespace DIXON.INE
//{
//    public partial class PrintLabel : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            DownloadFile();
//        }
//        protected void DownloadFile()
//        {
//            string sApplicationPrintingFullFilePath = string.Empty;
//            try
//            {
//                HttpResponse Response = HttpContext.Current.Response;
//                string sHeaderName = string.Empty;
//                string sUserID = Session["UserID"].ToString();
//                string sLineCode = Session["LINECODE"].ToString();

//                string sSearchParaMeter = "BCI" + "_" + sUserID + "_" + sLineCode + "_" + PCommon.sSiteCode;
//                var SearchFile = Directory.GetFiles(PCommon.sFilePrintingPath).Where(file => Regex.IsMatch(file, sSearchParaMeter));
//                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData,
//                    System.Reflection.MethodBase.GetCurrentMethod().Name, "File Name : " + sSearchParaMeter);
//                String[] files = SearchFile.ToArray();
//                using (ZipFile zipFile = new ZipFile())
//                {
//                    foreach (string item in files)
//                    {
//                        sHeaderName = item.ToString();
//                        sApplicationPrintingFullFilePath = PCommon.sFilePrintingPath + "\\" + sHeaderName;
//                        zipFile.AddFile(sHeaderName, string.Empty);
//                        CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData,
//                            System.Reflection.MethodBase.GetCurrentMethod().Name, "File Name : " + sSearchParaMeter + ", File Added : " + item.ToString());
//                    }
//                    Response.ClearContent();
//                    Response.ClearHeaders();
//                    //Set zip file name  
//                    Response.AppendHeader("content-disposition", "attachment; filename=DIXONPrinting_" + sSearchParaMeter + "_" + DateTime.Now.ToString("ddMMyyyyhhmmsstt") + ".zip");
//                    zipFile.CompressionMethod = CompressionMethod.BZip2;
//                    zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
//                    //Save the zip content in output stream  
//                    zipFile.Save(Response.OutputStream);
//                }
//                foreach (string file in files)
//                {
//                    FileInfo fi = new FileInfo(file);
//                    fi.Delete();
//                }
//            }
//            catch (Exception ex)
//            {
//                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
//            }
//        }
//    }
//}



using Common;
using Ionic.Zip;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DIXON.INE
{
    public partial class PrintLabel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DownloadFile();
        }

        protected void DownloadFile()
        {
            try
            {
                // =========================================================================
                // SCENARIO 1: NEW - In-Memory Download (From Split Reel / Session)
                // =========================================================================
                if (Session["DOWNLOAD_BYTES"] != null && Session["DOWNLOAD_NAME"] != null)
                {
                    byte[] fileBytes = (byte[])Session["DOWNLOAD_BYTES"];
                    string fileName = Session["DOWNLOAD_NAME"].ToString();

                    // Clear Session
                    Session["DOWNLOAD_BYTES"] = null;
                    Session["DOWNLOAD_NAME"] = null;

                    // Send the file
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.BinaryWrite(fileBytes);
                    Response.Flush();

                    // USE THIS instead of Response.End() to avoid ThreadAbortException
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Response.SuppressContent = true;
                    return;
                }

                // =========================================================================
                // SCENARIO 2: LEGACY - Physical File Search (Existing Logic)
                // =========================================================================
                string sApplicationPrintingFullFilePath = string.Empty;
                HttpResponse ResponseObj = HttpContext.Current.Response; // Renamed to avoid hiding local var
                string sHeaderName = string.Empty;

                if (Session["UserID"] == null || Session["LINECODE"] == null)
                {
                    return;
                }

                string sUserID = Session["UserID"].ToString();
                string sLineCode = Session["LINECODE"].ToString();

                string sSearchParaMeter = "BCI" + "_" + sUserID + "_" + sLineCode + "_" + PCommon.sSiteCode;

                if (!Directory.Exists(PCommon.sFilePrintingPath))
                {
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, "Printing Path not found: " + PCommon.sFilePrintingPath);
                    return;
                }

                var SearchFile = Directory.GetFiles(PCommon.sFilePrintingPath).Where(file => Regex.IsMatch(file, sSearchParaMeter));

                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, "File Name : " + sSearchParaMeter);

                String[] files = SearchFile.ToArray();

                if (files.Length > 0)
                {
                    using (ZipFile zipFile = new ZipFile())
                    {
                        foreach (string item in files)
                        {
                            sHeaderName = Path.GetFileName(item);
                            sApplicationPrintingFullFilePath = item;

                            zipFile.AddFile(item, string.Empty);

                            CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData,
                                System.Reflection.MethodBase.GetCurrentMethod().Name, "File Name : " + sSearchParaMeter + ", File Added : " + item.ToString());
                        }

                        ResponseObj.ClearContent();
                        ResponseObj.ClearHeaders();

                        string zipName = "DIXONPrinting_" + sSearchParaMeter + "_" + DateTime.Now.ToString("ddMMyyyyhhmmsstt") + ".zip";
                        ResponseObj.AppendHeader("content-disposition", "attachment; filename=" + zipName);

                        zipFile.CompressionMethod = CompressionMethod.BZip2;
                        zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;

                        zipFile.Save(ResponseObj.OutputStream);
                    }

                    foreach (string file in files)
                    {
                        try
                        {
                            if (File.Exists(file)) File.Delete(file);
                        }
                        catch
                        {
                        }
                    }

                    ResponseObj.End();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}