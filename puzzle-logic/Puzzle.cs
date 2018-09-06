using System;
using System.Collections.Generic;
using System.Linq;

namespace puzzle_logic
{
    public class Puzzle
    {
        public PuzzlePiece[][] Columns { get; private set; }
        public int Size { get; private set; }
        private PuzzlePiece hidePiece;
        private Random random = new Random();
        private IList<int> randomPiecePositions;

        public Puzzle(int size = 3)
        {
            this.Size = size;
            FillColumnsAndRows();
        }

        public bool IsDone()
        {
            var isDone = true;
            var current = Columns.First().First().Number;
            var last = Columns.First().First().Number;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    last = current;
                    current = Columns[i][j].Number;

                    if (last > current && last != current)
                    {
                        isDone = false;
                        break;
                    }
                }
            }

            return isDone;
        }

        public void Shuffle()
        {
            var currentColumns = Columns.Clone();
            var nextColumns = new PuzzlePiece[Size][];

            randomPiecePositions = new List<int>();

            for (int i = 0; i < Size; i++)
            {
                nextColumns[i] = new PuzzlePiece[Size];
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var currentPiece = Columns[i][j];
                    decimal nextPosition = this.GetNextRandomPosition();
                    var nextColumn = (int)Math.Floor(nextPosition % Size);
                    var nextRow = (int)Math.Floor(nextPosition / Size);

                    nextColumns[nextColumn][nextRow] = currentPiece;
                }
            }

            Columns = nextColumns;
        }

        public void SlideDown()
        {
            var position = GetPiecePosition(hidePiece);
            var nextRow = position.Row + 1;

            if (nextRow > (Size - 1))
            {
                throw new InvalidOperationException("It's not possible to move to a invalid position");
            }

            Columns[position.Column][position.Row] = Columns[nextRow][position.Row];
            Columns[nextRow][position.Row] = hidePiece;
        }

        public void SlideLeft()
        {
            var position = GetPiecePosition(hidePiece);
            var nextColumn = position.Column - 1;

            if (nextColumn < 0)
            {
                throw new InvalidOperationException("It's not possible to move to a invalid position");
            }

            Columns[position.Column][position.Row] = Columns[nextColumn][position.Row];
            Columns[nextColumn][position.Row] = hidePiece;
        }

        public void SlideRight()
        {
            var position = GetPiecePosition(hidePiece);
            var nextColumn = position.Column + 1;

            if (nextColumn > (Size - 1))
            {
                throw new InvalidOperationException("It's not possible to move to a invalid position");
            }

            Columns[position.Column][position.Row] = Columns[nextColumn][position.Row];
            Columns[nextColumn][position.Row] = hidePiece;
        }

        public void SlideUp()
        {
            var position = GetPiecePosition(hidePiece);
            var nextRow = position.Row - 1;

            if (nextRow < 0)
            {
                throw new InvalidOperationException("It's not possible to move to a invalid position");
            }

            Columns[position.Column][position.Row] = Columns[nextRow][position.Row];
            Columns[nextRow][position.Row] = hidePiece;
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

        private int GetNextRandomPosition()
        {
            var nextPosition = random.Next(Size * Size);
            var isNewPosition = !randomPiecePositions.Contains(nextPosition);

            while (randomPiecePositions.Contains(nextPosition))
            {
                nextPosition = random.Next(Size * Size);
            }

            randomPiecePositions.Add(nextPosition);

            return nextPosition;
        }

        private PiecePosition GetPiecePosition(PuzzlePiece hidePiece)
        {
            throw new NotImplementedException();
        }

        private bool IsHidePiece(int i, int j)
        {
            return i == 0 && j == 0;
        }

        private class PiecePosition
        {
            public int Column { get; set; }
            public int Row { get; set; }
        }
    }
}
