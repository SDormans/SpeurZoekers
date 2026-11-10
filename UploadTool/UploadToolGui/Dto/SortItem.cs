using Speurzoekers.Common.Helper.Enums;
using System;
using System.Linq.Expressions;

namespace UploadToolGui.Dto
{
    public class SortItem
    {
        public string Name { get; private set; }
        public string PropertyName { get; private set; }
        public SortingMethod SortingMethod { get; private set; }

        public SortItem()
        { }

        public static SortItem Create<entity, Prop>(
            string name,
            Expression<Func<entity, Prop>> property,
            SortingMethod sortingMethod = SortingMethod.Normal)
        {
            SortItem item = new SortItem
            {
                Name = name,
                PropertyName = ((MemberExpression)property.Body).Member.Name,
                SortingMethod = sortingMethod
            };

            return item;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
