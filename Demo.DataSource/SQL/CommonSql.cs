namespace Demo.Data.SQL
{
    /// <summary>
    /// 常用Sql语句库
    /// </summary>
    public static class CommonUseSql
    {
        /// <summary>
        /// 获取数据库所有表名和表注释
        /// </summary>
        public const string GetTableAndDescription = @"SELECT Name, 'Description' = (SELECT TOP 1 value FROM sys.extended_properties p 
					                                        WHERE p.major_id = t.OBJECT_ID 
					                                        AND p.name = 'MS_Description' 
					                                        AND p.minor_id = 0)
				                                        FROM sys.tables t";

        /// <summary>
        /// 获取指定存储过程的参数列表
        /// </summary>
        public const string GetProcedureParams = @"select Name,[Length], IsOutParam,
                                                        'DbType' = dbo.f_QueryDbTypeValue((SELECT TOP 1 name FROM sys.types t WHERE t.system_type_id = xtype))
                                                        from syscolumns where ID in
                                                            (SELECT id FROM sysobjects as a  
                                                                WHERE OBJECTPROPERTY(id, N'IsProcedure') = 1
                                                                and id = object_id(N'{0}'))";
    }
}
