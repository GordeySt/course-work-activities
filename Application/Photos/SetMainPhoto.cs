using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Photos
{
    public class SetMainPhoto
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                var photo = FindPhotoToSetAsMain(user, request);

                CheckIfPhotoWasFound(photo);

                var currentMainPhoto = FindCurrentMainPhoto(user); 

                SetNewMainPhoto(currentMainPhoto, photo);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }

            private Photo FindPhotoToSetAsMain(AppUser user, Command request)
            {
                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                return photo;
            }

            private void CheckIfPhotoWasFound(Photo photo)
            {
                if (photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Photo = "Not Found" });
            }

            private Photo FindCurrentMainPhoto(AppUser user)
            {
                var currentMainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);

                return currentMainPhoto;
            }

            private void SetNewMainPhoto(Photo currentMainPhoto, Photo photoToSet)
            {
                currentMainPhoto.IsMain = false;
                photoToSet.IsMain = true;
            }
        }
    }
}