using ReportingMachineFailures.Enums;
using ReportingMachineFailures.Models;

namespace ReportingMachineFailures.Services
{
    public class FailureService
    {
        public List<Failure> Failures { get; set; }

        public FailureService()
        {
            Failures = new List<Failure>();
        }

        public int AddNewFailure(User userApplicant,
                                      FailureLocation location,
                                      List<FailureKind> failureKind,
                                      FailureStatusEnum status = FailureStatusEnum.New)
        {
            var newFailure = new Failure()
            {
                UserApplicant = userApplicant,
                Location = location,
                Status = status
            };

            foreach (var failure in failureKind)
            {
                newFailure.FailureKind.Add(failure);
            }

            Failures.Add(newFailure);

            return newFailure.Id;
        }
        public int AddNewFailure(User userApplicant, Failure failure)
        {
            Failures.Add(failure);

            return failure.Id;
        }

        public int UpdateFailure(int userId, int existFailureId, Failure newFailure)
        {
            var existFailure = GetUserFailure(userId, existFailureId);

            var failureIndex = Failures.FindIndex(x => x.Id == existFailure.Id);

            if (failureIndex < 0 ||  Failures[failureIndex].Status != FailureStatusEnum.New)
            {
                return -1;
            }

            Failures[failureIndex].FailureKind = newFailure.FailureKind;
            Failures[failureIndex].Location = newFailure.Location;
            Failures[failureIndex].Status = newFailure.Status;
            Failures[failureIndex].UserApplicant = newFailure.UserApplicant;

            return Failures[failureIndex].Id;
        }

        public int DeleteFailure(int id)
        {
            var failureToDelete = Failures.Where(x => x.Id == id).FirstOrDefault();

            if (failureToDelete != null && (failureToDelete.Status == FailureStatusEnum.New || failureToDelete.Status == FailureStatusEnum.Closed))
            {
                Failures.Remove(failureToDelete);
                return failureToDelete.Id;
            }

            return -1;
        }

        public int AddSolutionToFailure(int applicantUserId, int failureID, FailureSolution solution)
        {
            var failure = GetUserFailure(applicantUserId, failureID);

            if (failure == null)
            {

                return -1;
            }

            failure.FailureSolututions.Add(solution);

            return solution.Id;
        }

        public List<Failure> GetFailureSolutions(int receiverID)
        {
            List<Failure> failureSolutions = new List<Failure>();

            var failures = GetAllFailures();

            foreach(var failure in failures)
            {
                var solution = failure.FailureSolututions?.Where(x => x.UserReceiver.Id == receiverID).FirstOrDefault();
                if(solution != null)
                {
                    failureSolutions.Add(failure);
                }
            }

            return failureSolutions;
        }
        public List<Failure> GetAllFailures()
        {
            return Failures;
        }

        public List<Failure> GetUserFailure(int userId)
        {
            return Failures.Where(x => x.UserApplicant.Id == userId).ToList();
        }

        public Failure GetUserFailure(int userId, int failureId)
        {
            return Failures.Where(x => x.UserApplicant.Id == userId && x.Id == failureId).FirstOrDefault();
        }
    }
}