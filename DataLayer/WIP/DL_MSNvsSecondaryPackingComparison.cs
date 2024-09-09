using BcilLib;
using Common;
using System;
using System.Data;
using System.Reflection;

namespace DataLayer.WIP
{
    public class DL_MSNvsSecondaryPackingComparison : DCommon
    {
        DBManager odb;
        DataTable dtobj;
        public DL_MSNvsSecondaryPackingComparison()
        {
            odb = SqlDBProvider();
        }

        public DataTable BindFGItemCode(string sSiteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                odb.CreateParameters(2);
                odb.AddParameters(0, "@TYPE", "BINDFGITEMCODE");
                odb.AddParameters(1, "@SITECODE", sSiteCode);
                odb.Open();
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dt;
        }
        public DataTable BindPO(string sFGITEMCODE, string sSiteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                odb.CreateParameters(3);
                odb.AddParameters(0, "@TYPE", "BINDPO");
                odb.AddParameters(1, "@SITECODE", sSiteCode);
                odb.AddParameters(2, "@FGITEMCODE", sFGITEMCODE);
                odb.Open();
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dt;
        }

        public DataTable BindInvoiceNo(string sPONumber, string sFGITEMCODE, string sSiteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                odb.CreateParameters(4);
                odb.AddParameters(0, "@TYPE", "BINDINVOICENO");
                odb.AddParameters(1, "@SITECODE", sSiteCode);
                odb.AddParameters(2, "@FGITEMCODE", sFGITEMCODE);
                odb.AddParameters(3, "@PONUMBER", sPONumber);
                odb.Open();
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dt;
        }
        public DataTable BindSecBOXID(string sInvoiceNo, string sPONumber, string sFGITEMCODE, string sSiteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                odb.CreateParameters(5);
                odb.AddParameters(0, "@TYPE", "BINDSECBOXID");
                odb.AddParameters(1, "@SITECODE", sSiteCode);
                odb.AddParameters(2, "@FGITEMCODE", sFGITEMCODE);
                odb.AddParameters(3, "@PONUMBER", sPONumber);
                odb.AddParameters(4, "@INVOICENO", sInvoiceNo);
                odb.Open();
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dt;
        }
        public DataSet BindMsnBOXID(string sSecBoxID, string sInvoiceNo, string sPONumber, string sFGITEMCODE, string sSiteCode
            ,string sLineCode, string sUserID)
        {
            DataSet ds = new DataSet();
            try
            {
                odb.CreateParameters(8);
                odb.AddParameters(0, "@TYPE", "BINDMSNBOXID");
                odb.AddParameters(1, "@SITECODE", sSiteCode);
                odb.AddParameters(2, "@FGITEMCODE", sFGITEMCODE);
                odb.AddParameters(3, "@PONUMBER", sPONumber);
                odb.AddParameters(4, "@INVOICENO", sInvoiceNo);
                odb.AddParameters(5, "@SECBOXID", sSecBoxID);
                odb.AddParameters(6, "@LINECODE", sLineCode);
                odb.AddParameters(7, "@VERFIEDBY", sUserID);
                odb.Open();
                ds = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON");
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return ds;
        }
         
        public DataTable ValidateScanMSNBarcode(string sModelCode, string sSecBoxID, string sInvoiceNo, string sPONumber,
            string sFGItemCode, string sSiteCode, string sMSNBarcode, string sLineCode, string sUserID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(10);
                odb.AddParameters(0, "@TYPE", "VALIDATEMSNBARCODE");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SECBOXID", sSecBoxID);
                odb.AddParameters(3, "@INVOICENO", sInvoiceNo);
                odb.AddParameters(4, "@PONUMBER", sPONumber);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@SITECODE", sSiteCode);
                odb.AddParameters(7, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(8, "@LINECODE", sLineCode);
                odb.AddParameters(9, "@VERFIEDBY", sUserID);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtResult;
        }

        public DataTable ValidateScanRSN(string sModelCode, string sSecBoxID, string sInvoiceNo, string sPONumber,
            string sFGItemCode, string sSiteCode, string sMSNBarcode, string sLineCode, string sUserID, string sRSN)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(11);
                odb.AddParameters(0, "@TYPE", "VALIDATERSN");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SECBOXID", sSecBoxID);
                odb.AddParameters(3, "@INVOICENO", sInvoiceNo);
                odb.AddParameters(4, "@PONUMBER", sPONumber);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@SITECODE", sSiteCode);
                odb.AddParameters(7, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(8, "@LINECODE", sLineCode);
                odb.AddParameters(9, "@VERFIEDBY", sUserID);
                odb.AddParameters(10, "@RSN", sRSN);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtResult;
        }

        public DataTable VERIFIEDSAVED(string sModelCode, string sSecBoxID, string sInvoiceNo, string sPONumber,
             string sFGItemCode, string sSiteCode, string sMSNBarcode, string sLineCode, string sUserID, DataTable _dt, string sRemarks)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(12);
                odb.AddParameters(0, "@TYPE", "SAVEVERIFIED");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SECBOXID", sSecBoxID);
                odb.AddParameters(3, "@INVOICENO", sInvoiceNo);
                odb.AddParameters(4, "@PONUMBER", sPONumber);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@SITECODE", sSiteCode);
                odb.AddParameters(7, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(8, "@LINECODE", sLineCode);
                odb.AddParameters(9, "@VERFIEDBY", sUserID);
                odb.AddParameters(10, "@MSNBARCODES", _dt);
                odb.AddParameters(11, "@REMARKS", sRemarks);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtResult;
        }

        public DataTable REJECTSAVED(string sModelCode, string sSecBoxID, string sInvoiceNo, string sPONumber,
            string sFGItemCode, string sSiteCode, string sMSNBarcode, string sLineCode, string sUserID, DataTable _dt, string sRemarks)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(12);
                odb.AddParameters(0, "@TYPE", "REJECTVERIFIED");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SECBOXID", sSecBoxID);
                odb.AddParameters(3, "@INVOICENO", sInvoiceNo);
                odb.AddParameters(4, "@PONUMBER", sPONumber);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@SITECODE", sSiteCode);
                odb.AddParameters(7, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(8, "@LINECODE", sLineCode);
                odb.AddParameters(9, "@VERFIEDBY", sUserID);
                odb.AddParameters(10, "@MSNBARCODES", _dt);
                odb.AddParameters(11, "@REMARKS", sRemarks);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtResult;
        }

        public DataTable GetData(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sSecBoxID)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(7);
                odb.AddParameters(0, "@TYPE", "GETMSNBARCODE");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@SECBOXID", sSecBoxID);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSN_VS_SECONDARY_PACKING_COMPARISON").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(EventNotice.EventTypes.evtError, MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtobj;
        }
    }
}
