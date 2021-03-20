using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;
using Application.Activities.Commands;
using Domain;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> ActivitiesList() => await Mediator.Send(new ActivitiesList.Query());

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ActivityDto>> Details(Guid id) => await Mediator.Send(new ActivityDetails.Query { Id = id });

        [HttpPost]
        public async Task<IActionResult> Create(Activity activity) => Ok(await Mediator.Send(new CreateActivity.Command
        {
            Activity = activity
        }));

        [HttpPut("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<IActionResult> Edit(Guid id, Activity activity)
        {
            activity.Id = id;
            return Ok(await Mediator.Send(new EditActivity.Command { Activity = activity }));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await Mediator.Send(new DeleteActivity.Command{ Id = id }));

        [HttpPost("{id}/attend")]
        public async Task<ActionResult<Unit>> Attend(Guid id)
        {
            return await Mediator.Send(new AttendActivity.Command { Id = id });
        }

        [HttpDelete("{id}/attend")]
        public async Task<ActionResult<Unit>> Unattend(Guid id)
        {
            return await Mediator.Send(new UnattendActivity.Command { Id = id });
        }
    }
}