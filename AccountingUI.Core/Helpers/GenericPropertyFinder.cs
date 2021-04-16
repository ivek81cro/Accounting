using System;
using System.Collections.Generic;

namespace AccountingUI.Core.Helpers
{
    public class GenericPropertyFinder<TModel> where TModel : class
    {
        private readonly List<string> _propName = new List<string>();
        private readonly List<string> _propValue = new List<string>();
        private readonly List<string> _propType = new List<string>();

        public IEnumerable<List<string>> PrintTModelPropertyAndValue(TModel tmodelObj)
        {
            foreach (var property in tmodelObj.GetType().GetProperties())
            {
                _propName.Add(property.Name.ToString());
                _propValue.Add(property.GetValue(tmodelObj).ToString());
                _propType.Add(property.PropertyType.Name.ToString());
            }
            return new List<List<string>> { _propName, _propValue, _propType };
        }
    }
}
