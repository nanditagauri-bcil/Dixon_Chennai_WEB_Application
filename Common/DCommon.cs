using System;

namespace Common
{
    public class DCommon
    {
        PCommon oPCommon = new PCommon();
        public DBManager SqlDBProvider()
        {
            try
            {
                DBManager oManager = new DBManager(DataProvider.SqlServer, PCommon.StrSqlCon);
                return oManager;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
