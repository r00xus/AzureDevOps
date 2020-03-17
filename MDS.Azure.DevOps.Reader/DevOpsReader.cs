using MDS.Azure.DevOps.Reader.Models;
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
            var tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(_tfsUrl));

            WorkItemStore workItemStore = new WorkItemStore(tpc);

            return workItemStore;
        }


        public List<WIActivityDto> GetActivities(List<string> persons, DateTime dateStart, DateTime? dateEnd)
        {
            var queryStr = WIQL.GetActivity.Replace("@persons", string.Join(",", persons.Select(x => $"'{x}'")));
            queryStr = queryStr.Replace("@dateFrom", dateStart.ToString("yyyy-MM-dd"));
            DateTime dateTo = dateEnd ?? DateTime.Now.Date;
            queryStr = queryStr.Replace("@dateTo", dateTo.ToString("yyyy-MM-dd"));

            var query = new Query(_workItemStore, queryStr);

            WorkItemCollection workItems = query.RunQuery();

            var result = new List<WIActivityDto>();

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

        private void GetTasks(List<WIActivityDto> activites)
        {
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
