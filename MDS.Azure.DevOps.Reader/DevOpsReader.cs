﻿using MDS.Azure.DevOps.Reader.Models;
using MDS.Azure.DevOps.Reader.Resources;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDS.Azure.DevOps.Reader
{
    public class DevOpsReader
    {
        public DevOpsReader(string tfsUrl)
        {
            _tfsUrl = tfsUrl;
            _workItemStore = CreateItemStore();
        }

        private string _tfsUrl { get; set; }

        private WorkItemStore _workItemStore { get; set; }

        private WorkItemStore CreateItemStore()
        {
            //var networkCredentials = new NetworkCredential("ruslan.runchev", "December2020", "metinvest");

            //var windowsCredentials = new Microsoft.VisualStudio.Services.Common.WindowsCredential(networkCredentials);

            //VssCredentials basicCredentials = new VssCredentials(windowsCredentials);

            var tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(_tfsUrl));

            tpc.EnsureAuthenticated();

            WorkItemStore workItemStore = new WorkItemStore(tpc);

            return workItemStore;
        }


        public List<WIActivityDto> GetActivities(List<string> persons, DateTime dateStart, DateTime? dateEnd)
        {
            var result = new List<WIActivityDto>();

            if (persons.Count == 0) return result;

            var queryStr = WIQL.GetActivity.Replace("@persons", string.Join(",", persons.Select(x => $"'{x}'")));
            queryStr = queryStr.Replace("@dateFrom", dateStart.ToString("yyyy-MM-dd"));
            DateTime dateTo = dateEnd ?? DateTime.Now.Date;
            queryStr = queryStr.Replace("@dateTo", dateTo.ToString("yyyy-MM-dd"));

            var query = new Query(_workItemStore, queryStr);

            WorkItemCollection workItems = query.RunQuery();

            foreach (WorkItem workItem in workItems)
            {
                var item = new WIActivityDto();

                item.Id = (int)workItem.Fields["ID"].Value;
                item.Name = workItem.Fields["Title"].Value.ToString();
                item.AssigndTo = workItem.Fields["Assigned To"].Value.ToString();
                item.TargetDate = (DateTime?)workItem.Fields["Target Date"].Value;
                item.State = workItem.Fields["State"].Value.ToString();
                item.AreaPath = workItem.Fields["Area Path"].Value.ToString();

                if (workItem.Fields["Completed Work"].Value != null)
                    item.CompletedWork = Convert.ToDecimal(workItem.Fields["Completed Work"].Value);

                result.Add(item);
            }

            GetTasks(result);

            return result;
        }

        public List<WITaskDto> GetTasks(List<int> taskIds)
        {
            var result = new List<WITaskDto>();

            if (taskIds.Count == 0) return result;

            var queryStr = WIQL.GetTasksById.Replace("@id", string.Join(",", taskIds.Select(x => $"'{x}'")));

            var query = new Query(_workItemStore, queryStr);

            WorkItemCollection workItems = query.RunQuery();

            foreach (WorkItem workItem in workItems)
            {
                var item = new WITaskDto();


                item.Id = (int)workItem.Fields["ID"].Value;
                item.Name = workItem.Fields["Title"].Value.ToString();
                item.FinishDate = (DateTime?)workItem.Fields["Finish Date"].Value;
                item.State = workItem.Fields["State"].Value.ToString();
                item.StartDate = (DateTime?)workItem.Fields["Start Date"].Value;
                item.mdsTaskDescription1 = workItem.Fields["mdsTaskDescription1"].Value.ToString();
                item.mdsTaskDescription2 = workItem.Fields["mdsTaskDescription2"].Value.ToString();
                item.mdsTaskWorkType = workItem.Fields["mdsTaskWorkType"].Value.ToString();
                item.mdsTaskActive = workItem.Fields["mdsTaskActive"].Value.ToString();
                item.Description = workItem.Fields["Description"].Value.ToString();


                if (workItem.Fields["Original Estimate"].Value != null)
                    item.OriginalEstimate = Convert.ToDecimal(workItem.Fields["Original Estimate"].Value);

                if (workItem.Fields["Completed Work"].Value != null)
                    item.CompletedWork = Convert.ToDecimal(workItem.Fields["Completed Work"].Value);

                result.Add(item);
            }

            return result;
        }

        private void GetTasks(List<WIActivityDto> activites)
        {
            if (activites.Count == 0) return;

            var queryStr = WIQL.GetTaskLinks.Replace("@activityId", string.Join(",", activites.Select(x => x.Id)));

            Query wiQuery = new Query(_workItemStore, queryStr);

            List<WorkItemLinkInfo> links = wiQuery.RunLinkQuery().ToList();

            links.RemoveAll(x => x.LinkTypeId != -2);

            var taskId = links.Select(x => x.TargetId).ToList();

            var queryStr2 = WIQL.GetTasks.Replace("@id", string.Join(",", taskId));

            Query query = new Query(_workItemStore, queryStr2);

            WorkItemCollection workItems = query.RunQuery();

            var tasks = new List<WITaskDto>();

            foreach (WorkItem workItem in workItems)
            {
                tasks.Add(new WITaskDto
                {
                    Id = (int)workItem.Fields["ID"].Value,
                    Name = workItem.Fields["Title"].Value.ToString(),
                    FinishDate = (DateTime?)workItem.Fields["Finish Date"].Value,
                    State = workItem.Fields["State"].Value.ToString(),
                    StartDate = (DateTime?)workItem.Fields["Start Date"].Value,
                    mdsTaskDescription1 = workItem.Fields["mdsTaskDescription1"].Value.ToString(),
                    mdsTaskDescription2 = workItem.Fields["mdsTaskDescription2"].Value.ToString(),
                    mdsTaskWorkType = workItem.Fields["mdsTaskWorkType"].Value.ToString(),
                    mdsTaskActive = workItem.Fields["mdsTaskActive"].Value.ToString(),
                    OriginalEstimate = Convert.ToDecimal(workItem.Fields["Original Estimate"].Value),
                    Description = workItem.Fields["Description"].Value.ToString()
                });
            }

            foreach (var link in links)
            {
                var task = tasks.FirstOrDefault(x => x.Id == link.TargetId);

                var activity = activites.FirstOrDefault(x => x.Id == link.SourceId);

                activity.Task = task;
            }
        }
    }
}
