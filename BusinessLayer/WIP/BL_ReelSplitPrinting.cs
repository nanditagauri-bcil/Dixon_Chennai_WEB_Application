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

        public DataTable SCANBARCODE(string sINELPartNo, string sReelID, string sSiteCode)
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

        //public string ChildLabelPrint(string _PartCode, string _ReelID, string sPrintedBy, decimal dUpdatedQty, string sPrinterIP,
        //    string sPrinterPort, string sUserID, string sLineCode, string sSiteCode)
        //{
        //    dlobj = new DL_ReelSplitPrinting();
        //    string sResult = string.Empty;

        //    BL_Common objBL_Common = new BL_Common();
        //    try
        //    {
        //        DataTable dtSplitResult = dlobj.ChildLabelPrinting(_PartCode, _ReelID, dUpdatedQty, sPrintedBy, sSiteCode, sLineCode);

        //        if (dtSplitResult != null && dtSplitResult.Rows.Count > 0)
        //        {
        //            var dbMessage = dtSplitResult.Rows[0][0].ToString();

        //            if (string.IsNullOrWhiteSpace(dbMessage))
        //            {
        //                return "N~Failed to generated split label";
        //            }

        //            if (dbMessage.ToUpper().StartsWith("N~") || dbMessage.ToUpper().StartsWith("ERROR~"))
        //            {
        //                return dbMessage;
        //            }

        //            var finalPRN = dtSplitResult.Rows[0]["FINAL_PRN"].ToString();
        //            var newPartbarcode = dtSplitResult.Rows[0]["PART_BARCODE"].ToString();
        //            sResult = dtSplitResult.Rows[0]["MESSAGE"].ToString();

        //            if (string.IsNullOrWhiteSpace(finalPRN))
        //            {
        //                return "N~Split label printing failed";
        //            }

        //            if (sResult.StartsWith("SUCCESS~"))
        //            {
        //                try
        //                {
        //                    PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, MethodBase.GetCurrentMethod().Name, "Kitting Label Printing Prn Page Called : Barcode  " + sResult.Split('~')[2]);

        //                    sResult = objBL_Common.sPrintDataLabel(sPrinterIP, finalPRN, _ReelID, "Split", sUserID, sLineCode);
        //                }
        //                catch (Exception ex)
        //                {
        //                    PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        //                    sResult = "N~Qty updated but printing data not found";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            sResult = "N~No result found, Please try again";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name, ex.Message);
        //        sResult = "ERROR~" + ex.Message;
        //    }
        //    return sResult;
        //}

        public string ChildLabelPrint(string _PartCode, string _ReelID, string sPrintedBy, decimal dUpdatedQty, string sPrinterIP, string sPrinterPort,
            string sUserID, string sLineCode, string sSiteCode)
        {
            dlobj = new DL_ReelSplitPrinting();
            string sResult = string.Empty;

            BL_Common objBL_Common = new BL_Common();
            try
            {
                DataTable dtSplitResult = dlobj.ChildLabelPrinting(_PartCode, _ReelID, dUpdatedQty, sPrintedBy, sSiteCode, sLineCode);

                if (dtSplitResult != null && dtSplitResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSplitResult.Rows)
                    {
                        var dbMessage = dr[0].ToString();

                        if (string.IsNullOrWhiteSpace(dbMessage))
                        {
                            return "N~Failed to generated split label";
                        }

                        if (dbMessage.ToUpper().StartsWith("N~") || dbMessage.ToUpper().StartsWith("ERROR~"))
                        {
                            return dbMessage;
                        }

                        var finalPRN = dr["FINAL_PRN"].ToString();
                        var newPartbarcode = dr["PART_BARCODE"].ToString(); // Note: variable declared but not used in original logic
                        sResult = dr["MESSAGE"].ToString();

                        if (string.IsNullOrWhiteSpace(finalPRN))
                        {
                            return "N~Split label printing failed";
                        }

                        if (sResult.StartsWith("SUCCESS~"))
                        {
                            try
                            {
                                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, MethodBase.GetCurrentMethod().Name,
                                    "Kitting Label Printing Prn Page Called : Barcode  " + sResult.Split('~')[2]);

                                sResult = objBL_Common.sPrintDataLabel(sPrinterIP, finalPRN, newPartbarcode, "Split", sUserID, sLineCode);

                                if (sResult.StartsWith("N~") || sResult.StartsWith("ERROR~"))
                                {
                                    return sResult;
                                }
                            }
                            catch (Exception ex)
                            {
                                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                                return "N~Qty updated but printing data not found";
                            }
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