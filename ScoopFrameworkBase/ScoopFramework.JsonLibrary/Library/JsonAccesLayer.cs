using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ScoopFramework.JsonSerializeLibrary.Library
{
    /// <summary>
    /// The json acces.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1005:SingleLineCommentsMustBeginWithSingleSpace", Justification = "Reviewed. Suppression is OK here."), Serializable]
    public static class JsonAccesLayer
    {
        /// <summary>
        /// The serialize json.
        /// </summary>
        /// <param name="dataTable">
        /// The data table.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string SerializeJson(DataTable dataTable)
        {
            var serializer = new JavaScriptSerializer();
            var tableRows = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dataTable.Rows)
            {
                var row = dataTable.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                tableRows.Add(row);
            }

            return serializer.Serialize(tableRows);
        }

        /// <summary>
        /// The derialize lis.
        /// </summary>
        /// <param name="json">
        /// The json.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<T> DerializeLis<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        /// <summary>
        /// The derialize data table.
        /// </summary>
        /// <param name="json">
        /// The json.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        public static DataTable DerializeDataTable(string json)
        {
            return JsonConvert.DeserializeObject<DataTable>(json);
        }
    }
}
