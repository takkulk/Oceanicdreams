using System.ComponentModel.DataAnnotations.Schema;

namespace Yachtberles.Models
{
    public class Berles
    {
        public int Id { get; set; }

        public int Uid { get; set; }

        public int YachtId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DailyRate { get; set; }


        public int BaseFee { get; set; }

        [NotMapped]
        public int TotalPrice
        {
            get
            {
                int napok = (EndDate - StartDate).Days + 1;
                return BaseFee + (napok * DailyRate);
            }
        }
    }
}
