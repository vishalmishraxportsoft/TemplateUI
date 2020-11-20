using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Models used only for Front view
namespace Template.Models
{
    // Events models to save data 
    public class Events
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public IFormFile Picture { get; set; }
        public string PicturePath { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoPath { get; set; }
        public string Requirement { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string ParticipationIcon { get; set; }
        //Is the number of seats limited?
        public bool ISsetlimited { get; set; }
        //Number of seats
        public int? Setlimited { get; set; }
        //Do you need approval?
        public bool IsNeedApproval { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }

    // Department Model to save data
    public class Departments
    {
        public int Id { get; set; }

        public string NameAR { get; set; }

        public string NameEN { get; set; }

        public IFormFile Logo { get; set; }
    }
    public class SessionStore
    {
        [JsonProperty("Table")]

        public string status { get; set; }
        public string message { get; set; }
        public List<Department> data{ get; set; }
    }

  
}
