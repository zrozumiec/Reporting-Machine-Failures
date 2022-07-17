namespace ReportingMachineFailures.Models
{
    public class Menu
    {
        private static int _id = 1;
        public int Id { get; }

        public string Name { get; set; }

        public string MenuName { get; set; }

        public Menu()
        {
            Id = _id++;
        }
    }
}
