using System.Collections.Generic;
using System.Threading.Tasks;

namespace puzzle_logic
{
    public interface IPuzzleBuilder
    {
        Puzzle Puzzle { get; }
        Task<IList<Puzzle>> Build(PuzzleEvents events);
    }
}