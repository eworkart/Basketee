using Basketee.API.DAOs;
using Basketee.API.DTOs;
using Basketee.API.DTOs.Orders;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Basketee.API.Services
{
    public class TimeslotService
    {
        public const double LATITUDE_FOR_5KM = 0.045045, LONGITUDE_FOR_5KM = 0.045045;
        public const string DAY_NAME_TODAY = "Today";
        public const string DAY_NAME_TOMORROW = "Tomorrow";
        public const string DAY_NAME_DAY_AFTER_TOMORROW = "Day after tomorrow";
        public const string DAY_NAME_MONDAY = "Monday";
        public const string DAY_NAME_SUNDAY = "Sunday";
        const double DAYS_TO_LIST = 3;
        public const int MAX_DELIVERY_PER_SLOT = 10;


        public static TimeSlotDto[] GetTimeslots(DateTime searchDateTime, string latitude, string longitude, int agencyId, ResponseDto respDto = null)
        {
            double lat = Convert.ToDouble(latitude), lon = Convert.ToDouble(longitude);
            double lowLat = lat - LATITUDE_FOR_5KM, upLat = lat + LATITUDE_FOR_5KM,
                loLon = lon - LONGITUDE_FOR_5KM, upLon = lon + LONGITUDE_FOR_5KM;

            DateTime startDate = searchDateTime;
            DateTime endDate = searchDateTime.Date.AddDays(DAYS_TO_LIST);
            List<TimeSlotDto> slotDtos = new List<TimeSlotDto>();

            //List<DistributionPoint> dps = null;
            List<DistributionAgency> dps = null;
            using (AgencyDao agentDao = new AgencyDao())
            {
                //var dps1 = agentDao.GetDistributionPointsBetween(Convert.ToString(lowLat),
                //     Convert.ToString(upLat),
                //     Convert.ToString(loLon),
                //     Convert.ToString(upLon));
                var dps1 = agentDao.GetDistributionAgencies(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                if (agencyId > 0)
                {
                    //dps = dps1.Where(dp => dp.AgenID == agencyId).ToList();                    
                    dps = dps1.Where(dp => dp.AgenID == agencyId).ToList();
                }
                else
                {
                    dps = dps1.ToList();
                }
                if (dps.Count == 0)
                {
                    if (respDto != null)
                    {
                        respDto.message = MessagesSource.GetMessage("no.dist");
                    }
                    return slotDtos.ToArray();
                }
            }
            List<MDeliverySlot> slots = null;
            using (DeliverySlotDao slotDao = new DeliverySlotDao())
            {
                slots = slotDao.GetAllSlots();

                //for (DateTime dt = startDate; dt.Date < endDate.Date; dt = dt.AddDays(1))
                //{

                //    foreach (MDeliverySlot slt in slots)
                //    {
                //        TimeSlotDto dto = new TimeSlotDto();
                //        //if (dt.Date == DateTime.Now.Date)
                //        //{
                //        //    dto.day = "Today";
                //        //}
                //        //else if (dt.Date == DateTime.Now.Date.AddDays(1))
                //        //{
                //        //    dto.day = "Tomorrow";
                //        //}
                //        //else if (dt.Date == DateTime.Now.Date.AddDays(2))
                //        //{
                //        //    dto.day = "Day after tomorrow";
                //        //}
                //        dto.date = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);                        
                //        dto.time_slot_id = slt.SlotID;
                //        dto.time_slot_name = slt.SlotName;
                //        //dto.agency_id = agencyId;
                //        dto.availability = slotDao.CheckAvailability(dt, dto.time_slot_id, MAX_DELIVERY_PER_SLOT);
                //        slotDtos.Add(dto);
                //    }
                //}



                //for (DateTime dt = startDate; dt.Date < endDate.Date; dt = dt.AddDays(1))
                //{
                //    TimeslotDisplayDto dto = new TimeslotDisplayDto();
                //    if (dt.Date == DateTime.Now.Date)
                //    {
                //        dto.day_name = "Today";
                //    }
                //    else if (dt.Date == DateTime.Now.Date.AddDays(1))
                //    {
                //        dto.day_name = "Tomorrow";
                //    }
                //    else if (dt.Date == DateTime.Now.Date.AddDays(2))
                //    {
                //        dto.day_name = "Day after tomorrow";
                //    }
                //    dto.day_date = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //    dto.time_slot_days = new List<TimeslotDaysDto>();
                //    foreach (MDeliverySlot slt in slots)
                //    {
                //        TimeslotDaysDto Daydto = new TimeslotDaysDto();
                //        Daydto.time_slot_id = slt.SlotID;
                //        Daydto.time_slot_name = slt.SlotName;
                //       // Daydto.agency_id = agencyId;
                //        Daydto.availability = slotDao.CheckAvailability(dt, Daydto.time_slot_id, MAX_DELIVERY_PER_SLOT);
                //        //dto.time_slot_days()
                //        dto.time_slot_days.Add(Daydto);
                //    }

                //    slotDtos.Add(dto);
                //}
            }
            return slotDtos.ToArray();
        }


        public static TimeslotDisplayDto[] GetTimeslotsDisplay(DateTime searchDateTime, string latitude, string longitude, int agencyId, ResponseDto respDto = null)
        {
            double lat = Convert.ToDouble(latitude), lon = Convert.ToDouble(longitude);
            double lowLat = lat - LATITUDE_FOR_5KM, upLat = lat + LATITUDE_FOR_5KM,
                loLon = lon - LONGITUDE_FOR_5KM, upLon = lon + LONGITUDE_FOR_5KM;

            DateTime startDate = searchDateTime;
            DateTime endDate = searchDateTime.Date.AddDays(DAYS_TO_LIST);
            List<TimeslotDisplayDto> slotDtos = new List<TimeslotDisplayDto>();
            List<DistributionAgency> dps = null;
            using (AgencyDao agentDao = new AgencyDao())
            {
                var dps1 = agentDao.GetDistributionAgencies(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                if (agencyId > 0)
                {
                    dps = dps1.Where(dp => dp.AgenID == agencyId).ToList();
                }
                else
                {
                    dps = dps1.ToList();
                }
                if (dps.Count == 0)
                {
                    if (respDto != null)
                    {
                        respDto.message = MessagesSource.GetMessage("no.dist");
                    }
                    return slotDtos.ToArray();
                }
            }
            List<MDeliverySlot> timeSlots = null;
            using (DeliverySlotDao slotDao = new DeliverySlotDao())
            {
                timeSlots = slotDao.GetAllSlots();
                int count = 1;
                DateTime dt = startDate;
                int addDayCountToday = 0;
                int addDayCountTommorw = 1;
                int addDayCountDayAftrTmrw = 2;
                for (int intCnt = 0; intCnt < DAYS_TO_LIST; intCnt++)
                {
                    TimeSpan elapseTime = TimeSpan.FromMinutes(30);
                    TimeSpan currentTime = DateTime.Now.TimeOfDay.Add(elapseTime);
                    List<MDeliverySlot> slots = new List<MDeliverySlot>();
                    //if (dt.Date == DateTime.Now.Date)
                    //{
                    //    slots = timeSlots.Where(x => x.EndTine >= currentTime).ToList();
                    //    if (slots.Count <= 0)
                    //    {
                    //        dt = dt.AddDays(1);
                    //        slots = timeSlots;
                    //    }
                    //}
                    //else
                    //{
                    //    slots = timeSlots;
                    //}
                    slots = timeSlots;                    
                    if (dt.Date.DayOfWeek.ToString() == DAY_NAME_SUNDAY)
                    {
                        dt = dt.AddDays(1);
                        addDayCountToday++;
                        addDayCountTommorw++;
                        addDayCountDayAftrTmrw++;
                    }
                    
                    TimeslotDisplayDto dto = new TimeslotDisplayDto();
                    if (dt.Date == DateTime.Now.Date.AddDays(addDayCountToday))
                    {
                        dto.day_name = DAY_NAME_TODAY;
                    }
                    else if (dt.Date == DateTime.Now.Date.AddDays(addDayCountTommorw))
                    {
                        dto.day_name = DAY_NAME_TOMORROW;                        
                    }
                    else if (dt.Date == DateTime.Now.Date.AddDays(addDayCountDayAftrTmrw))
                    {
                        dto.day_name = DAY_NAME_DAY_AFTER_TOMORROW;
                    }
                    
                    //else if (dt.Date == DateTime.Now.Date.AddDays(2))
                    //{
                    //    dto.day_name = DAY_NAME_DAY_AFTER_TOMORROW;
                    //}
                    //else
                    //{
                    //    dto.day_name = dt.Date.DayOfWeek.ToString();
                    //}
                    dto.day_date = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dto.time_slot = new List<TimeslotDaysDto>();
                    foreach (MDeliverySlot slt in slots)
                    {
                        TimeslotDaysDto Daydto = new TimeslotDaysDto();
                        Daydto.time_slot_id = slt.SlotID;//count;
                        Daydto.time_slot_name = slt.SlotName;
                        Daydto.availability = ((slt.EndTine < currentTime && dto.day_name == DAY_NAME_TODAY) ? 0 : slotDao.CheckAvailability(dt, Daydto.time_slot_id, MAX_DELIVERY_PER_SLOT)).ToString();
                        dto.time_slot.Add(Daydto);
                        count++;
                    }
                    slotDtos.Add(dto);
                    dt = dt.AddDays(1);
                }
            }
            return slotDtos.ToArray();
        }

        public static bool CheckTimeslotFree(DateTime deliveryDate, string latitude, string longitude, short deliverySlotID, int agencyId)
        {
            var timeslotes = GetTimeslots(deliveryDate, latitude, longitude, agencyId);
            bool free = timeslotes.Any(ts => ts.time_slot_id == deliverySlotID && ts.availability == 1 && ts.agency_id == agencyId);
            return free;
        }
    }
}