using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MjrChess.Trainer.Models;

namespace MjrChess.Trainer.Data
{
    /// <summary>
    /// Repository service for retrieving and updating TacticsPuzzles.
    /// </summary>
    public class TacticsPuzzleRepository : EFRepository<Data.Models.TacticsPuzzle, TacticsPuzzle>
    {
        public TacticsPuzzleRepository(PuzzleDbContext context, IMapper mapper, ILogger<EFRepository<Data.Models.TacticsPuzzle, TacticsPuzzle>> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Gets an IQueryable that includes puzzle history with puzzle entities.
        /// </summary>
        protected override IQueryable<Data.Models.TacticsPuzzle> DbSetWithRelatedEntities =>
            base.DbSetWithRelatedEntities.Include(p => p.History);
    }
}
