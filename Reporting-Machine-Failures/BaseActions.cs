using ReportingMachineFailures.Models;

namespace ReportingMachineFailures
{
    public abstract class BaseActions
    {
        public void WriteMenuOnConsole(List<Menu> menu)
        {
            foreach (var item in menu)
            {
                Console.WriteLine(item.Name);
            }
        }

        public int ChangeStringToInt(string userInput)
        {
            int output;

            int.TryParse((string)userInput, out output);

            return output;
        }

        protected void PrintReports(List<Failure> failureList)
        {
            ClearConsole.Clear();

            Console.Write("ID:" + " | " + "Country:" + " | " + "City:" + " | " + "Street:" + " | " + "Factory:" + " | ");
            Console.Write("Machine:" + " | " + "Description:" + " | " + "Status:" + " | " + "Applicant Id:" + " | " + "Applicant Name:" + " | " + "Applicant Surname:" + " | " + "Accepted Solution:");

            foreach (var failure in failureList)
            {
                Console.Write("\r\n" + failure.Id + ". | ");

                Console.Write(failure.Location.Country + " | ");
                Console.Write(failure.Location.City + " | ");
                Console.Write(failure.Location.Street + " | ");
                Console.Write(failure.Location.Factory + " | ");
                Console.Write(failure.Location.Machine + " | ");
                Console.Write(failure.Location.Description + " | ");

                Console.Write(failure.Status + " | ");

                Console.Write(failure.UserApplicant.Id + " | ");
                Console.Write(failure.UserApplicant.Name + " | ");
                Console.Write(failure.UserApplicant.Surname + " | ");

                foreach (var item2 in failure.FailureKind)
                {
                    Console.Write(item2.Kind + " | ");
                    Console.Write(item2.Description + " | ");
                }
                Console.Write(failure.AcceptedFailureSolution?.Id + " " + failure.AcceptedFailureSolution?.UserReceiver.Surname, Console.ForegroundColor = ConsoleColor.Green);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.ReadLine();
        }
    }
}
