using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using Oracle.DataAccess.Client;
using WorkQualityReport.Config;
using Wpms.App.Core.Util.Common.Helper;

namespace WorkQualityReport.Dal
{
    public class HelperDal
    {
        private static string ConnectionString = ConfigureManager.Logsever.DbConnection;

        public string GetDbUser(int provinceId)
        {
            return
                ExcecuteSalar(string.Format("select t.name from cfg_user_info t where t.user_area={0} and rownum=1",
                    provinceId)).ToString();
            
        }

        public static object ExcecuteNoQurey(string sql,OracleConnection db=null)
        {
            if (db==null)
            {
                return null;
            }
                    if (db.State != ConnectionState.Open)
                    {
                        db.Open();
                    }
                    var cmd = db.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
            
                    int result= cmd.ExecuteNonQuery();
            if (result<0)
            {
                throw new Exception(string.Format("数据库操作失败！sql为：{0}",sql));
            }
            return result;
        }
        public static object ExcecuteNoQureyWithoutTrans(string sql)
        {
            using (OracleConnection db = new OracleConnection(ConnectionString))
            {
                try
                {
                    if (db.State != ConnectionState.Open)
                    {
                        db.Open();
                    }
                    var cmd = db.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    int result = cmd.ExecuteNonQuery();
                    if (result < 0)
                    {
                        throw new Exception(string.Format("数据库操作失败！sql为：{0}", sql));
                    }
                    return result;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    db.Close();
                }
            }
            
         
        }
        private static Dictionary<string, OracleConnection> _connectCache=new Dictionary<string, OracleConnection>();
        private static Dictionary<string, OracleTransaction> _transCache = new Dictionary<string, OracleTransaction>();
       

        public static OracleConnection GetConnection(string clientId)
        {
            OracleConnection connection;
            if (!_connectCache.ContainsKey(clientId))
            {
                connection = new OracleConnection(ConnectionString);
                connection.Open();
                _connectCache.Add(clientId,connection);
            }
            else
            {
                connection = _connectCache[clientId];
            }
            return connection;
        }

       

        private static OracleTransaction _transaction;
        public static void BeginTrans(string clientId)
        {
            if (_transCache.ContainsKey(clientId))
            {
                return;
            }
            if (!_connectCache.ContainsKey(clientId))
            {
                throw  new Exception("please firstly create the connection!");
            }
            if (_connectCache[clientId].State != ConnectionState.Open)
            {
                _connectCache[clientId].Open();
            }

            var transaction = _connectCache[clientId].BeginTransaction(IsolationLevel.ReadCommitted);
           
            _transCache.Add(clientId,transaction);
        }

        public static void RollBackTrans(string clientId)
        {
            if (!_transCache.ContainsKey(clientId))
            {
                 throw  new Exception();
            }
            var trans = _transCache[clientId];
            if (trans != null)
            {
                _transCache.Remove(clientId);
                trans.Rollback();
                trans.Dispose();
                _connectCache.Remove(clientId);
            }
        }

        public static void CommitTrans(string clientId)
        {
            if (!_transCache.ContainsKey(clientId))
            {
                throw new Exception();
            }
            var trans = _transCache[clientId];
            if (trans != null)
            {
                _transCache.Remove(clientId);
                trans.Commit();
                trans.Dispose();
                _connectCache.Remove(clientId);
            }
            
        }
        public static int ExecuteNonQuery(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;
                cmd.BindByName = true;
             
               
                int val = cmd.ExecuteNonQuery();
 
                return val;
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().Error(DateTime.Now.ToString() + "     Sql:" + cmdText, ex);
                throw ex;
            }
        }


        public static object ExcecuteSalar(string sql)
        {
            using (OracleConnection db = new OracleConnection(ConnectionString))
            {
                try
                {
                    db.Open();
                    var cmd = db.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteScalar();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    db.Close();
                }
            }
        }

        public static List<T> ExecuteAndGetInstanceList<T>(string commandText, Dictionary<string, Object> inputParameters, Func<IDataRecord,T> dataRecord,string connection=null)
        {
            using (OracleConnection db = new OracleConnection(string.IsNullOrWhiteSpace(connection)?ConnectionString:connection))
            {
                try
                {
                    db.Open();
                    var cmd = db.CreateCommand();
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.Text;
                    List<T> list = new List<T>();

                    using (OracleDataReader data = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (data.Read())
                        {
                            list.Add(dataRecord(data));
                        }
                        return list;
                    }
 
                }
                catch
                {
                    throw;
                }
                finally
                {
                    db.Close();
                }
            }

        }
    }
}
