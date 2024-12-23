using System.Text.Json.Serialization;

namespace Heyer.Modules.Hiring.PublishedLanguage;

public record Requirements
{
    public ExperienceLevel ExperienceLevel { get; private set; }
    public List<Skill>? Skills { get; private set; }

    private Requirements()
    {
    }

    [JsonConstructor]
    public Requirements(ExperienceLevel experienceLevel, List<Skill> skills)
    {
        ExperienceLevel = experienceLevel;
        Skills = skills;
    }
}