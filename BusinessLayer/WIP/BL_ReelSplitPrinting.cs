using Common;
using DataLayer;
using System;
using System.Data;
using System.Reflection;

namespace BusinessLayer
{
    public class BL_ReelSplitPrinting
    {
        DL_ReelSplitPrinting dlobj = null;
        public DataTable BindINELPartNo(string sSiteCode)
        {
            DataTable dtINELPartNo = new DataTable();
            dlobj = new DL_ReelSplitPrinting();
            try
            {
                dtINELPartNo = dlobj.BINDINEL_PARTNO(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtINELPartNo;
        }
        public DataTable BindReelBarcode(string sItemCode, string sSiteCode)
        {
            DataTable dtReelBarcode = new DataTable();
            dlobj = new DL_ReelSplitPrinting();
            try
            {
                dtReelBarcode = dlobj.BindReelBarcode(sItemCode, sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReelBarcode;
        }
        public DataTable SCANBARCODE(string sINELPartNo,  string sReelID, string sSiteCode)
        {
            DataTable dtReelBarcode = new DataTable();
            dlobj = new DL_ReelSplitPrinting();
            try
            {
                dtReelBarcode = dlobj.ValidateReelBarcode(sINELPartNo, sReelID, sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReelBarcode;
        }
        public string ChildLabelPrint(string _PartCode, string _ReelID, string sPrintedBy, decimal dUpdatedQty, string sPrinterIP,
            string sPrinterPort, string sUserID, string sLineCode, string sSiteCode)
        {
            dlobj = new DL_ReelSplitPrinting();
            string sResult = string.Empty;

            BL_Common objBL_Common = new BL_Common();
            try
            {
                DataTable dtGetdetails = dlobj.ChildLabelPrinting(_PartCode, _ReelID, dUpdatedQty, sPrintedBy, sSiteCode, sLineCode);

                if (dtGetdetails.Rows.Count > 0)
                {
                    sResult = dtGetdetails.Rows[0][0].ToString();
                    string sPartBarcode = sResult.Split('~')[2];

                    if (sResult.StartsWith("SUCCESS~"))
                    {
                        try
                        {
                            PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, MethodBase.GetCurrentMethod().Name, "Kitting Label Printing Prn Page Called : Barcode  " + sResult.Split('~')[2]);

                            string sPRNPrintingResult = string.Empty;

                            DataTable dtPRNDetail = dlobj.GetWIPSplitLabelDetail(sPartBarcode, sPrintedBy, sSiteCode, sLineCode);

                            if (dtPRNDetail.Rows.Count == 0)
                            {
                                sResult = "N~Qty updated but printing detail not found";
                                return sResult;
                            }
                            else
                            {
                                sPRNPrintingResult = dtPRNDetail.Rows[0][0].ToString();
                            }

                            if (sPRNPrintingResult.Length == 0)
                            {
                                sResult = "PRINTERPRNNOTPRINT~Qty updated but printing failed";
                            }
                            else if (sPRNPrintingResult.StartsWith("ERROR~"))
                            {
                                sResult = sPRNPrintingResult;
                            }
                            else
                            {
                                sResult = objBL_Common.sPrintDataLabel(sPrinterIP, sPRNPrintingResult, _ReelID, "Split", sUserID, sLineCode);
                            }
                        }
                        catch (Exception ex)
                        {
                            PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                            sResult = "N~Qty updated but printing data not found";
                        }
                    }
                }
                else
                {
                    sResult = "N~No result found, Please try again";
                }
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name, ex.Message);
                sResult = "ERROR~" + ex.Message;
            }
            return sResult;
        }
    }
}