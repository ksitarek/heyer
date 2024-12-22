namespace Heyer.Modules.Hiring.Domain.JobOffers;

public record Skill
{
    public string Label { get; private set; } = null!;
    public SkillLevel Level { get; private set; }

    private Skill()
    {
    }

    public Skill(string label, SkillLevel level)
    {
        Label = label;
        Level = level;
    }
}