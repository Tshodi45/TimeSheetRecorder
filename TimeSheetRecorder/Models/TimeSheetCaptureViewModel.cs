using Microsoft.Build.Evaluation;
using TimeSheetRecorder.Data;

namespace TimeSheetRecorder.Models
{
    public class TimeSheetCaptureViewModel
    {
        public  TimeSheetCaptureViewModel()
        {
            TimeSheetDetails = new List<TimeSheetDetails>();
        }
        public bool IsNew { get; set; }
        public TimeSheetHeader TimeSheetHeader { get; set; }

        public TimeSheetDetails TimeSheetDetail { get; set; }

        public List<TimeSheetDetails> TimeSheetDetails { get; set; }

    }
}
