using LaboratoryWorkDistribution.Classes;

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