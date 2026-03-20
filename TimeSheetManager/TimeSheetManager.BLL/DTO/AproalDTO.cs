using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetManager.BLL.DTO
{
    public class ApprovalRequest
    {
        public int TimesheetId { get; set; }
        public string Comment { get; set; }
    }
}
