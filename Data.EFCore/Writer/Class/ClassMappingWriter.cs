using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Writers.Class;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Class
{
    public class ClassMappingWriter
        : IClassMappingWriter
    {

        private readonly MCPContext _context;

        public ClassMappingWriter(MCPContext context)
        {
            _context = context;
        }

        public async Task<ClassMapping> GetById(Guid id)
        {
            return await _context.ClassMappings.IncludeForDefaultClassMappingLogic()
                .FirstOrDefaultAsync(mapping => mapping.Id == id);
        }

        public async Task<ClassMapping> GetByLatestMapping(string name)
        {
            var (nameComponents, packageName, classNameComponents) = ProcessClassName(name);

            var versionedMapping =
                await _context.ClassVersionedMappings.OrderByDescending(mapping => mapping.CreatedOn)
                    .Include(mapping => mapping.CommittedMappings)
                    .FirstOrDefaultAsync();

            return await GetMappingFromNameAndVersionedMapping(classNameComponents, packageName, versionedMapping);
        }

        public async Task<ClassMapping> GetByMappingInVersion(string name, Guid versionId)
        {
            var (nameComponents, packageName, classNameComponents) = ProcessClassName(name);

            var versionedMapping =
                await _context.ClassVersionedMappings.OrderByDescending(mapping => mapping.CreatedOn)
                    .Include(mapping => mapping.CommittedMappings)
                    .FirstOrDefaultAsync(mapping => mapping.GameVersion.Id == versionId);

            return await GetMappingFromNameAndVersionedMapping(classNameComponents, packageName, versionedMapping);
        }

        public async Task<ClassMapping> GetByMappingInVersion(string name, GameVersion gameVersion)
        {
            return await this.GetByMappingInVersion(name, gameVersion.Id);
        }

        public async Task<ClassMapping> GetByMappingInRelease(string name, Guid releaseId)
        {
            var (nameComponents, packageName, classNameComponents) = ProcessClassName(name);

            var release = await _context.Releases.FirstOrDefaultAsync(r => r.Id == releaseId);

            var versionedMapping =
                await _context.ClassVersionedMappings
                    .Include(mapping => mapping.CommittedMappings)
                    .FirstOrDefaultAsync(mapping =>
                    mapping.GameVersion.Id == release.GameVersion.Id);

            if (versionedMapping == null)
                return null;

            return await GetMappingFromNameAndVersionedMapping(classNameComponents, packageName, versionedMapping);
        }

        public async Task<ClassMapping> GetByMappingInRelease(string name, Release release)
        {
            return await this.GetByMappingInVersion(name, release.Id);
        }

        public async Task<ClassVersionedMapping> GetVersionedMapping(Guid id)
        {
            return await _context.ClassVersionedMappings.FirstOrDefaultAsync(versionedMappings =>
                versionedMappings.Id == id);
        }

        public async Task<ClassProposalMappingEntry> GetProposal(Guid proposalEntry)
        {
            return await _context.ClassProposalMappingEntries.FirstOrDefaultAsync(proposalMappingEntry =>
                proposalMappingEntry.Id == proposalEntry);
        }

        public async Task<ClassCommittedMappingEntry> GetCommittedEntry(Guid committedEntryId)
        {
            return await _context.ClassCommittedMappingEntries.FirstOrDefaultAsync(committedMappingEntry =>
                committedMappingEntry.Id == committedEntryId);
        }

        public async Task<IQueryable<ClassMapping>> AsMappingQueryable()
        {
            return await Task.FromResult(_context.ClassMappings.IncludeForDefaultClassMappingLogic()
            );
        }

        public async Task<IQueryable<ClassMapping>> GetByLatestRelease()
        {
            return await this.GetByRelease(await _context.Releases.OrderByDescending(release => release.CreatedOn)
                .FirstOrDefaultAsync());
        }

        public async Task<IQueryable<ClassMapping>> GetByRelease(Guid releaseId)
        {
            return await this.GetByRelease(await _context.Releases.FirstOrDefaultAsync(release => release.Id == releaseId));
        }

        public async Task<IQueryable<ClassMapping>> GetByRelease(string releaseName)
        {
            return await this.GetByRelease(await _context.Releases.FirstOrDefaultAsync(release => release.Name == releaseName));
        }

        public async Task<IQueryable<ClassMapping>> GetByRelease(Release release)
        {
            if (release == null)
                return null;

            return _context.ClassMappings.Where(cls => cls.VersionedMappings.Any(vcls =>
                vcls.CommittedMappings.Any(cmvcls => cmvcls.Releases.Any(r => r.Release == release)))).IncludeForDefaultClassMappingLogic();
        }

        public async Task<IQueryable<ClassMapping>> GetByLatestVersion()
        {
            return await this.GetByVersion(await _context.GameVersions.OrderByDescending(release => release.CreatedOn)
                .FirstOrDefaultAsync());
        }

        public async Task<IQueryable<ClassMapping>> GetByVersion(Guid versionId)
        {
            return _context.ClassMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion.Id == versionId)).IncludeForDefaultClassMappingLogic();
        }

        public async Task<IQueryable<ClassMapping>> GetByVersion(string versionName)
        {
            return _context.ClassMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion.Name == versionName)).IncludeForDefaultClassMappingLogic();
        }

        public async Task<IQueryable<ClassMapping>> GetByVersion(GameVersion version)
        {
            return _context.ClassMappings.Where(mapping =>
                mapping.VersionedMappings.Any(versionedMapping => versionedMapping.GameVersion == version)).IncludeForDefaultClassMappingLogic();
        }

        public async Task Add(ClassMapping mapping)
        {
            if (mapping is ClassMapping classMapping)
            {
                this._context.ClassMappings.Add(classMapping);
            }
        }

        public async Task Update(ClassMapping mapping)
        {
            if (mapping is ClassMapping classMapping)
            {
                this._context.Entry(mapping).State = EntityState.Modified;
            }
        }

        public async Task SaveChanges()
        {
            await this._context.SaveChangesAsync();
        }

        private static (string[] nameComponents, string packageName, string[] classNameComponents) ProcessClassName(string name)
        {
            var nameComponents = name.Split('.');
            var packageName = "";
            if (nameComponents.Length > 1)
            {
                packageName = nameComponents.Take(nameComponents.Length - 1).Aggregate(((s, s1) => $"{s}.{s1}"));
            }

            var classNameComponents = nameComponents.Last().Split('$');
            return (nameComponents, packageName, classNameComponents);
        }

        private async Task<ClassMapping> GetMappingFromNameAndVersionedMapping(IEnumerable<string> classNameComponents,
            string packageName,
            ClassVersionedMapping versionedMapping)
        {
            Guid? parentClassId = null;
            ClassCommittedMappingEntry targetMapping = null;

            var nameComponentsList = classNameComponents.ToList();
            foreach (var classNameComponent in nameComponentsList)
            {
                targetMapping = await _context.ClassCommittedMappingEntries
                    .Include(mapping => mapping.VersionedMapping)
                    .FirstOrDefaultAsync(mapping =>
                    mapping.VersionedMapping.Package == packageName &&
                    mapping.VersionedMapping.Outer.Id == parentClassId &&
                    mapping.OutputMapping == classNameComponent &&
                    versionedMapping.CommittedMappings.Select(committedMapping => committedMapping.Id).Contains(mapping.Id));

                if (targetMapping == null)
                    return null;

                parentClassId = targetMapping.VersionedMapping.Id;
            }

            if (targetMapping == null)
            {
                parentClassId = null;
                foreach (var classNameComponent in nameComponentsList)
                {
                    targetMapping = await _context.ClassCommittedMappingEntries
                        .Include(mapping => mapping.VersionedMapping)
                        .FirstOrDefaultAsync(mapping =>
                        mapping.VersionedMapping.Package == packageName &&
                        mapping.VersionedMapping.Outer.Id == parentClassId &&
                        mapping.InputMapping == classNameComponent &&
                        versionedMapping.CommittedMappings.Select(committedMapping => committedMapping.Id).Contains(mapping.Id));

                    if (targetMapping == null)
                        return null;

                    parentClassId = targetMapping.Id;
                }
            }

            if (targetMapping == null)
                return null;

            return await _context.ClassMappings.IncludeForDefaultClassMappingLogic()
                .FirstOrDefaultAsync(mapping =>
                mapping.VersionedMappings.Select(v => v.Id).Contains(versionedMapping.Id));
        }

        public async Task AddProposal(ClassProposalMappingEntry proposalEntry)
        {
            await _context.ClassProposalMappingEntries.AddAsync(proposalEntry);
        }
    }
}
