using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Field;
using Data.Core.Writers.Field;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Field
{
    public class FieldMappingWriter
        : IFieldMappingWriter
    {
        private readonly MCPContext _context;

        public FieldMappingWriter(MCPContext context)
        {
            _context = context;
        }

        public async Task<FieldMapping> GetById(Guid id)
        {
            return await _context.FieldMappings.FirstOrDefaultAsync(mapping => mapping.Id == id);
        }

        public async Task<IQueryable<FieldMapping>> GetByLatestMapping(string name)
        {
            return _context.FieldMappings.Where(fieldMapping =>
                fieldMapping.VersionedMappings.OrderByDescending(versionedMapping => versionedMapping.CreatedOn)
                    .FirstOrDefault().CommittedMappings
                    .Any(committedMapping => committedMapping.OutputMapping == name || committedMapping.InputMapping == name));
        }

        public async Task<IQueryable<FieldMapping>> GetByMappingInVersion(string name, Guid versionId)
        {
            return _context.FieldMappings.Where(mapping => mapping.VersionedMappings.Any(versionMapping =>
                versionMapping.GameVersion.Id == versionId &&
                versionMapping.CommittedMappings.Any(committedMapping => committedMapping.OutputMapping == name || committedMapping.InputMapping == name)));
        }

        public async Task<IQueryable<FieldMapping>> GetByMappingInVersion(string name, GameVersion gameVersion)
        {
            return _context.FieldMappings.Where(mapping => mapping.VersionedMappings.Any(versionMapping =>
                versionMapping.GameVersion.Id == gameVersion.Id &&
                versionMapping.CommittedMappings.Any(committedMapping => committedMapping.OutputMapping == name || committedMapping.InputMapping == name)));
        }

        public async Task<IQueryable<FieldMapping>> GetByMappingInRelease(string name, Guid releaseId)
        {
            return _context.FieldMappings.Where(mapping => mapping.VersionedMappings.Any(versionMapping =>
                versionMapping.CommittedMappings.Any(committedMapping => (committedMapping.OutputMapping == name || committedMapping.InputMapping == name) &&
                                                                         committedMapping.Releases.Select(release => release.Release.Id)
                                                                             .Contains(releaseId))));
        }

        public async Task<IQueryable<FieldMapping>> GetByMappingInRelease(string name, Release release)
        {
            return _context.FieldMappings.Where(mapping => mapping.VersionedMappings.Any(versionMapping =>
                versionMapping.CommittedMappings.Any(committedMapping => (committedMapping.OutputMapping == name || committedMapping.InputMapping == name) &&
                                                                         committedMapping.Releases
                                                                             .Any(r => r.Release == release))));
        }

        public async Task<FieldVersionedMapping> GetVersionedMapping(Guid id)
        {
            return await _context.FieldVersionedMappings.FirstOrDefaultAsync(mapping => mapping.Id == id);
        }

        public async Task<FieldProposalMappingEntry> GetProposal(Guid proposalEntry)
        {
            return await _context.FieldProposalMappingEntries.FirstOrDefaultAsync(mapping => mapping.Id == proposalEntry);
        }

        public async Task<FieldCommittedMappingEntry> GetCommittedEntry(Guid committedEntryId)
        {
            return await _context.FieldCommittedMappingEntries.FirstOrDefaultAsync(mapping => mapping.Id == committedEntryId);
        }

        public async Task<IQueryable<FieldMapping>> AsMappingQueryable()
        {
            return _context.FieldMappings;
        }

        public async Task<IQueryable<FieldMapping>> GetByLatestRelease()
        {
            var latest = await _context.Releases.OrderByDescending(release => release.CreatedOn).FirstOrDefaultAsync();
            return await this.GetByRelease(latest.Id);
        }

        public async Task<IQueryable<FieldMapping>> GetByRelease(Guid releaseId)
        {
            return _context.FieldMappings.Where(mapping => mapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.CommittedMappings.Any(committedMapping =>
                    committedMapping.Releases.Select(release => release.Id).Contains(releaseId))));
        }

        public async Task<IQueryable<FieldMapping>> GetByRelease(string releaseName)
        {
            var release = await _context.Releases.FirstOrDefaultAsync(r => r.Name == releaseName);
            return await this.GetByRelease(release.Id);
        }

        public async Task<IQueryable<FieldMapping>> GetByRelease(Release release)
        {
            return await this.GetByRelease(release.Id);
        }

        public async Task<IQueryable<FieldMapping>> GetByLatestVersion()
        {
            return await this.GetByVersion(await _context.GameVersions.OrderByDescending(release => release.CreatedOn)
                .FirstOrDefaultAsync());
        }

        public async Task<IQueryable<FieldMapping>> GetByVersion(Guid versionId)
        {
            return _context.FieldMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion.Id == versionId));
        }

        public async Task<IQueryable<FieldMapping>> GetByVersion(string versionName)
        {
            return _context.FieldMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion.Name == versionName));
        }

        public async Task<IQueryable<FieldMapping>> GetByVersion(GameVersion version)
        {
            return _context.FieldMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion == version));
        }

        public async Task<IQueryable<FieldMapping>> GetByClassInLatestVersion(Guid classId)
        {
            var latest = await _context.GameVersions.OrderByDescending(version => version.CreatedOn)
                .FirstOrDefaultAsync();
            return await this.GetByClassInVersion(classId, latest.Id);
        }

        public async Task<IQueryable<FieldMapping>> GetByClassInLatestRelease(Guid classId)
        {
            var latest = await _context.Releases.OrderByDescending(release => release.CreatedOn)
                .FirstOrDefaultAsync();
            return await this.GetByClassInRelease(classId, latest.Id);
        }

        public async Task<IQueryable<FieldMapping>> GetByClassInLatestVersion(ClassMapping classMapping)
        {
            return await this.GetByClassInLatestVersion(classMapping.Id);
        }

        public async Task<IQueryable<FieldMapping>> GetByClassInLatestRelease(ClassMapping classMapping)
        {
            return await this.GetByClassInLatestRelease(classMapping.Id);
        }

        public async Task<IQueryable<FieldMapping>> GetByClassInVersion(Guid classId, Guid versionId)
        {
            return _context.FieldMappings.Where(fieldMapping => fieldMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == versionId && versionedMapping.MemberOf.Id == classId));
        }

        public async Task<IQueryable<FieldMapping>> GetByClassInRelease(Guid classId, Guid releaseId)
        {
            return _context.FieldMappings.Where(fieldMapping => fieldMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.CommittedMappings.Any(committedMapping =>
                    committedMapping.Releases.Select(release => release.Id).Contains(releaseId)) &&
                versionedMapping.MemberOf.Id == classId));
        }

        public async Task<IQueryable<FieldMapping>> GetByClassInVersion(ClassMapping classMapping, Guid versionId)
        {
            return await this.GetByClassInVersion(classMapping.Id, versionId);
        }

        public async Task<IQueryable<FieldMapping>> GetByClassInRelease(ClassMapping classMapping, Guid releaseId)
        {
            return await this.GetByClassInRelease(classMapping.Id, releaseId);
        }

        public async Task Add(FieldMapping mapping)
        {
            if (mapping is FieldMapping fieldMapping)
            {
                await _context.FieldMappings.AddAsync(fieldMapping);
            }
        }

        public async Task Update(FieldMapping mapping)
        {
            if (mapping is FieldMapping fieldMapping)
            {
                _context.Entry(fieldMapping).State = EntityState.Modified;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddProposal(FieldProposalMappingEntry proposalEntry)
        {
            await _context.FieldProposalMappingEntries.AddAsync(proposalEntry);
        }
    }
}
