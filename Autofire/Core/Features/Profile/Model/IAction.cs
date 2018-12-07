namespace Autofire.Core.Features.Profile.Model
{
    public interface IAction
    {
        string Name { get; set; }
        string Key { get; set; }
        decimal Interval { get; set; }
    }
}