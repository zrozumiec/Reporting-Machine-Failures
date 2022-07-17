using ReportingMachineFailures;
using ReportingMachineFailures.Enums;
using ReportingMachineFailures.Models;
using ReportingMachineFailures.Services;

FailureService failureService = new FailureService();

Console.WriteLine("----Welcome!----\r\n");

Console.WriteLine("Write your name:");
var userName = Console.ReadLine();

Console.WriteLine("Write your surname:");
var userSurname = Console.ReadLine();

var user = CreateNewUser(userName, userSurname, UserKindEnum.WithoutRole);

ClearConsole.Clear();

var menu = new MenuService();
menu = InitializeMenus(menu);

var userKindInput = 0;

InitialData(user);

var applicantActions = new ApplicantActions(failureService, user, menu);
var receiverActions = new ReceiverActions(failureService, user);

do
{
    ClearConsole.Clear();
    Console.WriteLine(@$"{user.Name} choose your role please:");

    var userKindMenu = menu.GetMenusByMenuName("UserKindMenu");

    applicantActions.WriteMenuOnConsole(userKindMenu);

    int.TryParse(Console.ReadLine(), out userKindInput);

    user.UserKind = (UserKindEnum)userKindInput;

    switch (user.UserKind)
    {
        case UserKindEnum.Applicant:
            ClearConsole.Clear();

            Console.WriteLine("Applicant menu:");
            var applicantMenu = menu.GetMenusByMenuName("ApplicantMenu");
            applicantActions.WriteMenuOnConsole(applicantMenu);

            applicantActions.Actions();
            break;

        case UserKindEnum.Receiver:
            ClearConsole.Clear();

            Console.WriteLine("Receiver menu:");
            var receiverMenu = menu.GetMenusByMenuName("ReceiverMenu");
            applicantActions.WriteMenuOnConsole(receiverMenu);

            receiverActions.Actions();
            break;

        case UserKindEnum.WithoutRole:
            ClearConsole.Clear();

            Console.WriteLine($"Good bye {user.Name}!");
            break;

        default:
            ClearConsole.Clear();

            Console.WriteLine("Wrong user!");
            break;
    }
} while (user.UserKind != UserKindEnum.WithoutRole);

User CreateNewUser(string name, string surname, UserKindEnum userKindEnum)
{
    var user = new User()
    {
        Name = name,
        Surname = surname,
        UserKind = userKindEnum
    };

    return user;
}

MenuService InitializeMenus(MenuService menuService)
{
    menuService.AddNewMenu("1. Applicant - Choose if you have some problems with machines and want to add new failure report", "UserKindMenu");
    menuService.AddNewMenu("2. Receiver - Choose if you are a service provider and want to troubleshoot failures", "UserKindMenu");
    menuService.AddNewMenu("3. Quit - Choose if you are in wrong place", "UserKindMenu");

    menuService.AddNewMenu("1. Add new failure report", "ApplicantMenu");
    menuService.AddNewMenu("2. Edit you report", "ApplicantMenu");
    menuService.AddNewMenu("3. Delete you report", "ApplicantMenu");
    menuService.AddNewMenu("4. Show all of your reports", "ApplicantMenu");
    menuService.AddNewMenu("5. Show specific report", "ApplicantMenu");
    menuService.AddNewMenu("6. Show all reservations", "ApplicantMenu");
    menuService.AddNewMenu("7. Show specific reservations", "ApplicantMenu");
    menuService.AddNewMenu("8. Close failure", "ApplicantMenu");
    menuService.AddNewMenu("9. Show only closed failures", "ApplicantMenu");
    menuService.AddNewMenu("10. Back", "ApplicantMenu");

    menuService.AddNewMenu("1. Electric", "FailureKind");
    menuService.AddNewMenu("2. Mechanic", "FailureKind");
    menuService.AddNewMenu("3. Pneumatic", "FailureKind");
    menuService.AddNewMenu("4. SoftwarePLC", "FailureKind");
    menuService.AddNewMenu("5. Robotic", "FailureKind");
    menuService.AddNewMenu("6. Other", "FailureKind");

    menuService.AddNewMenu("1. Show all reports", "ReceiverMenu");
    menuService.AddNewMenu("2. Add solution to chosen failure", "ReceiverMenu");
    menuService.AddNewMenu("3. Show all your solutions", "ReceiverMenu");
    menuService.AddNewMenu("4. Back", "ReceiverMenu");


    return menuService;
}

void InitialData(User user)
{
    var applicantUser = user;
    applicantUser.UserKind = UserKindEnum.Applicant;

    var failure = new Failure()
    {
        AcceptedFailureSolution = null,
        FailureKind = new List<FailureKind>()
        {
            new FailureKind { Description = "Voltage to low", Kind = FailureKindEnum.Electric },
            new FailureKind { Description = "Missing interlock to rotate FX01", Kind = FailureKindEnum.SoftwarePLC },
        },
        Location = new FailureLocation()
        {
            Country = "Poland",
            City = "Rzeszów",
            Street = "Lisa Kuli 2",
            Factory = "PSA",
            Machine = "K4AAF21",
            Description = ""
        },
        Status = FailureStatusEnum.New,
        UserApplicant = applicantUser,
        FailureSolututions = new List<FailureSolution>()
        {
            new FailureSolution()
            {
                UserReceiver = new User()
                {
                    Name = "Kamil",
                    Surname = "Joel"
                },
                SolutionDescription = "1. Voltage to low - increase voltage value on supply. 2. PLC - change logic for interlock",
                ExpectedSolutionCoastMin = 1500.0M,
                ExpectedSolutionCoastMax = 1500.0M
            },
            new FailureSolution()
            {
                UserReceiver = new User()
                {
                    Name = "Marius",
                    Surname = "Etkiel"
                },
                SolutionDescription = "1. Voltage to low - change supply. 2. PLC - change logic for interlock",
                ExpectedSolutionCoastMin = 3000.0M,
                ExpectedSolutionCoastMax = 3000.0M
            }
        }
    };

    failure.UserApplicant.FailureList.Add(failure);
    failureService.Failures.Add(failure);


    var failure2 = new Failure()
    {
        AcceptedFailureSolution = null,
        FailureKind = new List<FailureKind>()
        {
            new FailureKind { Description = "Rebuild 45FX10 according with new Pplan", Kind = FailureKindEnum.Mechanic },
        },
        Location = new FailureLocation()
        {
            Country = "Germany",
            City = "Lipsk",
            Street = "Guttera",
            Factory = "VW",
            Machine = "K1Af21",
            Description = "Call +49543234675 to get pass"
        },
        Status = FailureStatusEnum.New,
        UserApplicant = applicantUser,
        FailureSolututions = new List<FailureSolution>()
        {
            new FailureSolution()
            {
                UserReceiver = new User()
                {
                    Name = "John",
                    Surname = "Wicki"
                },
                SolutionDescription = "",
                ExpectedSolutionCoastMin = 5000.0M,
                ExpectedSolutionCoastMax = 6500.0M

            },
            new FailureSolution()
            {
                UserReceiver = new User()
                {
                    Name = "Jan",
                    Surname = "Marious"
                },
                SolutionDescription = "Depending from changes need to be done 3000-7000$",
                ExpectedSolutionCoastMin = 3000.0M,
                ExpectedSolutionCoastMax = 7000.0M,
            }
        }
    };

    failure2.UserApplicant.FailureList.Add(failure2);
    failureService.Failures.Add(failure2);

    var failure3 = new Failure()
    {
        FailureKind = new List<FailureKind>()
        {
            new FailureKind { Description = "Add new program to pick new type of part", Kind = FailureKindEnum.Robotic },
            new FailureKind { Description = "Add logic to service new robot program", Kind = FailureKindEnum.SoftwarePLC },
        },
        Location = new FailureLocation()
        {
            Country = "Germany",
            City = "Lipsk",
            Street = "Guttera",
            Factory = "VW",
            Machine = "K1Af21",
            Description = "Call +49543234675 to get pass"
        },
        Status = FailureStatusEnum.Opened,
        UserApplicant = applicantUser,
        FailureSolututions = new List<FailureSolution>()
        {
            new FailureSolution()
            {
                UserReceiver = new User()
                {
                    Name = "Nico",
                    Surname = "Hadic"
                },
                SolutionDescription = "Integration robot new program + PLC logic",
                ExpectedSolutionCoastMin = 10000.0M,
                ExpectedSolutionCoastMax = 15000.0M

            },
            new FailureSolution()
            {
                UserReceiver = new User()
                {
                    Name = "Frank",
                    Surname = "Moris"
                },
                SolutionDescription = "Robotic - 5000$ + PLC - 7000$",
                ExpectedSolutionCoastMin = 13000.0M,
                ExpectedSolutionCoastMax = 13000.0M,
            }
        },
    };

    failure3.AcceptedFailureSolution = failure3.FailureSolututions[1];
    failure3.UserApplicant.FailureList.Add(failure3);
    failureService.Failures.Add(failure3);

    var failure4 = new Failure()
    {
        FailureKind = new List<FailureKind>()
        {
            new FailureKind { Description = "Air pressure is too low when all clamps on FX50 are moving!", Kind = FailureKindEnum.Pneumatic },
        },
        Location = new FailureLocation()
        {
            Country = "China",
            City = "Foshan",
            Street = "Yhoa 432",
            Factory = "FAW-VW",
            Machine = "BS-121",
            Description = ""
        },
        Status = FailureStatusEnum.New,
        UserApplicant = new User
        {
            Name = "Monica",
            Surname = "Xao",
            UserKind = UserKindEnum.Applicant
        },
        FailureSolututions = new List<FailureSolution>()
        {
            new FailureSolution()
            {
                UserReceiver = new User()
                {
                    Name = "Victor",
                    Surname = "Muller"
                },
                SolutionDescription = "Rebuild pneumatic system",
                ExpectedSolutionCoastMin = 3500.0M,
                ExpectedSolutionCoastMax = 4000.0M

            },
            new FailureSolution()
            {
                UserReceiver = new User()
                {
                    Name = "Mario",
                    Surname = "Ezykiel"
                },
                SolutionDescription = "Pneumatic changes",
                ExpectedSolutionCoastMin = 1000.0M,
                ExpectedSolutionCoastMax = 4000.0M,
            }
        },
    };

    failure4.AcceptedFailureSolution = failure4.FailureSolututions[1];
    failure4.UserApplicant.FailureList.Add(failure4);
    failureService.Failures.Add(failure4);

    var failure5 = new Failure()
    {
        FailureKind = new List<FailureKind>()
        {
            new FailureKind { Description = "Create electrical circuit for new device", Kind = FailureKindEnum.Electric },
        },
        Location = new FailureLocation()
        {
            Country = "Poland",
            City = "Krakow",
            Street = "Wesołą",
            Factory = "VDD",
            Machine = "",
            Description = ""
        },
        Status = FailureStatusEnum.New,
        UserApplicant = new User
        {
            Name = "Jan",
            Surname = "Purek",
            UserKind = UserKindEnum.Applicant
        },
    };

    failure5.UserApplicant.FailureList.Add(failure5);
    failureService.Failures.Add(failure5);
}