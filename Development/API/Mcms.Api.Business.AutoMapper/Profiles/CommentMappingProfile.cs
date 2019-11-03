using System;
using System.Linq;
using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Comment;
using Mcms.Api.Data.Poco.Models.Comments;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class CommentMappingProfile
        : Profile
    {
        public CommentMappingProfile()
        {
            SetupCommentToDtoMapping();
            SetupDtoToCommentMapping();
        }

        private void SetupCommentToDtoMapping()
        {
            var commentToDtoMapping = CreateMap<Comment, CommentDto>();
            commentToDtoMapping.ForAllMembers(d => d.Ignore());
            commentToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            commentToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            commentToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            commentToDtoMapping.ForMember(d => d.Content,
                opts => opts.MapFrom(d => d.Content));
            commentToDtoMapping.ForMember(d => d.Reactions,
                opts => opts.MapFrom(d => d.Reactions.Select(r => r.Id).ToHashSet()));
            commentToDtoMapping.ForMember(d => d.HasBeenEdited,
                opts => opts.MapFrom(d => d.HasBeenEdited));
            commentToDtoMapping.ForMember(d => d.IsDeleted,
                opts => opts.MapFrom(d => d.IsDeleted));
            commentToDtoMapping.ForMember(d => d.DeletedBy,
                opts => opts.MapFrom(d => d.DeletedBy));
            commentToDtoMapping.ForMember(d => d.DeletedOn,
                opts => opts.MapFrom(d => d.DeletedOn));
            commentToDtoMapping.ForMember(d => d.ProposedMapping,
                opts => opts.MapFrom(d => d.ProposedMapping != null ? (Guid?) d.ProposedMapping.Id : (Guid?) null));
            commentToDtoMapping.ForMember(d => d.Release,
                opts => opts.MapFrom(d => d.Release != null ? (Guid?) d.Release.Id : (Guid?) null));
            commentToDtoMapping.ForMember(d => d.Parent,
                opts => opts.MapFrom(d => d.Parent != null ? (Guid?) d.Parent.Id : (Guid?) null));
            commentToDtoMapping.ForMember(d => d.Children,
                opts => opts.MapFrom(d => d.Children.Select(c => c.Id).ToHashSet()));
        }

        private void SetupDtoToCommentMapping()
        {
            var dtoToCommentMapping = CreateMap<CommentDto, Comment>();
            dtoToCommentMapping.ForAllMembers(d => d.Ignore());
            dtoToCommentMapping.ForMember(d => d.Content,
                opts => opts.MapFrom(d => d.Content));
            dtoToCommentMapping.ForMember(d => d.Reactions,
                opts => opts.MapFrom(d => d.Reactions.Select(id => new CommentReaction {Id = id}).ToList()));
            dtoToCommentMapping.ForMember(d => d.IsDeleted,
                opts => opts.MapFrom(d => d.IsDeleted));
            dtoToCommentMapping.ForMember(d => d.ProposedMapping,
                opts => opts.MapFrom(d => d.ProposedMapping.HasValue ? new ProposedMapping {Id = d.ProposedMapping.Value} : null));
            dtoToCommentMapping.ForMember(d => d.Release,
                opts => opts.MapFrom(d => d.Release.HasValue ? new Release {Id = d.Release.Value} : null));
            dtoToCommentMapping.ForMember(d => d.Parent,
                opts => opts.MapFrom(d => d.Parent.HasValue ? new Comment {Id = d.Parent.Value} : null));
            dtoToCommentMapping.ForMember(d => d.Children,
                opts => opts.MapFrom(d => d.Children.Select(c => new Comment {Id = c}).ToList()));
            dtoToCommentMapping.BeforeMap((dto, comment) =>
            {
                if (!string.IsNullOrEmpty(comment.Content))
                {
                    if (comment.Content != dto.Content)
                    {
                        comment.HasBeenEdited = true;
                    }
                }
            });
        }
    }
}
