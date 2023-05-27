using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crowd_Funding.Models
{
    public class Campaign
    {
        [Key]
        public int CID { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime CampaignDateTime { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Disabled";

        [NotMapped]
        public User User { get; set; }
        [NotMapped]
        public List<Donation>  Donations { get; set; }

        public int UID { get; set; } = 1;
        public string AdminAction { get; set; } = "Pending";

    }
}
