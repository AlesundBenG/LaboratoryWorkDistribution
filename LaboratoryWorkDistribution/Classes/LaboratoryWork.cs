
namespace LaboratoryWorkDistribution.Classes
{
    public class LaboratoryWork
    {
        public List<string> Languages { get; private set; }

        public List<string> Tasks { get; private set; }

        public LaboratoryWork(List<string> languages, List<string> tasks) {
            this.Languages = languages;
            this.Tasks = tasks;
        }
    }
}
