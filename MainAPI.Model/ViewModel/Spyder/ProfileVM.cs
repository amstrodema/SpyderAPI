using MainAPI.Models.Spyder;
using MainAPI.Models.Comment.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class ProfileVM
    {
        public Guid ID { get; set; }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public string Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Comments { get; set; }
        public DateTime Date { get; set; }
        public DateTime SortDate { get; set; }
        public Comment.Spyder.Comment Comment { get; set; }
        public bool voteType { get; set; }
    }
}
