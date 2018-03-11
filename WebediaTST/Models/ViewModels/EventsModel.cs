using WebediaTST.Models.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebediaTST.Models.ViewModels
{
    public class EventModel
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string Path { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime Date { get; set; }
    }

}