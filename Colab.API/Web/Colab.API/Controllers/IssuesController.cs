﻿namespace Colab.API.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using Colab.API.DataTransferObjects.Issues;
    using Colab.API.InputModels;
    using Colab.API.InputModels.Issues;
    using Colab.Data;
    using Colab.Models;

    using Microsoft.AspNet.Identity;

    public class IssuesController : BaseApiController
    {
        public IssuesController(IColabData data)
            : base(data)
        {
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]IssueDto issue)
        {
            var newIssue = new Issue
            {
                Title = issue.Title,
                Status = issue.Status,
                Priority = issue.Priority,
                TeamId = issue.TeamId,
                ReporterId = issue.ReporterId,
                AssigneeId = issue.AssigneeId
            };

            this.Data.Issues.Add(newIssue);
            this.Data.SaveChanges();

            return this.Ok(new 
            {
                teamId = issue.TeamId,
                issueId = newIssue.Id
            });
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var projects = this.Data.Issues
                .All()
                .Select(IssueDto.ToDto)
                .ToList();

            return this.Ok(projects);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var issue = this.Data.Issues
                .All()
                .Where(x => x.Id == id)
                .Select(IssueDto.ToDto)
                .FirstOrDefault();

            if (issue == null)
            {
                return this.NotFound();
            }

            return this.Ok(issue);
        }

        [HttpPost]
        public IHttpActionResult Update([FromBody]IssueInputModel inputModel)
        {
            var issueToUpdate = this.Data.Issues
                .All()
                .FirstOrDefault(x => x.Id == inputModel.Id);

            if (issueToUpdate == null)
            {
                return this.NotFound();
            }

            inputModel.UpdateEntity(issueToUpdate);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var issueToDelte = this.Data.Issues
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (issueToDelte == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.Identity.GetUserId();

            if (issueToDelte.ReporterId != currentUserId)
            {
                return this.Unauthorized();
            }

            this.Data.Issues.Delete(issueToDelte);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
