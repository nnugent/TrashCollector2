using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrashCollector2.Models
{
    public class ChangeDayViewModel
    {
        public List<PickUpDay> Days { get; set; }
    }

    public class ViewAllPickUpsViewModel
    {
        public List<PickUpDay> Days { get; set; }

        public List<UserInfo> RegisteredUsers { get; set; }

    }

    public class ViewMyPickUpsViewModel
    {
        public List<PickUpDay> Days { get; set; }

        public List<UserInfo> RegisteredUsers { get; set; }

        public UserInfo EmployeeInfo { get; set; }
    }

    public class ViewAreaMapViewModel
    {
        public List<double> Latitudes { get; set; }

        public List<double> Longitudes { get; set; }

        public List<string> Users { get; set; }

    }

}