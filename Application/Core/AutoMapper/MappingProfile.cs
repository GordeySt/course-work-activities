using System.Linq;
using Application.Activities;
using Application.Comments.Dto;
using AutoMapper;
using Domain;

namespace Application.Core.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            string currentUserName = null;

            CreateMap<Activity, ActivityDto>();
            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(d => d.UserName, options => options.MapFrom(source => source.AppUser.UserName))
                .ForMember(d => d.DisplayedName, options => options.MapFrom(source => source.AppUser.DisplayedName))
                .ForMember(d => d.MainImage, options => options.MapFrom(source => source.AppUser.Photos.FirstOrDefault(x =>
                x.IsMain).Url))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count()))
                .ForMember(d => d.FollowingsCount, o => o.MapFrom(s => s.AppUser.Followings.Count()))
                .ForMember(d => d.IsFollowing, o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName
                    == currentUserName)))
                .ForMember(d => d.MainImage, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Comment, CommentDto>()
               .ForMember(d => d.UserName, o => o.MapFrom(s => s.Author.UserName))
               .ForMember(d => d.DisplayedName, o => o.MapFrom(s => s.Author.DisplayedName))
               .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x
               => x.IsMain).Url));
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.MainImage, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count()))
                .ForMember(d => d.FollowingsCount, o => o.MapFrom(s => s.Followings.Count()))
                .ForMember(d => d.IsFollowing, o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUserName)));
            CreateMap<UserActivity, Profiles.UserActivityDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Activity.Id))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Activity.Date))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Activity.Title))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Activity.Category))
                .ForMember(d => d.HostUserName, o => o.MapFrom(s => 
                    s.Activity.UserActivities.FirstOrDefault(x => x.IsHost).AppUser.UserName));
        }
    }
}