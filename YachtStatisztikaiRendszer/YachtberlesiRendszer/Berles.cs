namespace YachtberlesiRendszer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Berles
    {
        [Key]
        public int Id { get; set; }
        public int Uid { get; set; }
        public int YachtId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DailyPrice { get; set; }
        public int Deposit { get; set; }

        [NotMapped]
        public int TotalPrice => (int)((EndDate - StartDate).TotalDays + 1) * DailyPrice;
    }
}
