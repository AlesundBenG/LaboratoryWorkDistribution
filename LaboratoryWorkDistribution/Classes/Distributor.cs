namespace LaboratoryWorkDistribution.Classes
{
    public class Distributor
    {
        public LaboratoryWork LaboratoryWork { get; private set; }

        public Distributor(LaboratoryWork laboratoryWork) {
            this.LaboratoryWork = laboratoryWork;
        }

        public DistributedLaboratoryWork Distribute() {
            var result = InitializeResult();
            var random = new Random();
            var freeTasks = new List<string>(this.LaboratoryWork.Tasks);
            var averageTaskCount = this.LaboratoryWork.Tasks.Count / this.LaboratoryWork.Languages.Count;
            //Average task distribution;
            foreach (var language in this.LaboratoryWork.Languages) {
                for (int i = 0; i < averageTaskCount; i++) {
                    var taskIndex = random.Next(0, freeTasks.Count);
                    result[language].Add(freeTasks[taskIndex]);
                    freeTasks.RemoveAt(taskIndex);
                }
            }
            //Remain task distribution.
            for (int i = freeTasks.Count; i > 0; i--) {
                var minCountTasks = result.Min(x => x.Value.Count);
                var languages = result.Where(x => x.Value.Count == minCountTasks).Select(x => x.Key).ToList();
                var languageIndex = random.Next(0, languages.Count());
                var taskIndex = random.Next(0, freeTasks.Count);
                result[languages[languageIndex]].Add(freeTasks[taskIndex]);
                freeTasks.RemoveAt(taskIndex);
            }
            Validate(result);
            return result;
        }

        private DistributedLaboratoryWork InitializeResult() {
            var result = new DistributedLaboratoryWork();
            foreach (var language in this.LaboratoryWork.Languages) {
                result.Add(language, new List<string>());
            }
            return result;
        }

        private void Validate(DistributedLaboratoryWork distributed) {
            var distributedTasks = distributed.SelectMany(x => x.Value);

            var severalDistributedTasks = distributedTasks.GroupBy(task => task).Where(group => group.Count() > 1);
            if (severalDistributedTasks.Any()) {
                throw new Exception($"Several times distributed tasks: {string.Join(", ", severalDistributedTasks.Select(task => $"{task.Key} {task.Count()} pcs."))}");
            }

            var notDistributedTasks = this.LaboratoryWork.Tasks.Where(task => !distributedTasks.Contains(task));
            if (notDistributedTasks.Any()) {
                throw new Exception($"Not distributed tasks: {string.Join(", ", notDistributedTasks)}!");
            }
        }
    }
}
