using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.Helper.Enums;
using Speurzoekers.Data.Entities.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public static class RepositoryHelper
    {
#warning Multiple "EF1000; Possible SQL injection vulnerability"
#pragma warning disable EF1000 // Possible SQL injection vulnerability.

        /// <summary>Get the value of the MaxLengthAttribute</summary>
        /// <param name="property">Lambda expression to property; x => x.[PROPERTY]</param>
        /// <returns>Maximum string length</returns>
        public static int GetColumnMaxStringLength<Entity>(Expression<Func<Entity, string>> property)
        {
            PropertyInfo prop = typeof(Entity).GetProperties()
                .FirstOrDefault(x => x.Name == ((MemberExpression)property.Body).Member.Name);

            if (prop == null)
            {
                throw new ArgumentException();
            }

            IEnumerable<Attribute> attrs = prop.GetCustomAttributes();

            foreach (Attribute attr in attrs)
            {
                if (attr is MaxLengthAttribute maxLength)
                {
                    return maxLength.Length;
                }
            }

            throw new ArgumentException();
        }

        /// <summary>Filter and sort records</summary>
        /// <param name="criterias">Filter criterias</param>
        /// <param name="orConcatenation">Join filter criterias with OR, otherwise AND</param>
        /// <param name="sorting">Sorting</param>
        /// <returns>Filtered and sorted records</returns>
        public static IQueryable<Sum> FilterSummary<Sum, Entity>(
            this DbQuery<Sum> set,
            bool orConcatenation,
            IEnumerable<FilterCriterion> criterias = null,
            Sorting sorting = null)
                where Sum : Summary
                where Entity : class
        {
            SpeurzoekersDbContext dbContext = set.GetDbContext();
            Dictionary<string, string> columns = GetColumns<Entity>(dbContext);
            StringBuilder sqlBuilder = new StringBuilder();
            object[] args = new object[0];
            string valueName = "[Name]",
                keyCol = columns[GetPrimaryKeyName<Entity>(dbContext)],
                sortCol = columns[sorting.PropertyName];

            sqlBuilder.Append($"SELECT {keyCol}, {valueName}");

            if (sorting != null)
            {
                if (sortCol != keyCol && sortCol != valueName)
                {
                    sqlBuilder.Append($", {sortCol}");
                }

                if (sorting.SortingMethod == SortingMethod.EnumLow)
                {
                    // X & -X = lowest set bit value
                    sqlBuilder.Append($", {sortCol} & -{sortCol} AS [SortCol]");
                }
            }

            sqlBuilder.Append($" FROM {GetTableName<Entity>(dbContext)}");

            if (criterias != null && criterias.Any())
            {
                args = SetSqlFilter(sqlBuilder, columns, criterias, orConcatenation);
            }

            if (sorting != null)
            {
                SetSqlSorting(sqlBuilder, sorting, sortCol);
            }

            string sql = sqlBuilder.ToString();
            return set.FromSql(sql, args);
        }

        private static object[] SetSqlFilter(
            StringBuilder sqlBuilder,
            Dictionary<string, string> columns,
            IEnumerable<FilterCriterion> criterias,
            bool orConcatenation)
        {
            string concat = orConcatenation ? " OR " : " AND ";

            sqlBuilder.Append(" WHERE ");

            object[] args = criterias.Select((c, i) =>
            {
                object arg = SetSqlFilterItem(sqlBuilder, columns, c, i);
                sqlBuilder.Append(concat);
                return arg;
            }).ToArray();

            sqlBuilder.Remove(sqlBuilder.Length - concat.Length, concat.Length);

            return args;
        }

        private static object SetSqlFilterItem(
            StringBuilder sqlBuilder, 
            Dictionary<string, string> columns, 
            FilterCriterion criterion, 
            int index)
        {                
            string column = columns[criterion.PropertyName];
            object value = criterion.Value;

            switch (criterion.Comparison)
            {
                case ComparisonTypes.EnumEqual:
                case ComparisonTypes.NumEqual:
                case ComparisonTypes.StringEqual:
                    sqlBuilder.Append($"{column} = {{{index}}}");
                    break;
                case ComparisonTypes.EnumNotEqual:
                case ComparisonTypes.NumNotEqual:
                case ComparisonTypes.StringNotEqual:
                    sqlBuilder.Append($"{column} <> {{{index}}}");
                    break;
                case ComparisonTypes.EnumHasFlag:
                    sqlBuilder.Append($"{column} & {{{index}}} > 0");
                    break;
                case ComparisonTypes.EnumNoFlag:
                    sqlBuilder.Append($"{column} & {{{index}}} = 0");
                    break;

                case ComparisonTypes.EnumCountEqual:
                case ComparisonTypes.EnumCountNotEqual:
                case ComparisonTypes.EnumCountLarger:
                case ComparisonTypes.EnumCountLargerOrEqual:
                case ComparisonTypes.EnumCountSmaller:
                case ComparisonTypes.EnumCountSmallerOrEqual:
                    GetEnumCount(sqlBuilder, column, criterion.Type.GetEnumUnderlyingType());

                    switch (criterion.Comparison)
                    {
                        case ComparisonTypes.EnumCountEqual:
                            sqlBuilder.Append($" = {{{index}}}");
                            break;
                        case ComparisonTypes.EnumCountNotEqual:
                            sqlBuilder.Append($" <> {{{index}}}");
                            break;
                        case ComparisonTypes.EnumCountLarger:
                            sqlBuilder.Append($" > {{{index}}}");
                            break;
                        case ComparisonTypes.EnumCountLargerOrEqual:
                            sqlBuilder.Append($" >= {{{index}}}");
                            break;
                        case ComparisonTypes.EnumCountSmaller:
                            sqlBuilder.Append($" < {{{index}}}");
                            break;
                        case ComparisonTypes.EnumCountSmallerOrEqual:
                            sqlBuilder.Append($" <= {{{index}}}");
                            break;
                    }
                    break;
                case ComparisonTypes.NumLarger:
                    sqlBuilder.Append($"{column} > {{{index}}}");
                    break;
                case ComparisonTypes.NumLargerOrEqual:
                    sqlBuilder.Append($"{column} >= {{{index}}}");
                    break;
                case ComparisonTypes.NumSmaller:
                    sqlBuilder.Append($"{column} < {{{index}}}");
                    break;
                case ComparisonTypes.NumSmallerOrEqual:
                    sqlBuilder.Append($"{column} <= {{{index}}}");
                    break;

                case ComparisonTypes.StringContains:
                    sqlBuilder.Append($"{column} LIKE {{{index}}}");
                    value = "%" + criterion.Value + "%";
                    break;
                case ComparisonTypes.StringNotContains:
                    sqlBuilder.Append($"{column} NOT LIKE {{{index}}}");
                    value = "%" + criterion.Value + "%";
                    break;
                default:
                    throw new ArgumentException("Unsupported comparison type");
            }

            return value;
        }

        private static void GetEnumCount(
            StringBuilder sqlBuilder,
            string column,
            Type type)
        {
            int count;

            if (type == typeof(byte))
            {
                count = 8;
            }
            else if (type == typeof(short))
            {
                count = 16;
            }
            else if (type == typeof(int))
            {
                count = 32;
            }
            else if (type == typeof(long))
            {
                count = 64;
            }
            else
            {
                throw new ArgumentException("Unsupported underlying enum type");
            }

            sqlBuilder.Append($"case when {column} & 1 = 1 then 1 else 0 end");

            for (long i = 1, x = 2; i < count; i++, x *= 2)
            {
                sqlBuilder.Append($" + case when {column} & {x} = {x} then 1 else 0 end");
            }
        }

        private static void SetSqlSorting(
            StringBuilder sqlBuilder,
            Sorting sorting,
            string column)
        {
            sqlBuilder.Append(" ORDER BY ");

            switch (sorting.SortingMethod)
            {
                case SortingMethod.Normal:
                    sqlBuilder.Append(column);

                    if (!sorting.Ascending)
                    {
                        sqlBuilder.Append(" DESC");
                    }
                    break;
                case SortingMethod.EnumHigh:
                    sqlBuilder.Append(column);

                    if (sorting.Ascending)
                    {
                        sqlBuilder.Append(" DESC");
                    }
                    break;
                case SortingMethod.EnumLow:
                    sqlBuilder.Append($"[SortCol], {column}");
                    break;
            }
        }

        /// <summary>Clear all records</summary>
        /// <param name="dbContext">Database context</param>
        public static async Task RemoveAll<Entity>(this DbSet<Entity> set) where Entity : class
        {
            SpeurzoekersDbContext dbContext = set.GetDbContext();
            string sql = $"DELETE FROM {GetTableName<Entity>(dbContext)}";

            await dbContext.Database.ExecuteSqlCommandAsync(sql);
        }

        /// <summary>Clear the records of which the given property matches any value from the list</summary>
        /// <param name="property">Lambda expression to property; x => x.[PROPERTY]</param>
        /// <param name="list">List of values</param>
        public static async Task RemoveWhereIn<Entity, Prop>(
            this DbSet<Entity> set,
            Expression<Func<Entity, Prop>> property,
            IEnumerable<string> list)
                where Entity : class
        {
            SpeurzoekersDbContext dbContext = set.GetDbContext();
            string table = GetTableName<Entity>(dbContext),
                prop = ((MemberExpression)property.Body).Member.Name,
                column = GetColumns<Entity>(dbContext)[prop],
                paraList = string.Join(",", Enumerable.Range(0, list.Count()).Select(x => "{" + x + "}")),
                sql = $"DELETE FROM {table} WHERE {column} IN ({paraList})";

            await dbContext.Database.ExecuteSqlCommandAsync(sql, list);
        }

        /// <summary>Updates the given property with the given expression</summary>
        /// <param name="property">Lambda expression to property; x => x.[PROPERTY]</param>
        /// <param name="updateExpression">Update expression, inluding '='</param>
        public static async Task UpdateWithExpression<Set, Prop>(
            this DbSet<Set> set,
            Expression<Func<Set, Prop>> property,
            string updateExpression)
                where Set : class
        {
            SpeurzoekersDbContext dbContext = set.GetDbContext();
            string table = GetTableName<Set>(dbContext),
                prop = ((MemberExpression)property.Body).Member.Name,
                column = GetColumns<Set>(dbContext)[prop],
                sql = $"UPDATE {table} SET {column} {updateExpression}";

            await dbContext.Database.ExecuteSqlCommandAsync(sql);
        }

        /// <summary>(Re-)Attaches an entity and updates it with the data model (does NOT work on sub-classes)</summary>
        /// <param name="model">Data model</param>
        /// <returns></returns>
        public static async Task Update<Entity, Model>(this DbSet<Entity> set, Model model) 
            where Entity : class
            where Model : class
        {
            SpeurzoekersDbContext dbContext = set.GetDbContext();
            string keyName = GetPrimaryKeyName<Entity>(dbContext);
            object keyValue = model.GetType().GetProperty(keyName).GetValue(model, null);
            Entity entity = await set.FindAsync(keyValue);

            if (entity != null)
            {
                set.Attach(entity).CurrentValues.SetValues(model);
            }
        }

        /// <summary>(Re-)Attaches the entities and updates them with the data models (does NOT work on sub-classes)</summary>
        /// <param name="models">List of data models</param>
        /// <returns></returns>
        public static async Task UpsertRange<Entity, Model, Prop, Id>(
            this DbSet<Entity> set,
            IList<Model> childeren,
            Expression<Func<Entity, Prop>> foreignkey,
            Prop foreignkeyValue,
            Func<Id> idGenerator,
            Id emptyId)
            where Entity : class, new()
            where Model : class
        {
            SpeurzoekersDbContext dbContext = set.GetDbContext();
            string keyName = GetPrimaryKeyName<Entity>(dbContext);
            PropertyInfo key = typeof(Model).GetProperty(keyName);
            var cols = GetColumns<Entity>(dbContext);
            string table = GetTableName<Entity>(dbContext),
                foreignkeyCol = cols[((MemberExpression)foreignkey.Body).Member.Name];

            // Delete
            if (childeren.Count > 0)
            {
                var list = new List<object>(childeren.Count + 1);
                string paraList = string.Join(",", Enumerable.Range(1, childeren.Count).Select(x => "{" + x + "}")),
                    sql = $"DELETE FROM {table} WHERE {foreignkeyCol} = {{0}} AND {cols[keyName]} NOT IN ({paraList})";

                list.Add(foreignkeyValue);
                list.AddRange(childeren.Select(x => key.GetValue(x, null)));

                await dbContext.Database.ExecuteSqlCommandAsync(sql, list);
            }
            else
            {
                string sql = $"DELETE FROM {table} WHERE {foreignkeyCol} = {{0}}";

                await dbContext.Database.ExecuteSqlCommandAsync(sql, foreignkeyValue);
            }

            foreach (Model child in childeren)
            {
                if (!key.GetValue(child, null).Equals(emptyId))
                {
                    object keyValue = child.GetType().GetProperty(keyName).GetValue(child, null);
                    Entity entity = await set.FindAsync(keyValue);
                    set.Attach(entity).CurrentValues.SetValues(child);
                }
                else
                {
                    key.SetValue(child, idGenerator());
                    Entity entity = new Entity();
                    set.Attach(entity).CurrentValues.SetValues(child);
                    set.Add(entity);
                }
            }
        }

        private static string GetPrimaryKeyName<Entity>(SpeurzoekersDbContext dbContext)
            where Entity : class
        {
            return dbContext.Model.FindEntityType(typeof(Entity)).FindPrimaryKey().Properties
                .Select(x => x.Name)
                .Single();
            
        }

        private static string GetTableName<Entity>(SpeurzoekersDbContext dbContext)
        {
            string table = dbContext.Model.FindEntityType(typeof(Entity)).Relational().TableName;
            return $"[dbo].[{table}]";
        }

        private static Dictionary<string, string> GetColumns<Entity>(SpeurzoekersDbContext dbContext)
        {
            return dbContext.Model.FindEntityType(typeof(Entity))
                  .GetProperties()
                  .ToDictionary(x => x.Name, y => $"[{y.Relational().ColumnName}]");
        }

        private static SpeurzoekersDbContext GetDbContext<Entity>(this IQueryable<Entity> dbSet) where Entity : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext))
                                       as ICurrentDbContext;
            return (SpeurzoekersDbContext)currentDbContext.Context;
        }
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
    }
}
