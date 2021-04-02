namespace Application.Activities
{
    public class AttendeeDto
    {
        public string UserName { get; set; }
        public string DisplayedName { get; set; }
        public string Image { get; set; }
        public bool IsHost { get; set; }
        public bool IsFollowing { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
    }
}