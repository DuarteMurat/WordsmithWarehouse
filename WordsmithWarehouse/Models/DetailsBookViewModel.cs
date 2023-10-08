using ClassLibrary.Entities;
using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class DetailsBookViewModel : BookViewModel
    {
        public List<Comment> Comments { get; set; }

        public List<User> Users { get; set; }

        public string CommentIds { get; set; }

        public string CurrentUserComment { get; set; } 

        public float CurrentUserCommentRating { get; set; }

    }
}
