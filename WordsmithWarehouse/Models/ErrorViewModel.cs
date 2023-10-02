using System;

namespace WordsmithWarehouse.Models
{
    public class ErrorViewModel : GlobalViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
