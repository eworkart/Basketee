using Basketee.API.DAOs;
using Basketee.API.DTOs.Common;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.Services
{

    public class CommonUserServices
    {
        public List<User> GetUsers()
        {
            UserDao userDao = new UserDao();
            DriverDao driverDao = new DriverDao();

            List<User> userList = new List<User>();

            List<Consumer> consumers = null;
            consumers = userDao.GetConsumers();

            if (consumers != null && consumers.Count() > 0)
            {
                foreach(Consumer cnr in consumers)
                {
                    userList.Add(new User()
                    {
                        ID = cnr.ConsID,
                        UserName = cnr.Name,
                        Password = cnr.Password,
                        Roles = "Consumer",
                        MobileNo = cnr.PhoneNumber,
                        AccToken = cnr.AccToken
                    });
                }
            }

            List<Driver> drivers = null;
            drivers = driverDao.GetDrivers();

            if (drivers != null && drivers.Count() > 0)
            {
                foreach (Driver dvr in drivers)
                {
                    userList.Add(new User()
                    {
                        ID = dvr.DrvrID,
                        UserName = dvr.DriverName,
                        Password = dvr.Password,
                        Roles = "Driver",
                        MobileNo = dvr.MobileNumber,
                        AccToken = dvr.AccToken
                    });
                }
            }

            return userList;
        }
    }
}
