using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class TopicViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public string NewMessageContent { get; set; }

        public string Username { get; set; }
    }
}
