namespace HotelServe.Models
{
    public class News
    {
        public int Id { get; set; }


        public string Title { get; set; }
        public string Contents { get; set; }

        public string Category { get; set; }

        public DateTime Starttime { get; set; }

        public DateTime Endtime { get; set; }

        public bool Enabled { get; set; }

         

        public string Image { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public string Creator { get; set; }
    }
}
