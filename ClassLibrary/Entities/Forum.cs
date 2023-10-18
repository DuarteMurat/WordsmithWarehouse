using ClassLibrary.Data;
using System;

namespace ClassLibrary.Entities
{
    public class Forum : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
