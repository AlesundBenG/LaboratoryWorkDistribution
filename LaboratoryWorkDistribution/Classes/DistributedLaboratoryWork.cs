namespace LaboratoryWorkDistribution.Classes
{
    public partial class DistributedLaboratoryWork : Dictionary<string, List<string>>
    {
        public override string ToString() {
            return string.Join("\n", this.Select(language => $"{language.Key}: {string.Join(", ", language.Value.OrderBy(x => x))}"));
        }
    }
}
