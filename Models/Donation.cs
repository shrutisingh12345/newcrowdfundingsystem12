using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crowd_Funding.Models
{
    public class Donation
    {
        [Key]
        public int DID { get; set; }
        [Required]
        public string DonorName { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Amount { get; set; }
        public int CID { get; set; } = 1;
        public DateTime DonationDate { get; set; } = DateTime.Now;

    }
}
