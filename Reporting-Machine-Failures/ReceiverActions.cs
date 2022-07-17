using ReportingMachineFailures.Enums;
using ReportingMachineFailures.Models;
using ReportingMachineFailures.Services;

namespace ReportingMachineFailures
{
    public class ReceiverActions : BaseActions
    {
        private readonly FailureService _failureService;
        private readonly User _user;

        public ReceiverActions(FailureService failureService, User user)
        {
            _failureService = failureService;
            _user = user;
        }

        public void Actions()
        {
            var action = ChangeStringToInt(Console.ReadLine());

            switch (action)
            {
                case 1:
                    ShowAllFailures();
                    break;
                case 2:
                    AddFailureSolution();
                    break;
                case 3:
                    ShowYourSolution();
                    break;
                case 4:
                    break;
            }
        }

        private void ShowYourSolution()
        {
           var solutions =  _failureService.GetFailureSolutions(_user.Id);

            PrintReports(solutions);
        }

        private void AddFailureSolution()
        {
            ClearConsole.Clear();

            Console.WriteLine("Give user ID from where you want to take failure:");
            var applicantFailureId = ChangeStringToInt(Console.ReadLine());
            Console.WriteLine("Give ID of failure to add solution:");
            var failureId = ChangeStringToInt(Console.ReadLine());

            var failure = _failureService.GetUserFailure(applicantFailureId, failureId);

            if (failure == null || failure.Status != FailureStatusEnum.New || failure.AcceptedFailureSolution is not null)
            {
                Console.WriteLine("Failure does not exist!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Write solution description:");
            var solutionDescription = Console.ReadLine();

            Console.WriteLine("Give expected min coast:");
            var coastMin = ChangeStringToInt(Console.ReadLine());

            Console.WriteLine("Give expected max coast:");
            var coastMax = ChangeStringToInt(Console.ReadLine());

            var solution = new FailureSolution()
            {
                UserReceiver = _user,
                SolutionDescription = solutionDescription,
                ExpectedSolutionCoastMin = coastMin,
                ExpectedSolutionCoastMax = coastMax
            };

            var result = _failureService.AddSolutionToFailure(applicantFailureId, failureId, solution);
            Console.WriteLine($"Solution id {result} added successfully!");
            Console.ReadLine();
        }

        private void ShowAllFailures()
        {
            var allFailures = _failureService.GetAllFailures();

            PrintReports(allFailures);
        }
    }
}
