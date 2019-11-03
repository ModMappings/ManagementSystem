using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Comment;
using Mcms.Api.Data.Poco.Models.Comments;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class CommentReactionMappingProfile
        : Profile
    {
        public CommentReactionMappingProfile()
        {
            SetupCommentReactionToDtoMapping();
            SetupDtoToCommentReactionMapping();
        }

        private void SetupCommentReactionToDtoMapping()
        {
            var commentReactionToDtoMapping = CreateMap<CommentReaction, CommentReactionDto>();
            commentReactionToDtoMapping.ForAllMembers(d => d.Ignore());
            commentReactionToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            commentReactionToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            commentReactionToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            commentReactionToDtoMapping.ForMember(d => d.Type,
                opts => opts.MapFrom(d => d.Type));
            commentReactionToDtoMapping.ForMember(d => d.Comment,
                opts => opts.MapFrom(d => d.Comment));
        }

        private void SetupDtoToCommentReactionMapping()
        {
            var dtoToCommentReactionMapping = CreateMap<CommentReactionDto, CommentReaction>();
            dtoToCommentReactionMapping.ForAllMembers(d => d.Ignore());
            dtoToCommentReactionMapping.ForMember(d => d.Type,
                opts => opts.MapFrom(d => d.Type));
            dtoToCommentReactionMapping.ForMember(d => d.Comment,
                opts => opts.MapFrom(d => d.Comment));
        }
    }
}
