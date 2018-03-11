using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebediaTST.Models.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using WebediaTST.Models.ViewModels;

namespace WebediaTST.Models.Repositories
{
    public class EventRepository : IEventRepository
    {
        public EventRepository()
        {
            
        }

        /// <summary>
        /// Add Event
        /// </summary>
        /// <param name="coffee"></param>
        public void Add(EventModel eventModel)
        {
            try
            {
                using (WebediaDBEntities1 context = new WebediaDBEntities1())
                {
                    Events events = new Events()
                    {
                        Id = eventModel.Id,
                        EventType = eventModel.EventType,
                        Path = eventModel.Path,
                        Date = eventModel.Date
                    };
                    context.Events.Add(events);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get N Last Events
        /// </summary>
        /// <returns></returns>
        public IList<EventModel> GetNLastEvents(int n)
        {
            using (WebediaDBEntities1 context = new WebediaDBEntities1())
            {
                var NfirstEvents = from b in context.Events
                            select new EventModel()
                            {
                                Id = b.Id,
                                EventType = b.EventType,
                                Path = b.Path,
                                Date = b.Date
                            };

                return NfirstEvents.Take(n).Reverse().ToList();
            }
        }

        public void Dispose()
        {

        }
    }
}