using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace DataAccess
{
    public partial class FireBirdSQL
    {
        private static string sCnn = "";
        private static FbConnection cnn = new FbConnection();
        private static bool blCheckString = true;

        /// <summary>
        /// 设置一个逻辑值，表示在设置连接字符串的时候，是否需要测试 数据库连接是否成功。一般在第一次设置一个字符串的时候需要设置为true。
        /// </summary>
        public static bool CheckString
        {
            set { blCheckString = value; }
        }

        /// <summary>
        /// 设置连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get { return sCnn; }
            set
            {
                string sTemp = cnn.ConnectionString;
                cnn.ConnectionString = value;
                if (blCheckString)
                {
                    try
                    {
                        cnn.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("打开数据库连接错误，请检查！\n" + ex.Message);
                        //cnn.ConnectionString = sTemp;
                    }

                    if (cnn.State == ConnectionState.Open)
                    {
                        cnn.Close();
                        sCnn = value;
                    }
                }
                else
                {
                    sCnn = value;
                }
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns></returns>
        private static bool ConnectDB()
        {
            if (cnn.State == ConnectionState.Closed)
            {
                try
                {
                    cnn.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("打开数据库出错！\n" + ex.Message);
                    //MyMessageBox.ErrorMessage("打开数据库出错！\n" + ex.Message);
                    return false;
                }

                return true;
            }

            return true;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <returns></returns>
        private static bool CloseDB()
        {
            if (cnn.State != ConnectionState.Closed)
            {
                try
                {
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("关闭数据库出错！\n" + ex.Message);
                    //MyMessageBox.ErrorMessage("关闭数据库出错！\n" + ex.Message);
                    return false;
                }

                return true;
            }

            return true;
        }

        /// <summary>
        /// 执行不是查询的SQL语句，如INSERT、DELETE、UPDATE 和 SET 语句等命令。查询可以存储过程，或者是一般的SQL语句。查询可以带参数，在oaPrarm中按顺序给出所需参数。
        /// </summary>
        /// <param name="sSQL">SQL语句，或者存储过程的名称</param>
        /// <param name="SQLType">SQL的类型，为CommandType枚举类型</param>
        /// <param name="oaPrarm">参数列表，如果没有，则为null</param>
        /// <returns>返回一个逻辑值，true代表操作成功，false代表操作失败。</returns>
        public static bool ExeNonQuery(string sSQL,CommandType SQLType,params object[] oaPrarm)
        {
            using (FbConnection cnnCur = new FbConnection(sCnn)) 
            {
                try
                {
                    if (cnnCur.State != ConnectionState.Open)
                    {
                        cnnCur.Open();
                    }
                }
                catch (FbException ex)
                {
                    throw new Exception("打开数据库连接出错，请检查数据库文件是否正确。" + ex.Message);
                }

                FbTransaction tran = cnnCur.BeginTransaction();
                FbCommand cmd = new FbCommand(sSQL, cnnCur);
                cmd.CommandType = SQLType;
                cmd.Transaction = tran;

                if (oaPrarm != null)
                {
                    List<FbParameter> lstPs = new List<FbParameter>();
                    FbParameter odpCur =null;
                    foreach (object o in oaPrarm)
                    {
                        odpCur = new FbParameter();
                        if (o is DateTime)
                        {
                            odpCur.FbDbType = FbDbType.TimeStamp;
                            odpCur.Value = string.Format("#{0}#", o);
                        }
                        else
                            odpCur.Value = o;
                        lstPs.Add(odpCur);
                    }
                    lstPs.TrimExcess();
                    cmd.Parameters.AddRange(lstPs.ToArray());
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    cnnCur.Close();
                    return true;
                }
                catch (FbException ex)
                {
                    cnnCur.Close();
                    //throw new Exception("数据保存出错：" + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行查询的SQL语句。查询可以带参数，在oaPrarm中按顺序给出所需参数。
        /// </summary>
        /// <param name="sSQL">SQL语句，或者存储过程的名称</param>
        /// <param name="SQLType">SQL的类型，为CommandType枚举类型</param>
        /// <param name="oaPrarm">参数列表，如果没有，则为null</param>
        /// <returns>返回一个DataTable表，该表为查询结果。如果查询语句中出现多张表作为查询结果，只返回第一个张表。</returns>
        public static DataTable ExeQuery(string sSQL, CommandType SQLType, FbConnection cnnCur, params object[] oaPrarm)
        {
            //using (FbConnection cnnCur = new FbConnection(sCnn))
            //{
                try
                {
                    try
                    {
                        if (cnnCur.State != ConnectionState.Open)
                        {
                            cnnCur.Open();
                        }
                    }
                    catch (FbException ex)
                    {
                        throw new Exception("打开数据库连接出错，请检查数据库文件是否正确。" + ex.Message);
                    }


                    FbCommand cmd = new FbCommand(sSQL, cnnCur);
                    cmd.CommandType = SQLType;

                    if (oaPrarm != null)
                    {
                        List<FbParameter> lstPs = new List<FbParameter>();
                        FbParameter odpCur = null;
                        foreach (object o in oaPrarm)
                        {
                            odpCur = new FbParameter();
                            odpCur.Value = o;
                            lstPs.Add(odpCur);
                        }
                        lstPs.TrimExcess();
                        cmd.Parameters.AddRange(lstPs.ToArray());
                    }
                    DataSet dsData = new DataSet();
                    FbDataAdapter daData = new FbDataAdapter(cmd);
                    try
                    {
                        daData.Fill(dsData);
                    }
                    catch(FbException ex)
                    {
                        cnnCur.Close();
                        throw (new Exception("读取数据出错：" + ex.Message));
                    }
                    if (dsData.Tables.Count > 0)
                        return dsData.Tables[0];
                    else
                        return null;
                }
                finally
                {
                    cnnCur.Close();
                    
                }
           // }
        }
    }
}
