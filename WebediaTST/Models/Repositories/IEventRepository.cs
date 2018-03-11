using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebediaTST.Models.Data;
using System.Threading.Tasks;
using WebediaTST.Models.ViewModels;


namespace WebediaTST.Models.Repositories
{
    public interface IEventRepository : IDisposable
    {
        void Add(EventModel eventModel);
        IList<EventModel> GetNLastEvents(int n);
    }
}