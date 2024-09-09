using BcilLib;
using Common;
using System;
using System.Data;
using System.Reflection;

namespace DataLayer.WIP
{
    public class DL_WIPCastingLabelMapping : DCommon
    {
        DBManager odb;
        DataTable dtobj;
        public DL_WIPCastingLabelMapping()
        {
            odb = SqlDBProvider();
        }

        public DataTable ValidateMachine(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dt = new DataTable();
            try
            {
                odb.CreateParameters(4);
                odb.AddParameters(0, "@TYPE", "VALIDATEMACHINE");
                odb.AddParameters(1, "@MACHINEID", sMachineID);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@LINECODE", sLineCode);
                odb.Open();
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_CASTING_LABEL_MAPPING").Tables[0];
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
        public DataTable BindFGItemCode(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dt = new DataTable();
            try
            {
                odb.CreateParameters(4);
                odb.AddParameters(0, "@TYPE", "BINDFGITEMCODE");
                odb.AddParameters(1, "@MACHINEID", sMachineID);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@LINECODE", sLineCode);
                odb.Open();
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_CASTING_LABEL_MAPPING").Tables[0];
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
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_CASTING_LABEL_MAPPING").Tables[0];
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

        public DataSet VaildateBarcode(string sPartBarcode, string sMachineID, string FGItemCode
            , string sSiteCode, string sLineCode)
        {
            DataSet dtResult = new DataSet();
            try
            {
                odb.Open();
                odb.CreateParameters(6);
                odb.AddParameters(0, "@PART_BARCODE", sPartBarcode);
                odb.AddParameters(1, "@FGITEMCODE", FGItemCode);
                odb.AddParameters(2, "@MACHINEID", sMachineID);
                odb.AddParameters(3, "@SITECODE", sSiteCode);
                odb.AddParameters(4, "@TYPE", "VALIDATEPCB");
                odb.AddParameters(5, "@LINECODE", sLineCode);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_CASTING_LABEL_MAPPING");
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, "Wave Pallet scanning:Barcode:" + sPartBarcode + ", Machine ID :" + sMachineID
                    + ", FG Item Code :" + FGItemCode
                    );
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

        public DataTable ValidateScanSubPCBID(string sMachineID, string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sSubPCBID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(8);
                odb.AddParameters(0, "@TYPE", "VALIDATESUBPCBID");
                odb.AddParameters(1, "@MACHINEID", sMachineID);
                odb.AddParameters(2, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(3, "@SITECODE", sSiteCode);
                odb.AddParameters(4, "@PART_BARCODE", sPartBarcode);
                odb.AddParameters(5, "@LINECODE", sLineCode);
                odb.AddParameters(6, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(7, "@SUB_PCBID", sSubPCBID);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_CASTING_LABEL_MAPPING").Tables[0];

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

        public DataTable SaveScanSubPCBID(string sMachineID, string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sUserID, DataTable _dtSubPCBIDs)
        {
            DataTable dtResult = new DataTable();
            try
            {
                odb.Open();
                odb.CreateParameters(9);
                odb.AddParameters(0, "@TYPE", "SAVESUBPCB");
                odb.AddParameters(1, "@MACHINEID", sMachineID);
                odb.AddParameters(2, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(3, "@SITECODE", sSiteCode);
                odb.AddParameters(4, "@PART_BARCODE", sPartBarcode);
                odb.AddParameters(5, "@LINECODE", sLineCode);
                odb.AddParameters(6, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(7, "@MAPPEDBY", sUserID);
                odb.AddParameters(8, "@WAVEPALLET_SUBPCBID", _dtSubPCBIDs);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_CASTING_LABEL_MAPPING").Tables[0];

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

        public DataTable GetData(string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sSubPCBID)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(7);
                odb.AddParameters(0, "@TYPE", "BINDSUBPCBIDS");
                odb.AddParameters(1, "@Model_Code", sModelCode.Trim());
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@PART_BARCODE", sPartBarcode);
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(6, "@SUB_PCBID", sSubPCBID);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_CASTING_LABEL_MAPPING").Tables[0];
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
