using BcilLib;
using Common;
using System;
using System.Data;
using System.Reflection;

namespace DataLayer.WIP
{
    public class DL_MSNvsGBComparison : DCommon
    {
        DBManager odb;
        DataTable dtobj;
        public DL_MSNvsGBComparison()
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
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSNVSGB_COMPARISON").Tables[0];
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
        public DataTable BindSubPCBID(string sFGitemCode, string sSiteCode)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(3);
                odb.AddParameters(0, "@TYPE", "GETSUBPCBDETAILS");
                odb.AddParameters(1, "@FGITEMCODE", sFGitemCode);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSNVSGB_COMPARISON").Tables[0];
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

        public DataSet VaildateBarcode(string sMSNBarcode, string FGItemCode, string sSiteCode, string sLineCode, string sUserID)
        {
            DataSet dsResult = new DataSet();
            try
            {
                odb.Open();
                odb.CreateParameters(6);
                odb.AddParameters(0, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(1, "@FGITEMCODE", FGItemCode);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@TYPE", "VALIDATEMSN");
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@VERFIEDBY", sUserID);
                dsResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_MSNVSGB_COMPARISON");
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
            return dsResult;
        }
         
        public DataTable ValidateScanGBBarcode(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sGBBarcode, string sUserID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(8);
                odb.AddParameters(0, "@TYPE", "VALIDATEGBBARCODE");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@GB_BARCODE", sGBBarcode);
                odb.AddParameters(7, "@VERFIEDBY", sUserID);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_MSNVSGB_COMPARISON").Tables[0];
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

        public DataTable VERIFIEDSAVED(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sUserID, DataTable _dt)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(8);
                odb.AddParameters(0, "@TYPE", "SAVEVERIFIED");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@VERFIEDBY", sUserID);
                odb.AddParameters(7, "@GBBARCODES", _dt);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_MSNVSGB_COMPARISON").Tables[0];
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

        public DataTable REJECTSAVED(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sUserID, DataTable _dt,string sRemarks)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(9);
                odb.AddParameters(0, "@TYPE", "REJECTVERIFIED");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@VERFIEDBY", sUserID);
                odb.AddParameters(7, "@GBBARCODES", _dt);
                odb.AddParameters(8, "@REMARKS", sRemarks);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_MSNVSGB_COMPARISON").Tables[0];
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
             , string sLineCode, string sFGItemCode, string sGBBarcode)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(7);
                odb.AddParameters(0, "@TYPE", "GETGBBARCODE");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@MSN_BARCODE", sMSNBarcode);
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@GB_BARCODE", sGBBarcode);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MSNVSGB_COMPARISON").Tables[0];
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
