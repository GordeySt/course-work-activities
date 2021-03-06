using MediatR;
using System.Collections.Generic;
using Domain;
using System.Threading;
using System.Threading.Tasks;
using Persistance;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.Interfaces;
using Application.Common;
using Application.Core;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace Application.Activities
{
    public class ActivitiesList
    {
        public class Query : IRequest<Result<PagedList<ActivityDto>>>
        {
            public ActivityParams Params { get; set; }
        }

        public class ActivityHandler : Handler, IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly IMapper _mapper;

            public ActivityHandler(DataContext context, IMapper mapper, IUserAccessor userAccessor) : base(context, userAccessor)
            {
                _mapper = mapper;
            }

            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Activities
                    .Where(d => d.Date >= request.Params.StartDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new
                    {
                        currentUserName = _userAccessor.GetCurrentUserName()
                    })
                    .AsQueryable();

                if (request.Params.IsGoing && !request.Params.IsHost)
                {
                    query = query.Where(x => x.UserActivities.Any(a => a.UserName == _userAccessor.GetCurrentUserName()));
                }

                if (request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.UserActivities.FirstOrDefault(x => x.IsHost).UserName ==
                        _userAccessor.GetCurrentUserName());
                }

                var pagedList = await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber,
                    request.Params.PageSize);

                return Result<PagedList<ActivityDto>>.Success(pagedList);
            }
        }
    }
}