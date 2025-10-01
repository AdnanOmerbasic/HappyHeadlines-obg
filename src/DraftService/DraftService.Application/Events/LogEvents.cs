using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftService.Application.Events
{
    public class LogEvents
    {
        public const int CreatedDraft = 4000;
        public const int UpdatedDraft = 4001;
        public const int DeletedDraft = 4002;
        public const int RetrivedDraftById = 4003;
        public const int RetrivedAllDrafts = 4004;

        public const int FailedToCreateDraft = 4500;
        public const int FailedToUpdateDraft = 4501;
        public const int FailedToDeleteDraft = 4502;
        public const int FailedToRetriveDraftById = 4503;
        public const int FailedToRetriveAllDrafts = 4504;
    }
}
