using System.Reflection;
namespace VotingSurvey.Application.Settings
{
    public class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}