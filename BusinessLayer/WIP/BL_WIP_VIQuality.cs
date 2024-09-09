using Common;
using DataLayer.WIP;
using System;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_WIP_VIQuality
    {
        DL_WIP_VIQuality dlobj;

        #region VIQuality
        public DataTable ValidateMachine(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dtLocation = new DataTable();
            dlobj = new DL_WIP_VIQuality();
            try
            {
                dtLocation = dlobj.ValidateMachine(sMachineID, sSiteCode, sLineCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLocation;
        }
        public DataTable BindFGItemCode(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dtFGItemCode = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtFGItemCode = dlobj.BindFGItemCode(sMachineID, sSiteCode, sLineCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFGItemCode;
        }

        public DataTable BindSubMachineID(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dtSubMachineID = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtSubMachineID = dlobj.BindSubMachineID(sMachineID, sSiteCode, sLineCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtSubMachineID;
        }
        public DataTable GetSampleQtyData(string sFGItemCode, string sSiteCode, string sLineCode, string sUserID, string SUBMACHINEID)
        {
            DataTable dtFGItemCode = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtFGItemCode = dlobj.GetSamplingQty(sFGItemCode, sSiteCode, sLineCode, sUserID, SUBMACHINEID);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFGItemCode;
        }
        public DataTable GetPdiSamplingQty(string sFGItemCode, string sSiteCode, string MACHINEID, string sUserID)
        {
            DataTable dtFGItemCode = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtFGItemCode = dlobj.GetPdiSamplingQty(sFGItemCode, sSiteCode, MACHINEID, sUserID);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFGItemCode;
        }
        public DataTable GetPdiSampling(string sFGItemCode, string sSiteCode, string MACHINEID, string sUserID, string SUBMACHINEID)
        {
            DataTable dtFGItemCode = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtFGItemCode = dlobj.GetPdiSampling(sFGItemCode, sSiteCode, MACHINEID, sUserID, SUBMACHINEID);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFGItemCode;
        }
        public DataTable QualityBarcode(string sPartBarcode, string sMachineID, string FGItemCode
           , string sSiteCode, string sLineCode, string sSamplePCB, string sUseriD, bool PDIInspectionBox
           , string sSubmachineID) //Added by Shivam(22/05/2023)
        {
            DataTable dtLineData = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtLineData = dlobj.QualityBarcode(sPartBarcode, sMachineID, FGItemCode
                    , sSiteCode, sLineCode, sSamplePCB, sUseriD, PDIInspectionBox
                    , sSubmachineID);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLineData;
        }
        #endregion

        #region OQC
        public DataTable ValidateOQCMachine(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dtLocation = new DataTable();
            dlobj = new DL_WIP_VIQuality();
            try
            {
                dtLocation = dlobj.ValidateOQCMachine(sMachineID, sSiteCode, sLineCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLocation;
        }
        public DataTable BindOQCFGItemCode(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dtFGItemCode = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtFGItemCode = dlobj.BindOQCFGItemCode(sMachineID, sSiteCode, sLineCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFGItemCode;
        }
        public DataTable BindReWorkStationID(string sSiteCode)
        {
            DataTable dtLineData = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtLineData = dlobj.BindReworkstation(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLineData;
        }

        public DataTable BindDefect(string sSiteCode)
        {
            DataTable dtLineData = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtLineData = dlobj.BindDefect(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLineData;
        }
        public DataTable ValidatePCB(string sPartBarcode, string sMachineID, string FGItemCode
            , string sSiteCode, string sLineCode
            )
        {
            DataTable dtLineData = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtLineData = dlobj.VaildateBarcode(sPartBarcode, sMachineID, FGItemCode
                    , sSiteCode, sLineCode
                    );
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLineData;
        }
        public DataTable SaveQualtiyData(string sPartBarcode, string sMachineID, string FGItemCode
       , string sDefect, string sReworkStation, string sObservation, string sStatus
            , string sSiteCode, string sLineCode, string sUseriD
            )
        {
            DataTable dtLineData = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtLineData = dlobj.SaveData(sPartBarcode, sMachineID, FGItemCode
                   , sDefect, sReworkStation, sObservation, sStatus
                   , sSiteCode, sLineCode, sUseriD
                   );
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLineData;
        }

        #endregion

        #region XraySamplingQuality
        public DataTable ValidateXrayMachine(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dtLocation = new DataTable();
            dlobj = new DL_WIP_VIQuality();
            try
            {
                dtLocation = dlobj.ValidateXrayMachine(sMachineID, sSiteCode, sLineCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLocation;
        }
        public DataTable BindXrayFGItemCode(string sMachineID, string sSiteCode, string sLineCode)
        {
            DataTable dtFGItemCode = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtFGItemCode = dlobj.BindXrayFGItemCode(sMachineID, sSiteCode, sLineCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFGItemCode;
        }
        public DataTable ValidateXrayPCB(string sPartBarcode, string sMachineID, string FGItemCode
            , string sSiteCode, string sLineCode
            )
        {
            DataTable dtLineData = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtLineData = dlobj.VaildateXrayBarcode(sPartBarcode, sMachineID, FGItemCode
                    , sSiteCode, sLineCode
                    );
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLineData;
        }
        public DataTable SaveXrayQualtiyData(string sPartBarcode, string sMachineID, string FGItemCode
       , string sDefect, string sReworkStation, string sObservation, string sStatus
            , string sSiteCode, string sLineCode, string sUseriD
            )
        {
            DataTable dtLineData = new DataTable();
            try
            {
                dlobj = new DL_WIP_VIQuality();
                dtLineData = dlobj.SaveXrayData(sPartBarcode, sMachineID, FGItemCode
                   , sDefect, sReworkStation, sObservation, sStatus
                   , sSiteCode, sLineCode, sUseriD
                   );
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLineData;
        }

        #endregion
    }
}
