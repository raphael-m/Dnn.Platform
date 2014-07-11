#region Copyright

// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

#region Usings

using System;
using System.Reflection;

using DotNetNuke.ComponentModel.DataAnnotations;

using NPoco;

#endregion

namespace DotNetNuke.Data.NPoco
{
    public class NPocoMapper : IMapper
    {
        private readonly string _tablePrefix;

        public NPocoMapper(string tablePrefix)
        {
            _tablePrefix = tablePrefix;
        }

        #region Implementation of IMapper

        public void GetTableInfo(Type pocoType, TableInfo ti)
        {
            //Table Name
            ti.TableName = DataUtil.GetTableName(pocoType, ti.TableName + "s");

            ti.TableName = _tablePrefix + ti.TableName;

            //Primary Key
            ti.PrimaryKey = DataUtil.GetPrimaryKeyColumn(pocoType, ti.PrimaryKey);

            ti.AutoIncrement = DataUtil.GetAutoIncrement(pocoType, true);
        }

        public bool MapMemberToColumn(MemberInfo pocoProperty, ref string columnName, ref bool resultColumn)
        {
            bool includeColumn = true;

            //Check if the class has the ExplictColumnsAttribute
            bool declareColumns = pocoProperty.DeclaringType != null
                            && pocoProperty.DeclaringType.GetCustomAttributes(typeof(DeclareColumnsAttribute), true).Length > 0;

            if (declareColumns)
            {
                if (pocoProperty.GetCustomAttributes(typeof(IncludeColumnAttribute), true).Length == 0)
                {
                    includeColumn = false;
                }
            }
            else
            {
                if (pocoProperty.GetCustomAttributes(typeof(IgnoreColumnAttribute), true).Length > 0)
                {
                    includeColumn = false;
                }
            }

            if (includeColumn)
            {
                columnName = DataUtil.GetColumnName(pocoProperty, columnName);

                resultColumn = (pocoProperty.GetCustomAttributes(typeof(ReadOnlyColumnAttribute), true).Length > 0);
            }

            return true;
        }

        public Func<object, object> GetFromDbConverter(MemberInfo mi, Type SourceType)
        {
            var t = mi.GetMemberInfoType();
            return mi != null ? GetFromDbConverter(t, SourceType) : null;
        }

        public Func<object, object> GetFromDbConverter(Type DestType, Type SourceType)
        {
            return null;
        }

        public Func<object, object> GetParameterConverter(Type SourceType)
        {
            return null;
        }

        public Func<object, object> GetToDbConverter(Type DestType, Type SourceType)
        {
            return null;
        }

        #endregion
    }
}