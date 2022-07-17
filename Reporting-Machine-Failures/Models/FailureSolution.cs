namespace ReportingMachineFailures.Models
{
    public class FailureSolution
    {
        private static int _id = 1;
        public int Id { get; }
        public User UserReceiver { get; set; }
        public string SolutionDescription { get; set; }
        public decimal? ExpectedSolutionCoastMin { get; set; }
        public decimal? ExpectedSolutionCoastMax { get; set; }


        public FailureSolution()
        {
            Id = _id++;
        }

    }
}
