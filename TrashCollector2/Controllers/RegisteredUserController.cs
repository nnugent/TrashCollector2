using GoogleMaps.LocationServices;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrashCollector2.Models;

namespace TrashCollector2.Controllers
{
    public class RegisteredUserController : Controller
    {
        private ApplicationDbContext _context;

        public RegisteredUserController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult UserMenu()
        {
            return View();
        }

        public ActionResult AccountDetails()
        {
            string userId = User.Identity.GetUserId();
            var employee = _context.UsersInfo.Select(e => e).Where(e => e.UserId == userId).FirstOrDefault();
            if (employee != null)
            {
                var viewModel = new UserInfo()
                {
                    PhoneNumber = employee.PhoneNumber,
                    Id = employee.Id,
                    Name = employee.Name,
                    Address1 = employee.Address1,
                    Address2 = employee.Address2,
                    City = employee.City,
                    State = employee.State,
                    ZipCode = employee.ZipCode,
                    Balance = employee.Balance,
                    PickUpDayId = employee.PickUpDayId
                };
                return View(viewModel);
            }
            return View();
        }

        public ActionResult SaveDetails(UserInfo userInfo)
        {
            var userId = User.Identity.GetUserId();
            var oldInfo = _context.UsersInfo.Where(m => m.UserId == userId).FirstOrDefault();

            var address = new AddressData();
            address.Address = userInfo.Address1 + " " + userInfo.Address2;
            address.City = userInfo.City;
            address.State = userInfo.State;
            address.Country = "United States";
            address.Zip = userInfo.ZipCode;

            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);

            userInfo.PhoneNumber.Trim();
            userInfo.PhoneNumber = userInfo.PhoneNumber.Replace(" ", "");

            if (CheckPhoneNumberFormat(userInfo.PhoneNumber))
            {
                if (userInfo.UserId == "")
                {
                    userInfo.UserId = userId;
                    userInfo.PickUpDayId = 1;
                    _context.UsersInfo.Add(userInfo);
                }
                else
                {
                    oldInfo.Name = userInfo.Name;
                    oldInfo.Address1 = userInfo.Address1;
                    oldInfo.Address2 = userInfo.Address2;
                    oldInfo.City = userInfo.City;
                    oldInfo.State = userInfo.State;
                    oldInfo.ZipCode = userInfo.ZipCode;
                    oldInfo.PhoneNumber = userInfo.PhoneNumber;
                }
                try
                {
                    oldInfo.Latitude = point.Latitude;
                    oldInfo.Longitude = point.Longitude;
                    _context.SaveChanges();
                }
                catch
                {

                }
            }
            return View("AccountDetails", userInfo);
        }

        public ActionResult AccountSummary()
        {
            var pickUpDays = _context.PickUpDays.ToList();
            var userId = User.Identity.GetUserId();
            var balance = _context.UsersInfo.Select(m => m).Where(m => m.UserId.Equals(userId)).FirstOrDefault();
            var viewModel = new AccountSummaryViewModel()
            {
                PickUpDays = pickUpDays,
                BalanceInfo = balance,
                StripeBalance = balance.Balance * 100
            };
            return View(viewModel);
        }

        public ActionResult ChangePickUpDay(AccountSummaryViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var info = _context.UsersInfo.Where(m => m.UserId.Equals(userId)).FirstOrDefault();
            if (model.BalanceInfo.PickUpDayId != null)
            {
                info.PickUpDayId = model.BalanceInfo.PickUpDayId;
                _context.SaveChanges();
            }
            return RedirectToAction("AccountSummary");
        }

        public ActionResult MakePayment(AccountSummaryViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var info = _context.UsersInfo.Where(m => m.UserId.Equals(userId)).FirstOrDefault();
            info.Balance = 0;
            _context.SaveChanges();
            return RedirectToAction("AccountSummary");
        }

        public ActionResult ScheduleExtraPickUp()
        {
            var userId = User.Identity.GetUserId();
            var viewModel = new ScheduleExtraPickUpViewModel()
            {
                Days = _context.PickUpDays.ToList(),
                UserDetails = _context.UsersInfo.Select(m => m).Where(m => m.UserId.Equals(userId)).FirstOrDefault()
        };
            return View(viewModel);
        }

        public ActionResult AddExtraPickUp(ScheduleExtraPickUpViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var userInfo = _context.UsersInfo.Where(m => m.UserId == userId).FirstOrDefault();
            userInfo.ExtraPickUpId = model.UserDetails.ExtraPickUpId;
            _context.SaveChanges();
            var viewModel = new ScheduleExtraPickUpViewModel()
            {
                UserDetails = model.UserDetails,
                Days = model.Days
            };
            return View("Success", viewModel);
        }

        public ActionResult SetUpSkipWeeks()
        {
            var userId = User.Identity.GetUserId();
            var viewModel = _context.UsersInfo.Where(m => m.UserId == userId).FirstOrDefault();
            return View(viewModel);
        }

        public ActionResult SaveSkipWeeks(UserInfo model)
        {
            var userId = User.Identity.GetUserId();
            var userDetails = _context.UsersInfo.Where(m => m.UserId == userId).FirstOrDefault();

            userDetails.PickedUp = model.PickedUp;

            _context.SaveChanges();

            return RedirectToAction("SaveSuccess");
        }

        public ActionResult SaveSuccess()
        {
            var userId = User.Identity.GetUserId();
            var userDetails = _context.UsersInfo.Where(m => m.UserId == userId).FirstOrDefault();
            return View(userDetails);
        }

        public bool CheckPhoneNumberFormat(string phoneNumber)
        {
            bool result = false;
            if ((phoneNumber.Substring(0, 1)) == "(" && (phoneNumber.Substring(4, 1)) == ")" && (phoneNumber.Substring(8, 1)) == "-")
            {
                result = true;
            }
            else
            {
                return result;
            }
            if (result)
            {
                try
                {
                    Convert.ToInt32(phoneNumber.Substring(1, 3));
                    Convert.ToInt32(phoneNumber.Substring(5, 3));
                    Convert.ToInt32(phoneNumber.Substring(9, 4));
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

    }
}