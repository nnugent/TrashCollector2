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
    [Authorize(Roles = "TrashCollectorEmployee")]
    public class TrashCollectorController : Controller
    {
        private ApplicationDbContext _context;

        public TrashCollectorController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult EmployeeMenu()
        {
            return View();
        }

        public ActionResult AddPickUpDay()
        {
            return View();
        }

        public ActionResult NewPickUpDay(PickUpDay newDay)
        {
            bool exists = CheckIfExists(newDay);
            if (!exists && newDay.Day != "" && newDay.Day != null)
            {
                _context.PickUpDays.Add(newDay);
                _context.SaveChanges();
            }
            return RedirectToAction("AddPickUpDay");
        }

        public ActionResult ChangeCurrentDay()
        {
            var viewModel = new ChangeDayViewModel()
            {
                Days = _context.PickUpDays.ToList()
            };
            return View(viewModel);
        }

        public ActionResult NextDay()
        {
            var days = _context.PickUpDays.ToList();
            var registeredUsers = _context.UsersInfo.ToList();

            

            for(int i = 0; i < days.Count(); i++)
            {
                if (days[i].CurrentDay)
                {
                    days[i].CurrentDay = false;
                    _context.SaveChanges();
                    try
                    {
                        days[i + 1].CurrentDay = true;
                        _context.SaveChanges();
                    }
                    catch
                    {
                        days[0].CurrentDay = true;
                        foreach (UserInfo el in registeredUsers)
                        {
                            if(el.PickedUp > 0) el.PickedUp--;
                        }
                        _context.SaveChanges();
                    }
                    break;
                }
            }

            return RedirectToAction("ChangeCurrentDay");
        }

        public ActionResult ViewAllPickUps()
        {
            var viewModel = new ViewAllPickUpsViewModel()
            {
                Days = _context.PickUpDays.ToList(),
                RegisteredUsers = _context.UsersInfo.ToList()
            };
            return View(viewModel);
        }

        public ActionResult ViewMyPickUps()
        {
            var userId = User.Identity.GetUserId();
            var viewModel = new ViewMyPickUpsViewModel()
            {
                Days = _context.PickUpDays.ToList(),
                RegisteredUsers = _context.UsersInfo.ToList(),
                EmployeeInfo = _context.UsersInfo.Where(m => m.UserId == userId).FirstOrDefault()
            };

            return View(viewModel);
        }

        public ActionResult ChargePickUp(int userId)
        {
            var person = _context.UsersInfo.Where(m => m.Id == userId).FirstOrDefault();

            person.Balance += 10.00;
            person.PickedUp++;

            _context.SaveChanges();

            return RedirectToAction("ViewAllPickUps");
        }

        public ActionResult ChargeExtraPickUp(int userId)
        {
            var person = _context.UsersInfo.Where(m => m.Id == userId).FirstOrDefault();

            person.Balance += 10.00;
            person.ExtraPickUpId = null;

            _context.SaveChanges();

            return RedirectToAction("ViewAllPickUps");
        }

        public ActionResult ChargeMyExtraPickUp(int userId)
        {
            var person = _context.UsersInfo.Where(m => m.Id == userId).FirstOrDefault();

            person.Balance += 10.00;
            person.ExtraPickUpId = null;

            _context.SaveChanges();

            return RedirectToAction("ViewAllPickUps");
        }

        public ActionResult ChargeMyPickUp(int userId)
        {
            var person = _context.UsersInfo.Where(m => m.Id == userId).FirstOrDefault();

            person.Balance += 10.00;
            person.PickedUp++;

            _context.SaveChanges();

            return RedirectToAction("ViewMyPickUps");
        }

        public ActionResult ViewUserAddress(string userId)
        {
            var person = _context.UsersInfo.Where(m => m.UserId == userId).FirstOrDefault();
            var viewModel = new UserInfo()
            {
                Name = person.Name,
                Address1 = person.Address1,
                City = person.City,
                State = person.State,
                ZipCode = person.ZipCode,
                Latitude = person.Latitude,
                Longitude = person.Longitude
            };
            return View(person);
        }

        public ActionResult ViewAreaMap()
        {
            var days = _context.PickUpDays.ToList();
            var registeredUsers = _context.UsersInfo.ToList();
            List<double> latitudes = new List<double>();
            List<double> longitudes = new List<double>();
            List<string> names = new List<string>();

            foreach(PickUpDay el in days)
            {
                if (el.CurrentDay)
                {
                    foreach(UserInfo u in registeredUsers)
                    {
                        if((u.PickUpDayId == el.Id || u.ExtraPickUpId == el.Id) && u.PickedUp == 0)
                        {
                            latitudes.Add(u.Latitude);
                            longitudes.Add(u.Longitude);
                            names.Add(u.Name);
                        }
                    }
                }
            }

            var viewModel = new ViewAreaMapViewModel()
            {
                Latitudes = latitudes,
                Longitudes = longitudes,
                Users = names
            };
            return View(viewModel);
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

        public bool CheckIfExists(PickUpDay pickUpDay)
        {
            var count = _context.PickUpDays.Select(s => s.Day).Where(s => s.Equals(pickUpDay.Day)).Count();
            if (count > 0)
            {
                return true;
            }
            return false;
        }
    }
}