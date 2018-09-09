using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace puzzle_logic
{
    public class Puzzle : ICloneable
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

            if (!IsMovementAllowed(movement))
            {
                throw new InvalidOperationException(INVALID_MOVEMENT_MESSAGE);
            }

            var nextPosition = GetNextPositionByMovement(movement);
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

            // for (int i = 0; i < Size; i++)
            // {
            //     for (int j = 0; j < Size; j++)
            //     {
            //         var currentPiece = Rows[i][j];
            //         decimal nextPosition = this.GetNextRandomPosition();
            //         var nextColumn = (int)Math.Floor(nextPosition % Size);
            //         var nextRow = (int)Math.Floor(nextPosition / Size);

            //         nextRows[nextRow][nextColumn] = currentPiece;
            //         currentPiece.Position = new PiecePosition(nextRow, nextColumn);
            //     }
            // }

            // [[3,2,5],[0,4,1],[6,7,8]]
            nextRows[0][0] = Rows[1][0];
            nextRows[0][1] = Rows[0][2];
            nextRows[0][2] = Rows[1][2];
            nextRows[1][0] = Rows[0][0];
            nextRows[1][1] = Rows[1][2];
            nextRows[1][2] = Rows[0][1];
            nextRows[2][0] = Rows[2][0];
            nextRows[2][1] = Rows[2][1];
            nextRows[2][2] = Rows[2][2];

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

        private PiecePosition GetNextPositionByMovement(MovementType movement)
        {
            var position = hidePiece.Position;
            var nextPosition = (PiecePosition)position.Clone();

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

            return nextPosition;
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
            var nextPosition = GetNextPositionByMovement(movement);

            switch (movement)
            {
                case MovementType.DOWN:
                    isMovementAllowed = (nextPosition.Row < Size);
                    break;
                case MovementType.LEFT:
                    isMovementAllowed = (nextPosition.Column >= 0);
                    break;
                case MovementType.RIGHT:
                    isMovementAllowed = (nextPosition.Column < Size);
                    break;
                case MovementType.UP:
                    isMovementAllowed = (nextPosition.Row >= 0);
                    break;
                default:
                    throw new InvalidOperationException(INVALID_MOVEMENT_MESSAGE);
            }

            return isMovementAllowed;
        }

        public object Clone()
        {
            var newPuzzle = (Puzzle)this.MemberwiseClone();
            var newPuzzleRows = (PuzzlePiece[][])newPuzzle.Rows.Clone();

            for (int i = 0; i < Size; i++)
            {
                newPuzzleRows[i] = (PuzzlePiece[])newPuzzle.Rows[i].Clone();
            }

            newPuzzle.Rows = newPuzzleRows;
            newPuzzle.hidePiece = (PuzzlePiece)newPuzzle.hidePiece.Clone();

            return newPuzzle;
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            for (int i = 0; i < Rows.Length; i++)
            {
                for (int j = 0; j < Rows[i].Length; j++)
                {
                    str.Append($"{Rows[i][j].Number},");
                }

                str.Append(";");
            }

            return str.ToString();
        }
    }
}
