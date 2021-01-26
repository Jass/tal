using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TestAndLearn;
using TestAndLearn.DI;

namespace WAPart.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string IM_trans { get; set; }
        public string IM_scope { get; set; }
        public string IM_sing { get; set; }

        public string IM_tran2 { get; set; }
        public string IM_scop2 { get; set; }
        public string IM_sing2{ get; set; }

        private PeopleContext _db;
        public IndexModel(ILogger<IndexModel> logger,CallRandom call , CallRandom call2 , PeopleContext db)
        {
            _logger = logger;
            IM_trans = call._Transy;
            IM_sing = call._Single;
            IM_scope = call._Scoped;

            IM_tran2 = call2._Transy;
            IM_scop2 = call2._Scoped;
            IM_sing2 = call2._Single;
            _db = db;
        }

        public void OnGet()
        {
            LoadSampleData();
            var people = _db.People
                .Include(a => a.Addresses)
                .Include(e => e.EmailAddresses)
                .ToList();
        }

        private void LoadSampleData()
        {
            if (_db.People.Count() == 0)
            {
                string file = System.IO.File.ReadAllText("generated.json");
                var people = JsonSerializer.Deserialize<List<Person>>(file);
                _db.AddRange(people);
                _db.SaveChanges();
            }
        }
    }
}
