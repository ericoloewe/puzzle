using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace puzzle_logic
{
    public class Puzzle : IPuzzle
    {
        private static string INVALID_MOVEMENT_MESSAGE = "It's not possible to move to a invalid position";
        public int Size { get; private set; }
        public PuzzlePiece[][] Rows { get; private set; }
        private IList<int> randomPiecePositions;
        private PuzzlePiece hidePiece;
        private PuzzlePiece[][] originalRows;
        private PuzzlePiece[] originalRowsByIndex;
        private Random random = new Random();

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

        public int AmountOfPiecesOutOfOrder()
        {
            var amountOfPiecesOutOfOrder = 0;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Rows[i][j].Number != originalRows[i][j].Number)
                    {
                        amountOfPiecesOutOfOrder++;
                    }
                }
            }

            return amountOfPiecesOutOfOrder;
        }

        public int MovementsToFinishAllPieces()
        {
            var movementsToFinishAllPieces = 0;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Rows[i][j].Number != originalRows[i][j].Number)
                    {
                        movementsToFinishAllPieces += MovementsToFinish(Rows[i][j]);
                    }
                }
            }

            return movementsToFinishAllPieces;
        }

        public bool IsDone()
        {
            var isDone = true;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Rows[i][j].Number != originalRows[i][j].Number)
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
            var nextPosition = GetNextPositionByMovement(movement);

            if (!IsPositionValid(nextPosition))
            {
                throw new InvalidOperationException(INVALID_MOVEMENT_MESSAGE);
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
                    originalRowsByIndex[piece.Number] = piece;
                }

                rows[i] = column;
            }

            Rows = rows;
            originalRows = (PuzzlePiece[][])rows.Clone();
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
            var nextPosition = GetNextPositionByMovement(movement);

            return IsPositionValid(nextPosition);
        }

        private bool IsPositionValid(PiecePosition position)
        {
            return (
                position.Row < Size
                && position.Column >= 0
                && position.Column < Size
                && position.Row >= 0
            );
        }
        public int MovementsToFinish(PuzzlePiece piece)
        {
            var originalPiece = originalRowsByIndex[piece.Number];
            var movementToFinish = Math.Abs(
                (originalPiece.Position.Column - piece.Position.Column) +
                (originalPiece.Position.Row - piece.Position.Row)
            );

            return movementToFinish;
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
