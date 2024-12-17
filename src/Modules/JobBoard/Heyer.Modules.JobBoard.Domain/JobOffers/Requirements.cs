namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record Requirements
{
    private ExperienceLevel _experienceLevel;
    private IEnumerable<Skill> _skills;

    public Requirements(ExperienceLevel experienceLevel, IEnumerable<Skill> skills)
    {
        _experienceLevel = experienceLevel;
        _skills = skills;
    }
}