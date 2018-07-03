using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loop54.Test.AspNetMvc.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string requestId)
        {
            RequestId = requestId;
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
