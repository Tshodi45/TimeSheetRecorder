using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TimeSheetRecorder.Data
{

    public class timeSheetRecorderContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeSheetDetails> TimeSheetDetails { get; set; }
        public DbSet<TimeSheetHeader> TimeSheetHeader { get; set; }


        public timeSheetRecorderContext(DbContextOptions<timeSheetRecorderContext> options) : base(options)
        {

        }
        ///protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        ///{
        ///    string connString = @"DATA SOURCE=localhost:1521-XEPDB1; PERSIST SECURITY INFO=True;USER ID=DEMOUSER; password=demouser123; Pooling = False; ";
        ///    optionsBuilder.UseOracle(connString);
        ///}
    }

    [Table("Project")]
    public class Project
    {
        public Project()
        {
            this.TimeSheetDetails = new HashSet<TimeSheetDetails>();
        }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TimeSheetDetails> TimeSheetDetails { get; set; }
    }

    [Table("TimeSheetDetails")]
    public partial class TimeSheetDetails
    {
        public Guid TimeSheetDetailsId { get; set; }
        public Guid TimeSheetHeaderId { get; set; }

        [DataType(DataType.Date)]
        public DateTime ActivityDate { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public int ActivityHours { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual TimeSheetHeader TimeSheetHeader { get; set; }
    }

    [Table("TimeSheetHeader")]
    public partial class TimeSheetHeader
    {
        public TimeSheetHeader()
        {
            this.TimeSheetDetails = new HashSet<TimeSheetDetails>();
        }
        public Guid TimeSheetHeaderId { get; set; }
        public int Week { get; set; }
        public string UserId { get; set; }
        public DateTime CaptureDate { get; set; }
        public virtual ICollection<TimeSheetDetails> TimeSheetDetails { get; set; }
    }
}