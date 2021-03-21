using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Entities.Users;
using RailWiki.Shared.Models.Photos;

namespace RailWiki.Shared.Services.Photos
{
    public interface ICommentService
    {
        Task<IEnumerable<GetCommentModel>> GetCommentsAsync(string entityType, int entityId);

        Task<GetCommentModel>  CreateAsync(CreateCommentModel model, User user);
    }

    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(IRepository<Comment> commentRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCommentModel>> GetCommentsAsync(string entityType, int entityId)
        {
            var comments = await _commentRepository.TableNoTracking
                .Include(x => x.User)
                .Where(x => x.EntityType == entityType
                    && x.EntityId == entityId)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<GetCommentModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return comments;
        }

        public async Task<GetCommentModel> CreateAsync(CreateCommentModel model, User user)
        {
            var comment = new Comment
            {
                EntityType = model.EntityType,
                EntityId = model.EntityId,
                CreatedOn = DateTime.UtcNow,
                UserId = user.Id,
                CommentText = model.CommentText,
            };

            await _commentRepository.CreateAsync(comment);

            // Set the user for the API to return
            // For some reason, setting it before saving causes EF to think
            // it's a new user (seems like maybe it's detached from context?)
            comment.User = user;

            var result = _mapper.Map<GetCommentModel>(comment);
            return result;
        }
    }
}
