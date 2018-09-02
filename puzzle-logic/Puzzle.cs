using System;
using System.Collections.Generic;

namespace puzzle_logic
{
    public class Puzzle
    {
        public PuzzlePiece[][] Columns { get; private set; }
        public int Size { get; private set; }
        private PuzzlePiece hidePiece;
        private Random random;
        private IList<int> randomPiecePositions;

        public Puzzle(int size = 3)
        {
            this.Size = size;
            FillColumnsAndRows();
        }

        public void Shuffle()
        {
            var currentColumns = Columns.Clone();
            var nextColumns = new PuzzlePiece[Size][];

            randomPiecePositions = new List<int>();

            for (int i = 0; i < Columns.Length; i++)
            {
                nextColumns[i] = new PuzzlePiece[Size];

                for (int j = 0; j < Columns[i].Length; j++)
                {
                    var currentPiece = Columns[i][j];
                    var nextPosition = this.GetNextRandomPosition();
                    var nextColumn = nextPosition % Size;
                    var nextRow = nextPosition / Size;

                    nextColumns[nextColumn][nextRow] = currentPiece;
                }
            }

            Columns = nextColumns;
        }

        private int GetNextRandomPosition()
        {
            var nextPosition = random.Next(Size * Size);
            var isNewPosition = randomPiecePositions.Contains(nextPosition);

            if (isNewPosition)
            {
                randomPiecePositions.Add(nextPosition);
            }
            else
            {
                while (randomPiecePositions.Contains(nextPosition))
                {
                    nextPosition = random.Next(Size * Size);
                }
            }

            return nextPosition;
        }

        public async void Build()
        {
            throw new NotImplementedException();
        }

        private void FillColumnsAndRows()
        {
            var columns = new PuzzlePiece[Size][];

            int currentIndex = 0;

            for (int i = 0; i < Size; i++)
            {
                var row = new PuzzlePiece[Size];

                for (int j = 0; j < Size; j++)
                {
                    var isHide = IsHidePiece(i, j);
                    var piece = new PuzzlePiece(currentIndex++, isHide);

                    if (isHide)
                    {
                        hidePiece = piece;
                    }

                    row[j] = piece;
                }

                columns[i] = row;
            }

            Columns = columns;
        }

        private bool IsHidePiece(int i, int j)
        {
            return i == 0 && j == 0;
        }
    }
}
