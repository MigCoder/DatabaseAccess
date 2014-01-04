using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Common.EntityTool;
using Common.EntityTool.DelegateReflection;
using DataSource.Config;
using DataSource.DataBaseMap;
using DataSource.SQL;

namespace DataSource.DataBase
{
    /// <summary>
    /// 提供数据库操作服务
    /// </summary>
    public class DbHelper
    {

        #region 标识属性

        /// <summary>
        /// 取消数据库操作对象
        /// </summary>
        public static CancellationTokenSource CancelSource = new CancellationTokenSource();

        /// <summary>
        /// 当前连接的数据库类型
        /// </summary>
        public static DatabaseType DataBaseType;

        #endregion

        #region 数据库操作对象

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public static DbConnection Connection;

        /// <summary>
        /// 数据库操作对象
        /// </summary>
        public static DbCommand Command;

        /// <summary>
        /// 数据库操作对象（专门执行存储过程）
        /// </summary>
        public static DbCommand ProcCommand;

        /// <summary>
        /// 数据库事务对象
        /// </summary>
        private static DbTransaction _trans;

        #endregion

        #region 事件

        /// <summary>
        /// 操作已经被取消事件
        /// </summary>
        public event Action CanceldOperate;
        /// <summary>
        /// 引发操作已经被取消事件
        /// </summary>
        protected virtual void OnCanceldOperate()
        {
            Action handler = CanceldOperate;
            if (handler != null) handler();
        }

        #endregion

        #region 构造与初始化

        static DbHelper()
        {

        }

        public static void Init(string connectStr, DatabaseType dbType)
        {
            switch (dbType)
            {
                case DatabaseType.MicrosoftSql:
                    {
                        Connection = new SqlConnection(connectStr);
                        ProcCommand = new SqlCommand();
                        Command = new SqlCommand();
                        break;
                    }
                default:
                    throw new ArgumentException("暂未支持该类型数据库！");
            }
            DataBaseType = dbType;
        }

        #endregion

        #region 打开关闭

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public static void OpenDataBase()
        {
            Connection.Open();
            Command.Connection = Connection;
            ProcCommand.Connection = Connection;
            ProcCommand.CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// 取消当前的数据库操作
        /// </summary>
        public void CancelOperate()
        {
            CancelSource.Cancel();
            this.OnCanceldOperate();
        }

        #endregion

        #region 执行操作

        /// <summary>
        /// 执行一句非查询Sql语句并返回受影响的行数
        /// </summary>
        public static int ExecuteNoQuery(string sql)
        {
            Command.CommandText = sql;
            return Command.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行一句查询语句并返回一个数据阅读器
        /// </summary>
        public static DbDataReader ExecuteReader(string query)
        {
            Command.CommandText = query;
            return Command.ExecuteReader(CommandBehavior.SingleResult);
        }

        /// <summary>
        /// 执行一句查询语句并返回结果的第一行第一列
        /// </summary>
        public static Object ExecuteScalar(string query)
        {
            Command.CommandText = query;
            return Command.ExecuteScalar();
        }

        #endregion

        #region 实体查询

        /// <summary>
        /// 执行一句查询语句，并将返回的结果填充为指定的实体
        /// </summary>
        public static T ReadEntity<T>(string sql) where T : class,new()
        {
            Command.CommandText = sql;
            using (var reader = Command.ExecuteReader())
            {
                return ReadData<T>(reader);
            }
        }

        /// <summary>
        /// 执行一句查询语句，并将返回的结果填充为指定的实体集合
        /// </summary>
        public static IEnumerable<T> ReadEntitys<T>(string sql) where T : class,new()
        {
            Command.CommandText = sql;
            using (var reader = Command.ExecuteReader())
            {
                var result = ReadDatas<T>(reader);

                return result;
            }
        }

        /// <summary>
        /// 执行一个查询存储过程，并将返回的结果填充为指定的实体
        /// </summary>
        public static T ReadEntity<T>(DbProc proc) where T : class, new()
        {
            ProcCommand.CommandText = proc.Name;
            ProcCommand.Parameters.Clear();
            ProcCommand.Parameters.AddRange(proc.ParamsToSql());

            using (var reader = ProcCommand.ExecuteReader())
            {
                var result = ReadData<T>(reader);

                return result;
            }
        }

        /// <summary>
        /// 执行一个查询存储过程，并将返回的结果填充为指定的实体集合
        /// </summary>
        public static IEnumerable<T> ReadEntitys<T>(DbProc proc) where T : class, new()
        {
            ProcCommand.CommandText = proc.Name;
            ProcCommand.Parameters.Clear();
            ProcCommand.Parameters.AddRange(proc.ParamsToSql());

            using (var reader = ProcCommand.ExecuteReader())
            {
                var result = ReadDatas<T>(reader);

                return result;
            }
        }

        #endregion

        #region 存储过程相关

        /// <summary>
        /// 获取指定存储过程的参数列表
        /// </summary>
        public static IEnumerable<DbParam> GetProcedureParams(string procName)
        {
            var sql = string.Format(CommonUseSql.GetProcedureParams, procName);

            return ReadEntitys<DbParam>(sql);
        }

        /// <summary>
        /// 将实体对象的属性与填充到存储过程对应的参数中（要求属性名与参数名一致）
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="entity">实体对象</param>
        public static DbProc FillParams<T>(string procName, T entity)
        {
            var procParams = GetProcedureParams(procName).ToList();

            var reflection = new DelegateReflection<T>();
            foreach (var param in procParams)
            {
                param.Value = reflection.GetValue(entity, param.Name.Remove(0, 1));
            }

            return new DbProc(procName) { Params = procParams };
        }

        /// <summary>
        /// 尝试执行指定名称的存储过程，如果成功则返回包装好的存储过程对象
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="objParams">参数列表(请按顺序给出)</param>
        public static DbProc ExecuteProcedure(string procName, params object[] objParams)
        {
            var procParams = GetProcedureParams(procName).ToList();
            if (objParams.Length != procParams.Count)
                throw new ArgumentException("所给参数与存储过程所需参数数量不一致!");

            for (int index = 0; index < procParams.Count; index++)
            {
                procParams[index].Value = objParams[index];
            }
            var proc = new DbProc(procName) { Params = procParams };
            ExecuteProcedure(proc);

            return proc;
        }

        /// <summary>
        /// 执行指定的存储过程,并为其输出参数以及返回值赋值
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        public static void ExecuteProcedure(DbProc proc)
        {
            if (proc == null) throw new ArgumentNullException();

            ProcCommand.CommandText = proc.Name;
            ProcCommand.Parameters.Clear();
            ProcCommand.Parameters.AddRange(proc.ParamsToSql());

            //执行并获取受影响的行数
            proc.AffectedRow = ProcCommand.ExecuteNonQuery();

            //为存储过程输出参数赋值
            foreach (DbParameter param in ProcCommand.Parameters)
            {
                if (param.Direction == ParameterDirection.Input) continue;

                var refParam = proc.Params.First(x => x.Name == param.ParameterName);
                refParam.Value = param.Value;
            }
        }

        #endregion

        #region 读取数据

        /// <summary>
        /// 从数据库阅读器中读取出一个指定实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="reader">阅读器</param>
        private static T ReadData<T>(DbDataReader reader) where T : class, new()
        {
            var result = new T();
            var dynamicSet = new DynamicReflection<T>();
            if (reader.Read() == false) return default(T);

            for (int index = 0; index < reader.FieldCount; index++)
            {
                var name = reader.GetName(index);
                var value = reader[index];
                dynamicSet.SetValue(result, name, value);
            }

            return result;
        }

        /// <summary>
        /// 从数据库阅读器中读取出指定实体的集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="reader">阅读器</param>
        private static IEnumerable<T> ReadDatas<T>(DbDataReader reader) where T : class, new()
        {
            var result = new List<T>();
            var dynamicGet = new DynamicReflection<T>();

            while (reader.Read())
            {
                var temp = new T();
                for (int index = 0; index < reader.FieldCount; index++)
                {
                    var name = reader.GetName(index);
                    var type = dynamicGet.Props[name].PropertyType;
                    var value = DbTypeConvert.ToCsharpValue(type, reader[index]);
                    dynamicGet.SetValue(temp, name, value);
                }
                result.Add(temp);
            }
            return result;
        }

        #endregion

        #region 事务

        /// <summary>
        /// 开启数据库事务
        /// </summary>
        public static void BeginTrans()
        {
            _trans = Connection.BeginTransaction();
            Command.Transaction = _trans;
            ProcCommand.Transaction = _trans;
        }

        /// <summary>
        /// 回滚数据库事务
        /// </summary>
        public static void RollBack()
        {
            if (_trans == null) return;

            _trans.Rollback();
        }

        /// <summary>
        /// 提交数据库事务
        /// </summary>
        public static void Commit()
        {
            if (_trans == null) return;

            _trans.Commit();
        }

        #endregion

        #region 资源释放

        public static void Dispose()
        {
            if (Command != null) Command.Dispose();
            if (Connection != null) Connection.Dispose();
        }

        #endregion
    }
}
