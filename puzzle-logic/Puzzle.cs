using System;
using System.Collections.Generic;

namespace puzzle_logic
{
    public class Puzzle
    {
        public IList<IList<PuzzlePiece>> Columns { get; private set; }
        public int Size { get; private set; }

        public Puzzle(int size = 3)
        {
            this.Size = size;
            fillColumnsAndRows();
        }

        private void fillColumnsAndRows()
        {
            Columns = new List<IList<PuzzlePiece>>();

            int currentIndex = 1;

            for (int i = 0; i < Size; i++)
            {
                var row = new List<PuzzlePiece>();

                for (int j = 0; j < Size; j++)
                {
                    var isHide = IsHidePiece(i, j);

                    row.Add(new PuzzlePiece(currentIndex++, i, j, isHide));
                }

                Columns.Add(row);
            }
        }

        public void Shuffle()
        {
            throw new NotImplementedException();
        }

        public void Build()
        {
            throw new NotImplementedException();
        }

        private bool IsHidePiece(int i, int j)
        {
            return i == 0 && j == 0;
        }
    }
}
