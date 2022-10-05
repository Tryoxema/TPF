using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TPF.Internal;

namespace TPF.Controls
{
    public class FilteringBehavior
    {
        public virtual IEnumerable<object> FilterItems(IEnumerable items, string searchText, string searchPath, bool ignoreCase, List<object> notMatchedItems)
        {
            var result = new List<object>();

            if (notMatchedItems == null) notMatchedItems = new List<object>();

            if (items == null) return result;

            var regexOption = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

            var regex = new Regex($"({searchText})", regexOption);

            foreach (var item in items)
            {
                // null-Werte überspringen
                if (item == null) continue;

                var baseType = item.GetType();

                // Prufen ob die Liste einen primitiven Datentypen oder String als Typen hat
                if (baseType.IsPrimitive || baseType == typeof(string))
                {
                    // Item direkt vergleichen
                    if (regex.Match(item.ToString()).Success) result.Add(item);
                    else notMatchedItems.Add(item);
                    // Nächsten Wert prüfen
                    continue;
                }

                // Wenn der Wert kein primitiver Datentyp ist brauchen wir einen Suchpfad
                if (searchPath == null) return result;

                var searchValue = PropertyHelper.GetPropertyValueFromPath(item, searchPath);

                if (searchValue == null) continue;

                var type = searchValue.GetType();
                // Es wird nur gefiltern wenn das Ergebnis ein String oder Primitiver Datentyp ist
                // Andernfalls könnten Werte wie "System.Object" ungewollte Ergebnisse liefern
                if (type.IsPrimitive || type == typeof(string))
                {
                    if (regex.Match(searchValue.ToString()).Success) result.Add(item);
                    else notMatchedItems.Add(item);
                }
                else notMatchedItems.Add(item);
            }

            return result;
        }
    }
}