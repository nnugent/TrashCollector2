using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollector2.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public int? PickUpDayId { get; set; }
        
        [ForeignKey("PickUpDayId")]
        [Display(Name = "Pick up Day")]
        public PickUpDay PickUpDay { get; set; }

        public Double Balance { get; set; }

        public Double Latitude { get; set; }

        public Double Longitude { get; set; }

        public int PickedUp { get; set; }

        public int? ExtraPickUpId { get; set; }

        [ForeignKey("ExtraPickUpId")]
        [Display(Name = "Pick up Day")]
        public PickUpDay ExtraPickUpDay { get; set; }

    }

    public class AccountSummaryViewModel
    {
        public List<PickUpDay> PickUpDays { get; set; }

        public UserInfo BalanceInfo { get; set; }

        public double StripeBalance { get; set; }
    }

    public class ScheduleExtraPickUpViewModel
    {
        public List<PickUpDay> Days { get; set; }

        public UserInfo UserDetails { get; set; }

    }
}