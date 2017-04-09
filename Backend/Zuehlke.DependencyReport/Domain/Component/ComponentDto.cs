namespace Zuehlke.DependencyReport
{
    public class ComponentDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ApplicationDto Application { get; set; }
    }
}