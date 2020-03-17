using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Excel.Helpers
{
    public static class AttributeHelpers
    {
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);

            var property = instance.GetType().GetProperty(propertyName);

            return (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();
        }

        public static T GetAttributeFromType<T>(this Type type, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);

            var property = type.GetProperty(propertyName);

            return  (T)property.GetCustomAttributes(attrType, true).FirstOrDefault();            
        }
    }
}
