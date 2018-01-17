using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.DbUtil.Extensions
{
    public static class ExtensionProperties
    {
        // Storage for the properties.
        private static Dictionary<object,Dictionary<string, object>>PropertyValues = new Dictionary<object, Dictionary<string, object>>();

        // Set a property value for the item.
        public static void SetExtraProperty(this object item,
            string name, object value)
        {
            // If we don't have a dictionary for this item yet,
            // make one.
            if (!PropertyValues.ContainsKey(item))
                PropertyValues[item] = new Dictionary<string, object>();

            // Set the value in the item's dictionary.
            PropertyValues[item][name] = value;
        }

        // Return a property value for the item.
        public static object GetExtraProperty(this object item, string name,
            object default_value)
        {
            // If we don't have a dictionary for
            // this item yet, return the default value.
            if (!PropertyValues.ContainsKey(item)) return default_value;

            // If the value isn't in the dictionary,
            // return the default value.
            if (!PropertyValues[item].ContainsKey(name))
                return default_value;

            // Return the saved value.
            return PropertyValues[item][name];
        }

        // Remove the property.
        public static void RemoveExtraProperty(this object item, string name)
        {
            // If we don't have a dictionary for this item, do nothing.
            if (!PropertyValues.ContainsKey(item)) return;

            // If the value isn't in the dictionary, do nothing.
            if (!PropertyValues[item].ContainsKey(name)) return;

            // Remove the value.
            PropertyValues[item].Remove(name);

            // If the dictionary is empty, remove it.
            if (PropertyValues[item].Count == 0)
                PropertyValues.Remove(PropertyValues[item]);
        }

        // Remove the object's property dictionary.
        public static void RemoveAllExtraProperty(this object item)
        {
            // If we have a dictionary for this item, remove it.
            if (PropertyValues.ContainsKey(item))
                PropertyValues.Remove(PropertyValues[item]);
        }
    }
}
