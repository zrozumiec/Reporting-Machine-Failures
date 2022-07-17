using ReportingMachineFailures.Enums;
using ReportingMachineFailures.Models;
using ReportingMachineFailures.Services;

namespace ReportingMachineFailures
{
    public class ApplicantActions : BaseActions
    {
        private readonly FailureService _failureService;
        private readonly User _user;
        private readonly MenuService _menuService;

        public ApplicantActions(FailureService failureService, User user, MenuService menuService)
        {
            _failureService = failureService;
            _user = user;
            _menuService = menuService;
        }

        public void Actions()
        {
            var applicantInput = Console.ReadLine();

            var input = 0;

            int.TryParse((string)applicantInput, out input);

            switch (input)
            {
                case 1:
                    AddFailure();
                    break;

                case 2:
                    UpdateFailure();
                    break;

                case 3:
                    DeleteFailure();
                    break;

                case 4:
                    PrintApplicantReports();
                    break;

                case 5:
                    Console.WriteLine("Give ID of failure you want to print:");
                    var failureToPrint = 0;

                    int.TryParse(Console.ReadLine(), out failureToPrint);
                    PrintApplicantReports(failureToPrint);
                    break;

                case 6:
                    ShowAllReservations();
                    break;

                case 7:
                    var chosenFailureWithSolution =  ShowAllReservations();

                    ShowSpecificReservation(chosenFailureWithSolution);
                    break;

                case 8:
                    CloseFinishedFailure();
                    break;

                case 9:
                    PrintApplicantReports(FailureStatusEnum.Closed);
                    break;

                case 10:
                    break;
            }
        }

        private void AddFailure()
        {
            var failure = AddFailureQuestionnaire();

            _failureService.AddNewFailure(_user, failure);
        }

        private void UpdateFailure()
        {
            Console.WriteLine("Give failure ID you want to edit:");
            var failureToEdit = -1;
            int.TryParse(Console.ReadLine(), out failureToEdit);

            var failureToEditExist = _failureService.GetUserFailure(_user.Id, failureToEdit);

            if (failureToEditExist is null || failureToEditExist.Status != FailureStatusEnum.New)
            {
                Console.WriteLine("Chosen failure to edit does not exist or is reserved by receiver!");
                Console.ReadLine();
                return;
            }

            var failure = AddFailureQuestionnaire();

            _failureService.UpdateFailure(_user.Id, failureToEdit, failure);
        }

        private void DeleteFailure()
        {
            var stringToInt = 0;

            Console.WriteLine("Give ID of failure you want to delete:");
            var failureToDelete = Console.ReadLine();

            int.TryParse(failureToDelete, out stringToInt);

            var result = _failureService.DeleteFailure(stringToInt);

            if (result == -1)
            {
                Console.WriteLine("Given failure Id do not exist in database or is reserved by receiver!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Deleted failure Id {result}");
            Console.ReadLine();
        }

        private Failure ShowAllReservations()
        {
            Console.WriteLine("Give ID of failure:");

            var failureReceiverToPrint = 0;

            int.TryParse(Console.ReadLine(), out failureReceiverToPrint);

            var chosenFailure = _failureService.GetUserFailure(_user.Id, failureReceiverToPrint);
            if (chosenFailure is null && chosenFailure.FailureSolututions.Count <= 0)
            {
                Console.WriteLine("Chosen failure or solution is not existing!");
                Console.ReadLine();
                return null;
            }
            ClearConsole.Clear();
            foreach (var receiver in chosenFailure.FailureSolututions)
            {
                Console.WriteLine(receiver.Id + ". " + receiver.UserReceiver.Name + " " + receiver.UserReceiver.Surname);
            }
            Console.ReadLine();

            return chosenFailure;
        }

        private void ShowSpecificReservation(Failure chosenFailureWithSolution)
        {
            Console.WriteLine("Give ID of solution:");

            var solutionId = -1;

            int.TryParse(Console.ReadLine(), out solutionId);

            var chosenSolution = chosenFailureWithSolution.FailureSolututions.Where(x => x.Id == solutionId).FirstOrDefault();

            if (chosenSolution is null)
            {
                Console.WriteLine("Chosen solution is not existing!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(chosenSolution.Id + ".   " + chosenSolution.UserReceiver.Name + "   " + chosenSolution.UserReceiver.Surname + "   " +
                chosenSolution.SolutionDescription + "   " + chosenSolution.ExpectedSolutionCoastMin + "-" + chosenSolution.ExpectedSolutionCoastMax + "$");
            Console.WriteLine("Do you want to accept this solution? yes/no");

            if (Console.ReadLine().ToLower() != "yes")
            {
                return;
            }
            else if (chosenFailureWithSolution.AcceptedFailureSolution is not null)
            {
                Console.WriteLine("Failure already has accepted solution!");
                Console.ReadLine();
                return;
            }

            chosenFailureWithSolution.AcceptedFailureSolution = chosenSolution;
            chosenFailureWithSolution.Status = FailureStatusEnum.Opened;
            Console.WriteLine("Solution accepted!");
            Console.ReadLine();
        }

        private void CloseFinishedFailure()
        {
            Console.WriteLine("Give ID of finished failure to close:");
            var failureToClose = 0;

            int.TryParse(Console.ReadLine(), out failureToClose);

            var failure = _failureService.Failures.Where(x => x.Id == failureToClose).FirstOrDefault();

            if (failure is null || failure.Status == FailureStatusEnum.Closed)
            {
                Console.WriteLine("Failure does not exist or is already closed!");
                Console.ReadLine();
                return;
            }

            failure.Status = FailureStatusEnum.Closed;
            Console.WriteLine("Failure closed");
            Console.ReadLine();
        }

        private void PrintApplicantReports(int failureId = -1)
        {
            var applicantFailures = new List<Failure>();

            if (failureId >= 0)
            {
                var failure = _failureService.GetUserFailure(_user.Id, failureId);
                if (failure is not null)
                {
                    applicantFailures.Add(failure);
                }
            }
            else
            {
                applicantFailures = _failureService.GetUserFailure(_user.Id);
            }

            if (applicantFailures.Count <= 0)
            {
                Console.WriteLine("Failures not exist!");
                Console.ReadLine();
                return;
            }

            PrintReports(applicantFailures);
        }

        private void PrintApplicantReports(FailureStatusEnum failureStatus)
        {
            var applicantFailures = new List<Failure>();

            applicantFailures = _failureService.Failures.Where(x => x.Status == failureStatus).ToList();

            if (applicantFailures.Count <= 0)
            {
                Console.WriteLine("Failures not existing!");
                Console.ReadLine();
                return;
            }

            PrintReports(applicantFailures);
        }

        private Failure AddFailureQuestionnaire()
        {
            ClearConsole.Clear();
            Console.WriteLine("Give country where factory is:");
            var country = Console.ReadLine();
            Console.WriteLine("Give city where factory is:");
            var city = Console.ReadLine();
            Console.WriteLine("Give street where factory is:");
            var street = Console.ReadLine();
            Console.WriteLine("Give name of factory:");
            var factory = Console.ReadLine();
            Console.WriteLine("Give name of machine with failure:");
            var machine = Console.ReadLine();
            Console.WriteLine("Describe how to get inside the factory and where exactly machine is:");
            var description = Console.ReadLine();

            FailureLocation failureLocation = new FailureLocation()
            {
                Country = country,
                City = city,
                Street = street,
                Factory = factory,
                Machine = machine,
                Description = description,
            };

            List<FailureKind> failureKindsList = new List<FailureKind>();

            do
            {
                var checkIfExist = false;
                ClearConsole.Clear();
                Console.WriteLine("Choose kind of failure:");
                var failureKindMenu = _menuService.GetMenusByMenuName("FailureKind");
                WriteMenuOnConsole(failureKindMenu);

                var failureKindInput = 0;
                int.TryParse(Console.ReadLine(), out failureKindInput);

                foreach (var item in failureKindsList)
                {
                    if (item.Kind == (FailureKindEnum)failureKindInput)
                    {
                        checkIfExist = true;
                        Console.WriteLine("\r\nChosen failure kind exist! Choose other option!\r\n");
                        break;
                    }
                }

                if (!checkIfExist)
                {
                    Console.WriteLine("Describe the failure:");
                    var failureDescription = Console.ReadLine();

                    FailureKind failureKind = new FailureKind()
                    {
                        Kind = (FailureKindEnum)failureKindInput,
                        Description = failureDescription,
                    };

                    failureKindsList.Add(failureKind);
                }

                Console.WriteLine("Do you want to add next kind of failure? yes/no");
            } while (Console.ReadLine().ToLower() != "no");

            return new Failure()
            {
                FailureKind = failureKindsList,
                Location = failureLocation,
                UserApplicant = _user
            };
        }
    }
}