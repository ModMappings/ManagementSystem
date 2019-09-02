using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Data.Core.Readers.Core;
using Data.Core.Writers.Parameters;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Parameter
{
    public class ParameterMappingWriter
        : IParameterMappingWriter
    {

        private readonly MCPContext _context;

        public ParameterMappingWriter(MCPContext context)
        {
            _context = context;
        }

        public async Task<ParameterMapping> GetById(Guid id)
        {
            return await _context.ParameterMappings.FirstOrDefaultAsync(parameterMappings =>
                parameterMappings.Id == id);
        }

        public async Task<IQueryable<ParameterMapping>> AsMappingQueryable()
        {
            return _context.ParameterMappings;
        }

        public async Task<IQueryable<ParameterMapping>> GetByLatestRelease()
        {
            var latest = await _context.Releases.OrderByDescending(release => release.CreatedOn).FirstOrDefaultAsync();
            return await this.GetByRelease(latest);
        }

        public async Task<IQueryable<ParameterMapping>> GetByRelease(Guid releaseId)
        {
            return _context.ParameterMappings.Where(parameterMapping =>
                parameterMapping.VersionedMappings.Any(versionedMapping =>
                    versionedMapping.CommittedMappings.Any(committedMapping =>
                        committedMapping.Releases.Select(release => release.Id).Contains(releaseId))));
        }

        public async Task<IQueryable<ParameterMapping>> GetByRelease(string releaseName)
        {
            var release = await _context.Releases.FirstOrDefaultAsync(r => r.Name == releaseName);
            return await this.GetByRelease(release);
        }

        public async Task<IQueryable<ParameterMapping>> GetByRelease(Release release)
        {
            return await this.GetByRelease(release.Id);
        }

        public async Task<IQueryable<ParameterMapping>> GetByLatestVersion()
        {
            return await this.GetByVersion(await _context.GameVersions.OrderByDescending(release => release.CreatedOn)
                .FirstOrDefaultAsync());
        }

        public async Task<IQueryable<ParameterMapping>> GetByVersion(Guid versionId)
        {
            return _context.ParameterMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion.Id == versionId));
        }

        public async Task<IQueryable<ParameterMapping>> GetByVersion(string versionName)
        {
            return _context.ParameterMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion.Name == versionName));
        }

        public async Task<IQueryable<ParameterMapping>> GetByVersion(GameVersion version)
        {
            return _context.ParameterMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion == version));
        }

        public async Task<IQueryable<ParameterMapping>> GetByLatestMapping(string name)
        {
            return _context.ParameterMappings.Where(parameterMapping =>
                parameterMapping.VersionedMappings.OrderByDescending(versionedMapping => versionedMapping.CreatedOn)
                    .FirstOrDefault().CommittedMappings
                    .Any(committedMapping => committedMapping.OutputMapping == name || committedMapping.InputMapping == name));
        }

        public async Task<IQueryable<ParameterMapping>> GetByMappingInVersion(string name, Guid versionId)
        {
            return _context.ParameterMappings.Where(parameterMapping =>
                parameterMapping.VersionedMappings.Any(versionedMapping =>
                    versionedMapping.GameVersion.Id == versionId &&
                    versionedMapping.CommittedMappings.Any(committedMapping =>
                        committedMapping.OutputMapping == name || committedMapping.InputMapping == name)));
        }

        public async Task<IQueryable<ParameterMapping>> GetByMappingInVersion(string name, GameVersion gameVersion)
        {
            return await this.GetByMappingInVersion(name, gameVersion.Id);
        }

        public async Task<IQueryable<ParameterMapping>> GetByMappingInRelease(string name, Guid releaseId)
        {
            return _context.ParameterMappings.Where(parameterMapping =>
                parameterMapping.VersionedMappings.Any(versionedMapping =>
                    versionedMapping.CommittedMappings.Any(committedMapping =>
                        (committedMapping.OutputMapping == name || committedMapping.InputMapping == name) &&
                        committedMapping.Releases.Select(release => release.Release.Id).Contains(releaseId))));
        }

        public async Task<IQueryable<ParameterMapping>> GetByMappingInRelease(string name, Release release)
        {
            return await this.GetByMappingInRelease(name, release.Id);
        }

        public async Task<ParameterVersionedMapping> GetVersionedMapping(Guid id)
        {
            return await _context.ParameterVersionedMappings.FirstOrDefaultAsync(mapping => mapping.Id == id);
        }

        public async Task<ParameterProposalMappingEntry> GetProposal(Guid proposalEntry)
        {
            return await _context.ParameterProposalMappingEntries.FirstOrDefaultAsync(mapping => mapping.Id == proposalEntry);
        }

        public async Task<ParameterCommittedMappingEntry> GetCommittedEntry(Guid committedEntryId)
        {
            return await _context.ParameterCommittedMappingEntries.FirstOrDefaultAsync(mapping => mapping.Id == committedEntryId);
        }

        public async Task<IQueryable<ParameterMapping>> GetByMethodInLatestVersion(Guid methodId)
        {
            var latestVersion = await _context.GameVersions.OrderByDescending(gameVersion => gameVersion.CreatedOn)
                .FirstOrDefaultAsync();

            return await this.GetByMethodInVersion(methodId, latestVersion.Id);
        }

        public async Task<IQueryable<ParameterMapping>> GetByMethodInLatestRelease(Guid methodId)
        {
            var latestRelease = await _context.Releases.OrderByDescending(release => release.CreatedOn)
                .FirstOrDefaultAsync();

            return await this.GetByMethodInVersion(methodId, latestRelease.Id);
        }

        public async Task<IQueryable<ParameterMapping>> GetByMethodInLatestVersion(MethodMapping methodMapping)
        {
            return await GetByMethodInLatestVersion(methodMapping.Id);
        }

        public async Task<IQueryable<ParameterMapping>> GetByMethodInLatestRelease(MethodMapping methodMapping)
        {
            return await GetByMethodInLatestRelease(methodMapping.Id);
        }

        public async Task<IQueryable<ParameterMapping>> GetByMethodInVersion(Guid methodId, Guid versionId)
        {
            return _context.ParameterMappings.Where(parameterMapping =>
                parameterMapping.VersionedMappings.Any(versionedMapping =>
                    versionedMapping.GameVersion.Id == versionId && versionedMapping.ParameterOf.Id == methodId));
        }

        public async Task<IQueryable<ParameterMapping>> GetByMethodInRelease(Guid methodId, Guid releaseId)
        {
            return _context.ParameterMappings.Where(parameterMapping =>
                parameterMapping.VersionedMappings.Any(versionedMapping =>
                    versionedMapping.ParameterOf.Id == methodId && versionedMapping.CommittedMappings.Any(committedMappings =>
                        committedMappings.Releases.Select(release => release.Id).Contains(releaseId))));
        }

        public async Task<IQueryable<ParameterMapping>> GetByMethodInVersion(MethodMapping methodMapping, Guid versionId)
        {
            return await this.GetByMethodInVersion(methodMapping.Id, versionId);
        }

        public async Task<IQueryable<ParameterMapping>> GetByMethodInRelease(MethodMapping methodMapping, Guid releaseId)
        {
            return await this.GetByMethodInRelease(methodMapping.Id, releaseId);
        }

        public async Task Add(ParameterMapping mapping)
        {
            if (mapping is ParameterMapping parameterMapping)
            {
                await _context.ParameterMappings.AddAsync(parameterMapping);
            }
        }

        public async Task Update(ParameterMapping mapping)
        {
            if (mapping is ParameterMapping parameterMapping)
            {
                _context.Entry(parameterMapping).State = EntityState.Modified;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddProposal(ParameterProposalMappingEntry proposalEntry)
        {
            await _context.ParameterProposalMappingEntries.AddAsync(proposalEntry);
        }
    }
}
