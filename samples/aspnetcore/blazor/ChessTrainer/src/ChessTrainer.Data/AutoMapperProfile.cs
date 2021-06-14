using System;
using AutoMapper;

namespace MjrChess.Trainer.Data
{
    /// <summary>
    /// AutoMapper profile for mapping back and forth between data entities and domain entities.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Gets the DB context used for retrieving data entities.
        /// </summary>
        public PuzzleDbContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
        /// </summary>
        /// <param name="context">The DB context to use for retrieving entities from the database.</param>
        public AutoMapperProfile(PuzzleDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            // Map data models to domain models
            CreateMap<Data.Models.PuzzleHistory, Trainer.Models.PuzzleHistory>();
            CreateMap<Data.Models.TacticsPuzzle, Trainer.Models.TacticsPuzzle>();

            // Map domain models to data models. Uses entities from the DB context if they exist (so that already
            // tracked entities are used, if possible) and creates new entities if they don't.
            CreateMap<Trainer.Models.PuzzleHistory, Data.Models.PuzzleHistory>()
                .ConstructUsing((p, resolutonContext) => FindDbObjectOrCreate(p, new Data.Models.PuzzleHistory()));
            CreateMap<Trainer.Models.TacticsPuzzle, Data.Models.TacticsPuzzle>()
                .ConstructUsing((p, resolutonContext) => FindDbObjectOrCreate(p, new Data.Models.TacticsPuzzle(p.Position, p.SetupMovedFrom, p.SetupMovedTo, p.SetupPiecePromotedTo, p.MovedFrom, p.MovedTo, p.PiecePromotedTo, p.IncorrectMovedFrom, p.IncorrectMovedTo, p.IncorrectPiecePromotedTo)));
        }

        /// <summary>
        /// Finds EF entities by ID if possible (so that tracked entities are used) or creates new entities if none exist with the given ID.
        /// </summary>
        /// <typeparam name="TDtoType">The domain type to convert to a data type.</typeparam>
        /// <typeparam name="TDataType">The data type to be returned.</typeparam>
        /// <param name="dto">The object to be found as a TDataType.</param>
        /// <param name="defaultObject">A new instance of TDataType to use if none is found in the DbContext with the right ID.</param>
        /// <returns>A TDataType from the DbContext with an ID matching dto's, or defaultObject if no match is found.</returns>
        private TDataType FindDbObjectOrCreate<TDtoType, TDataType>(TDtoType dto, TDataType defaultObject)
            where TDtoType : Trainer.Models.IEntity
            where TDataType : Data.Models.EntityBase
        {
            TDataType? ret = null;
            if (dto.Id != 0)
            {
                ret = Context.Find<TDataType>(dto.Id);
            }

            return ret ?? defaultObject;
        }
    }
}
