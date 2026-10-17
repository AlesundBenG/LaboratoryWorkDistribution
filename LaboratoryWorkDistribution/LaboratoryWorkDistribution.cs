#region Main

//Settings.
var languages = new List<string> { "C", "Pascal", "Python" };
var tasks = new List<string> { "A", "B", "C", "D", "E", "F", "H", "I", "J", "K" };
var studentCount = 64;

//Distribution.
var laboratoryWork = new LaboratoryWork(languages, tasks);
var distributor = new Distributor(laboratoryWork);
for (int i = 0; i < studentCount; i++) {
    var distributed = distributor.Distribute();
    Console.WriteLine($"Student №{i + 1}:");
    Console.WriteLine(distributed.ToString());
    Console.WriteLine();
}

#endregion

#region Classes

public class LaboratoryWork
{
    public List<string> Languages { get; private set; }

    public List<string> Tasks { get; private set; }

    public LaboratoryWork(List<string> languages, List<string> tasks) {
        this.Languages = languages;
        this.Tasks = tasks;
    } 
}

public partial class DistributedLaboratoryWork : Dictionary<string, List<string>>
{
    public override string ToString() {
        return string.Join("\n", this.Select(language => $"{language.Key}: {string.Join(", ", language.Value.OrderBy(x => x))}"));
    }
}

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
            for(int i = 0; i < averageTaskCount; i++) {
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
        foreach(var language in this.LaboratoryWork.Languages) {
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

#endregion