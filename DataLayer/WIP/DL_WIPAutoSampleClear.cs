using Common;
using System;
using System.Data;

namespace DataLayer.WIP
{
    public class DL_WIPAutoSampleClear : DCommon
    {
        DBManager odb;
        DataTable dtobj;
        public DL_WIPAutoSampleClear()
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
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_WIP_AUTO_SAMPLE_CLEAR").Tables[0];
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
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_WIP_AUTO_SAMPLE_CLEAR").Tables[0];
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
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_WIP_AUTO_SAMPLE_CLEAR");
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

        public DataSet SaveResult(string sPartBarcode, string sMachineID, string sFGItemCode, string sSiteCode,
              string sLineCode, string sUserID, string sResultType)
        {
            DataSet dtResult = new DataSet();
            try
            {
                odb.Open();
                odb.CreateParameters(7);
                odb.AddParameters(0, "@MACHINEID", sMachineID);
                odb.AddParameters(1, "@PART_BARCODE", sPartBarcode);
                odb.AddParameters(2, "@LINECODE", sLineCode);
                odb.AddParameters(3, "@FGITEMCODE", sFGItemCode);
                odb.AddParameters(4, "@USERID", sUserID);
                odb.AddParameters(5, "@SITECODE", sSiteCode);
                odb.AddParameters(6, "@TYPE", sResultType);
                dtResult = odb.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_WIP_AUTO_SAMPLE_CLEAR");

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

    }
}
