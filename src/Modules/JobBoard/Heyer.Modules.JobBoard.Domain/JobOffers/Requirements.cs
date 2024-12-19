namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record Requirements
{
    public ExperienceLevel ExperienceLevel { get; private set; }
    public IEnumerable<Skill> Skills { get; private set; } = null!;

    private Requirements()
    {
        
    }
    
    public Requirements(ExperienceLevel experienceLevel, IEnumerable<Skill> skills)
    {
        ExperienceLevel = experienceLevel;
        Skills = skills;
    }
}