using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWiki.Shared.Models.Users
{
    public class UserStatsModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public int PhotoCount { get; set; }
        public int LocomotiveCount { get; set; }
        public int RollingStockCount { get; set; }
        public int LocationCount { get; set; }
    }
}
