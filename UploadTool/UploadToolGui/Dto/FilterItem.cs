using UploadToolGui.Dto.Enums;
using System;
using System.Linq.Expressions;

namespace UploadToolGui.Dto
{
    public class FilterItem
    {
        public string Name { get; private set; }
        public FilterType FilterType { get; private set; }
        public string PropertyName { get; private set; }
        public Type Type { get; private set; }

        private FilterItem()
        { }

        public static FilterItem Create<entity, prop>  (
            string name,
            Expression<Func<entity, prop>> property,
            FilterType filterType)
        {
            FilterItem item = new FilterItem
            {
                Name = name,
                FilterType = filterType,
                PropertyName = ((MemberExpression)property.Body).Member.Name,
                Type = typeof(prop)
            };

            return item;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
