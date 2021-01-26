using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestAndLearn.DI;

namespace WAPart.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {


        public string IM_trans { get; set; }
        public string IM_scope { get; set; }
        public string IM_sing { get; set; }

        public string IM_tran2 { get; set; }
        public string IM_scop2 { get; set; }
        public string IM_sing2 { get; set; }


        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger, CallRandom call, CallRandom call2)
        {
            _logger = logger;

            IM_trans = call._Transy;
            IM_sing = call._Single;
            IM_scope = call._Scoped;

            IM_tran2 = call2._Transy;
            IM_scop2 = call2._Scoped;
            IM_sing2 = call2._Single;
        }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
