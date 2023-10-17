namespace WordsmithWarehouse.Models
{
    public class TicketDetailsViewModel
    {
        public int Id { get; set; }

        public bool Open { get; set; }

        public bool Close { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }
    }
}
