using ClassLibrary.Data;
using System.ComponentModel;

namespace ClassLibrary.Entities
{
    public class Message: IEntity
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
    }
}
