using System;
using System.Collections.Generic;
using System.Linq;

namespace puzzle_logic
{
    public class Puzzle
    {
        private static string INVALID_MOVEMENT_MESSAGE = "It's not possible to move to a invalid position";
        public PuzzlePiece[][] Rows { get; private set; }
        public int Size { get; private set; }
        private PuzzlePiece hidePiece;
        private Random random = new Random();
        private IList<int> randomPiecePositions;

        public Puzzle(int size = 3)
        {
            this.Size = size;
            FillColumnsAndRows();
        }

        public IList<MovementType> AllowedMovements()
        {
            var allowedMovements = new List<MovementType>();

            if (IsMovementAllowed(MovementType.DOWN))
            {
                allowedMovements.Add(MovementType.DOWN);
            }

            if (IsMovementAllowed(MovementType.LEFT))
            {
                allowedMovements.Add(MovementType.LEFT);
            }

            if (IsMovementAllowed(MovementType.RIGHT))
            {
                allowedMovements.Add(MovementType.RIGHT);
            }

            if (IsMovementAllowed(MovementType.UP))
            {
                allowedMovements.Add(MovementType.UP);
            }

            return allowedMovements;
        }

        public bool IsDone()
        {
            var isDone = true;
            var current = Rows.First().First().Number;
            var last = Rows.First().First().Number;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    last = current;
                    current = Rows[i][j].Number;

                    if (last > current && last != current)
                    {
                        isDone = false;
                        break;
                    }
                }
            }

            return isDone;
        }

        public void Move(MovementType movement)
        {
            var position = hidePiece.Position;
            var nextPosition = position;

            if (IsMovementAllowed(movement))
            {
                throw new InvalidOperationException(INVALID_MOVEMENT_MESSAGE);
            }

            switch (movement)
            {
                case MovementType.DOWN:
                    nextPosition.Row = nextPosition.Row + 1;
                    break;
                case MovementType.LEFT:
                    nextPosition.Column = nextPosition.Column - 1;
                    break;
                case MovementType.RIGHT:
                    nextPosition.Column = nextPosition.Column + 1;
                    break;
                case MovementType.UP:
                    nextPosition.Row = nextPosition.Row - 1;
                    break;
            }

            var pieceToMove = Rows[nextPosition.Row][nextPosition.Column];

            Rows[position.Row][position.Column] = pieceToMove;
            Rows[nextPosition.Row][nextPosition.Column] = hidePiece;
            pieceToMove.Position = position;
            hidePiece.Position = nextPosition;
        }

        public void Shuffle()
        {
            var currentRows = Rows.Clone();
            var nextRows = new PuzzlePiece[Size][];

            randomPiecePositions = new List<int>();

            for (int i = 0; i < Size; i++)
            {
                nextRows[i] = new PuzzlePiece[Size];
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var currentPiece = Rows[i][j];
                    decimal nextPosition = this.GetNextRandomPosition();
                    var nextColumn = (int)Math.Floor(nextPosition % Size);
                    var nextRow = (int)Math.Floor(nextPosition / Size);

                    nextRows[nextRow][nextColumn] = currentPiece;
                    currentPiece.Position = new PiecePosition(nextRow, nextColumn);
                }
            }

            Rows = nextRows;
        }

        private void FillColumnsAndRows()
        {
            var rows = new PuzzlePiece[Size][];

            int currentIndex = 0;

            for (int i = 0; i < Size; i++)
            {
                var column = new PuzzlePiece[Size];

                for (int j = 0; j < Size; j++)
                {
                    var position = new PiecePosition(i, j);
                    var isHide = IsHidePiece(i, j);
                    var piece = new PuzzlePiece(currentIndex++, position, isHide);

                    if (isHide)
                    {
                        hidePiece = piece;
                    }

                    column[j] = piece;
                }

                rows[i] = column;
            }

            Rows = rows;
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

        private bool IsHidePiece(int i, int j) => i == 0 && j == 0;

        private bool IsMovementAllowed(MovementType movement)
        {
            var isMovementAllowed = false;
            var position = hidePiece.Position;

            switch (movement)
            {
                case MovementType.DOWN:
                    isMovementAllowed = (position.Row + 1 <= (Size - 1));
                    break;
                case MovementType.LEFT:
                    isMovementAllowed = (position.Column >= 0);
                    break;
                case MovementType.RIGHT:
                    isMovementAllowed = (position.Column + 1 <= (Size - 1));
                    break;
                case MovementType.UP:
                    isMovementAllowed = (position.Row >= 0);
                    break;
                default:
                    throw new InvalidOperationException(INVALID_MOVEMENT_MESSAGE);
            }

            return isMovementAllowed;
        }
    }
}
