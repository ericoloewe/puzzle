using System.Collections.Generic;
using System.Threading.Tasks;

namespace puzzle_logic
{
    public interface IPuzzleBuilder
    {
        IPuzzle Puzzle { get; }
        Task<IList<IPuzzle>> Build(PuzzleEvents events);
    }
}