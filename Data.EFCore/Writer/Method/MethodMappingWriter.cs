using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Method;
using Data.Core.Readers.Core;
using Data.Core.Writers.Method;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Data.EFCore.Writer.Method
{
    public class MethodMappingWriter
        : IMethodMappingWriter
    {

        private readonly MCPContext _context;

        public MethodMappingWriter(MCPContext context)
        {
            _context = context;
        }

        public async Task<MethodMapping> GetById(Guid id)
        {
            return await _context.MethodMappings.FirstOrDefaultAsync(methodMapping => methodMapping.Id == id);
        }

        public async Task<IQueryable<MethodMapping>> AsMappingQueryable()
        {
            return _context.MethodMappings;
        }

        public async Task<IQueryable<MethodMapping>> GetByLatestRelease()
        {
            var latest = await _context.Releases.OrderByDescending(release => release.CreatedOn).FirstOrDefaultAsync();
            return await GetByRelease(latest);
        }

        public async Task<IQueryable<MethodMapping>> GetByRelease(Guid releaseId)
        {
            return _context.MethodMappings.Where(fieldMapping => fieldMapping.VersionedMappings.Any(versionedMappings =>
                versionedMappings.CommittedMappings.Any(committedMappings =>
                    committedMappings.Releases.Select(release => release.Id).Contains(releaseId))));
        }

        public async Task<IQueryable<MethodMapping>> GetByRelease(string releaseName)
        {
            var namedRelease = await _context.Releases.FirstOrDefaultAsync(release => release.Name == releaseName);
            return await this.GetByRelease(namedRelease);
        }

        public async Task<IQueryable<MethodMapping>> GetByRelease(Release release)
        {
            return await this.GetByRelease(release.Id);
        }

        public async Task<IQueryable<MethodMapping>> GetByLatestVersion()
        {
            return await this.GetByVersion(await _context.GameVersions.OrderByDescending(release => release.CreatedOn)
                .FirstOrDefaultAsync());
        }

        public async Task<IQueryable<MethodMapping>> GetByVersion(Guid versionId)
        {
            return _context.MethodMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion.Id == versionId));
        }

        public async Task<IQueryable<MethodMapping>> GetByVersion(string versionName)
        {
            return _context.MethodMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion.Name == versionName));
        }

        public async Task<IQueryable<MethodMapping>> GetByVersion(GameVersion version)
        {
            return _context.MethodMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion == version));
        }

        public async Task<IQueryable<MethodMapping>> GetByLatestMapping(string name)
        {
            return _context.MethodMappings.Where(methodMapping =>
                methodMapping.VersionedMappings.OrderByDescending(versionedMapping => versionedMapping.CreatedOn)
                    .FirstOrDefault().CommittedMappings
                    .Any(committedMapping => committedMapping.OutputMapping == name || committedMapping.InputMapping == name));
        }

        public async Task<IQueryable<MethodMapping>> GetByMappingInVersion(string name, Guid versionId)
        {
            return _context.MethodMappings.Where(mapping => mapping.VersionedMappings.Any(versionMapping =>
                versionMapping.GameVersion.Id == versionId &&
                versionMapping.CommittedMappings.Any(committedMapping => committedMapping.OutputMapping == name || committedMapping.InputMapping == name)));
        }

        public async Task<IQueryable<MethodMapping>> GetByMappingInVersion(string name, GameVersion gameVersion)
        {
            return _context.MethodMappings.Where(mapping => mapping.VersionedMappings.Any(versionMapping =>
                versionMapping.GameVersion.Id == gameVersion.Id &&
                versionMapping.CommittedMappings.Any(committedMapping => committedMapping.OutputMapping == name || committedMapping.InputMapping == name)));
        }

        public async Task<IQueryable<MethodMapping>> GetByMappingInRelease(string name, Guid releaseId)
        {
            return _context.MethodMappings.Where(mapping => mapping.VersionedMappings.Any(versionMapping =>
                versionMapping.CommittedMappings.Any(committedMapping => (committedMapping.OutputMapping == name || committedMapping.InputMapping == name) &&
                                                                         committedMapping.Releases.Select(release => release.Release.Id)
                                                                             .Contains(releaseId))));
        }

        public async Task<IQueryable<MethodMapping>> GetByMappingInRelease(string name, Release release)
        {
            return _context.MethodMappings.Where(mapping => mapping.VersionedMappings.Any(versionMapping =>
                versionMapping.CommittedMappings.Any(committedMapping => (committedMapping.OutputMapping == name || committedMapping.InputMapping == name) &&
                                                                         committedMapping.Releases
                                                                             .Any(r => r.Release == release))));
        }

        public async Task<MethodVersionedMapping> GetVersionedMapping(Guid id)
        {
            return await _context.MethodVersionedMappings.FirstOrDefaultAsync(mapping => mapping.Id == id);
        }

        public async Task<MethodProposalMappingEntry> GetProposal(Guid proposalEntry)
        {
            return await _context.MethodProposalMappingEntries.FirstOrDefaultAsync(mapping => mapping.Id == proposalEntry);
        }

        public async Task<MethodCommittedMappingEntry> GetCommittedEntry(Guid committedEntryId)
        {
            return await _context.MethodCommittedMappingEntries.FirstOrDefaultAsync(mapping => mapping.Id == committedEntryId);
        }

        public async Task<IQueryable<MethodMapping>> GetByClassInLatestVersion(Guid classId)
        {
            var latest = await _context.GameVersions.OrderByDescending(version => version.CreatedOn)
                .FirstOrDefaultAsync();
            return await this.GetByClassInVersion(classId, latest.Id);
        }

        public async Task<IQueryable<MethodMapping>> GetByClassInLatestRelease(Guid classId)
        {
            var latest = await _context.Releases.OrderByDescending(release => release.CreatedOn)
                .FirstOrDefaultAsync();
            return await this.GetByClassInRelease(classId, latest.Id);
        }

        public async Task<IQueryable<MethodMapping>> GetByClassInLatestVersion(ClassMapping classMapping)
        {
            return await this.GetByClassInLatestVersion(classMapping.Id);
        }

        public async Task<IQueryable<MethodMapping>> GetByClassInLatestRelease(ClassMapping classMapping)
        {
            return await this.GetByClassInLatestRelease(classMapping.Id);
        }

        public async Task<IQueryable<MethodMapping>> GetByClassInVersion(Guid classId, Guid versionId)
        {
            return _context.MethodMappings.Where(fieldMapping => fieldMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == versionId && versionedMapping.MemberOf.Id == classId));
        }

        public async Task<IQueryable<MethodMapping>> GetByClassInRelease(Guid classId, Guid releaseId)
        {
            return _context.MethodMappings.Where(fieldMapping => fieldMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.CommittedMappings.Any(committedMapping =>
                    committedMapping.Releases.Select(release => release.Id).Contains(releaseId)) &&
                versionedMapping.MemberOf.Id == classId));
        }

        public async Task<IQueryable<MethodMapping>> GetByClassInVersion(ClassMapping classMapping, Guid versionId)
        {
            return await this.GetByClassInVersion(classMapping.Id, versionId);
        }

        public async Task<IQueryable<MethodMapping>> GetByClassInRelease(ClassMapping classMapping, Guid releaseId)
        {
            return await this.GetByClassInRelease(classMapping.Id, releaseId);
        }

        public async Task Add(MethodMapping mapping)
        {
            if (mapping is MethodMapping methodMapping)
            {
                await _context.MethodMappings.AddAsync(methodMapping);
            }
        }

        public async Task Update(MethodMapping mapping)
        {
            if (mapping is MethodMapping methodMapping)
            {
                _context.Entry(methodMapping).State = EntityState.Modified;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddProposal(MethodProposalMappingEntry proposalEntry)
        {
            await _context.MethodProposalMappingEntries.AddAsync(proposalEntry);
        }
    }
}
