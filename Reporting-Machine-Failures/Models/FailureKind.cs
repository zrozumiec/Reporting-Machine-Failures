using ReportingMachineFailures.Enums;

namespace ReportingMachineFailures.Models
{
    public class FailureKind
    {
        private static int _id = 1;

        public int Id { get; }
        public FailureKindEnum Kind { get; set; }
        public string Description { get; set; }

        public FailureKind()
        {
            Id = _id++;
        }
    }
}
