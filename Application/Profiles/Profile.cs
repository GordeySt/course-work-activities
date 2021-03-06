using System.Collections.Generic;
using Domain;

namespace Application.Profiles
{
    public class Profile
    {
        public string DisplayedName { get; set; }
        public string UserName { get; set; }
        public string MainImage { get; set; }
        public string Bio { get; set; }
        public bool IsFollowing { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}