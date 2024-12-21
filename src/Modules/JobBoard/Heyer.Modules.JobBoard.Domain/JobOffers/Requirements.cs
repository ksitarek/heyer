namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record Requirements
{
    public ExperienceLevel ExperienceLevel { get; private set; }
    public List<Skill>? Skills { get; private set; }

    private Requirements()
    {
    }

    public Requirements(ExperienceLevel experienceLevel, List<Skill> skills)
    {
        ExperienceLevel = experienceLevel;
        Skills = skills;
    }
}