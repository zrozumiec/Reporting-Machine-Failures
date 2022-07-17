using ReportingMachineFailures.Enums;

namespace ReportingMachineFailures.Models
{
    public class User
    {
        private static int _id = 1;

        public int Id { get; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserKindEnum UserKind { get; set; }
        public List<Failure> FailureList { get; set; }

        public User()
        {
            Id = _id++;

            FailureList = new();
        }
    }
}
