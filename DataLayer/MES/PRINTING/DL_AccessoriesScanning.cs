using BcilLib;
using Common;
using System;
using System.Data;
using System.Reflection;

namespace DataLayer.MES.PRINTING
{
    public class DL_AccessoriesScanning : DCommon
    {
        DBManager odb;
        DataTable dtobj;
        public DL_AccessoriesScanning()
        {
            odb = SqlDBProvider();
        }

        public DataTable Bind_Model_Mapping_Accessories(string sFGitemCode, string sSiteCode)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(3);
                odb.AddParameters(0, "@TYPE", "GETACCESSORIESDETAILS");
                odb.AddParameters(1, "@FG_ITEM_CODE", sFGitemCode);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_ACCESSORIES_SCANNING").Tables[0];
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

        public DataTable dlScanIMEIBarcode(string sFGitemCode, string sSiteCode, string sBarcode)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(4);
                odb.AddParameters(0, "@TYPE", "IMEISCAN");
                odb.AddParameters(1, "@FG_ITEM_CODE", sFGitemCode);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@PART_BARCODE", sBarcode);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_ACCESSORIES_SCANNING").Tables[0];
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
        public DataTable dlScanAccessoriesBarcode(string sFGitemCode, string sSiteCode, string sPcbBarcode
            , string sLineCode, string sModelName, string sAccBarcode, string sAccName,
            string sScannedBy, string sScanAccBarcode, string sAdaptorSN, string sMacID
            )
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(12);
                odb.AddParameters(0, "@TYPE", "ACCESSORIESSCAN");
                odb.AddParameters(1, "@FG_ITEM_CODE", sFGitemCode);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.AddParameters(3, "@PART_BARCODE", sPcbBarcode);
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@MODELNAME", sModelName);
                odb.AddParameters(6, "@ACCESSORIESBARCODE", sAccBarcode);
                odb.AddParameters(7, "@ACCESSORIESNAME", sAccName);
                odb.AddParameters(8, "@SCANBY", sScannedBy);
                odb.AddParameters(9, "@SCANNEDACCESSARYBARCODE", sScanAccBarcode);
                odb.AddParameters(10, "@ADAPTOR_SN", sAdaptorSN);
                odb.AddParameters(11, "@MACID", sMacID);

                PCommon.mBcilLogger.LogMessage(EventNotice.EventTypes.evtData, MethodBase.GetCurrentMethod().Name,
                    "Accessories Barcode Log : Scanned Barcode : " + sPcbBarcode
                    + ", Accessories Barcode : " + sAccBarcode
                    + ", Accessories Name : " + sAccName
                    + ", Stand Label : " + sScanAccBarcode
                    + ", Adapter SN : " + sAdaptorSN
                    + ", MacID : " + sMacID
                    );

                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_ACCESSORIES_SCANNING").Tables[0];
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
