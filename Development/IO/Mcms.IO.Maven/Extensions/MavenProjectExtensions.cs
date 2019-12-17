using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcms.IO.Core.Artifacts;

namespace Mcms.IO.Maven.Extensions
{
    public static class MavenProjectExtensions
    {
        public static async Task<IDictionary<string, List<IArtifact>>> GetArtifactsForGameVersions(this MavenProject project)
        {
            return (await project.GetArtifactsAsync()).Values.GroupBy(a => a.GameVersion)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public static async Task<List<IArtifact>> GetArtifactsForGameVersion(this MavenProject project,
            string gameVersion)
        {
            return (await project.GetArtifactsAsync()).Values.Where(a => a.GameVersion == gameVersion).ToList();
        }
    }
}