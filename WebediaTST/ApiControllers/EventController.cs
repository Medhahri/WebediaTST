using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebediaTST.Models.Data;
using WebediaTST.Models.ViewModels;
using WebediaTST.Models.Repositories;
using System.Threading.Tasks;

namespace AdneomTST.Controllers
{
    public class EventController : ApiController, IDisposable
    {
        private IEventRepository eventRepository;

        public EventController()
            : this(new EventRepository())
         {}

        public EventController(IEventRepository eventRepository)
        {
            // we can use many container for implement IoC: Unity For example
            this.eventRepository = eventRepository;
        }

        [HttpGet]
        [Route("api/Events/")]
        public IList<EventModel> GetTypeCoffee(int n)
        {
            return eventRepository.GetNLastEvents(n);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                eventRepository.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}