using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrashCollector2.Models
{
    public class PickUpDay
    {
        public int Id { get; set; }

        public string Day { get; set; }

        public bool CurrentDay { get; set; }
    }
}