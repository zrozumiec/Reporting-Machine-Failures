using ReportingMachineFailures.Enums;

namespace ReportingMachineFailures.Models
{
    public class Failure
    {
        private static int _id = 1;
        public int Id { get; }
        public FailureLocation Location { get; set; }
        public FailureStatusEnum Status { get; set; }
        public User UserApplicant { get; set; }
        public FailureSolution AcceptedFailureSolution { get; set; }
        public List<FailureSolution> FailureSolututions { get; set; }
        public List<FailureKind> FailureKind { get; set; }

        public Failure()
        {
            Id = _id++;

            FailureKind = new();
            FailureSolututions = new();

        }
    }
}