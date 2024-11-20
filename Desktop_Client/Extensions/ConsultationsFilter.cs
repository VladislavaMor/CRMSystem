using CRM_Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Desktop_Client
{
    internal static class ConsultationsFilter
    {
        public static IList<Appeal> FilterByDate(IList<Appeal> consultations,
           DateTime firstDate, DateTime lastDate)
        {
            var filtered = new List<Appeal>();
            filtered = consultations.Where(c => c.Created.Date >= lastDate.Date && c.Created.Date <= firstDate.Date).ToList();
            return filtered;
        }
    }
}
