using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Test using the 8 Queens problem on a 8x8 chessboard.
    /// </summary>
    [TestFixture]
    public class EightQueensSolverShould
    {
        private const int ExpectedQueens = 8;

        [Test]
        public void SolveWithEightQueensModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithEightQueensModelSolutionContainsEightQueens()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var actualColumnValues = actualResult.Snapshot.GetCompoundLabelByVariableName("cols").Values;
            Assert.That(actualColumnValues, Is.Unique);
            Assert.That(actualColumnValues, Is.All.GreaterThanOrEqualTo(1)
                                                  .And
                                                  .LessThanOrEqualTo(ExpectedQueens));
        }

        [Test]
        public void SolveWithAttachedChessboardVisualizerAssignsEightQueens()
        {
            var sut = CreateWorkspace();
            sut.Solve();
            var chessboardVisualizer = (ChessboardTabModel)sut.GetVisualizerBy("board");
            var allQueenSquares = chessboardVisualizer.GetSquaresOccupiedBy(PieceType.Queen);
            Assert.That(allQueenSquares, Has.Count.EqualTo(ExpectedQueens));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = WorkspaceModel.Create($"{ExpectedQueens} Queens Model")
                                          .AddAggregate("cols", ExpectedQueens, "1..size(cols)")
                                          .WithConstraintAllDifferent("cols")
                                          .WithConstraintExpression("$cols[i] <> $cols[j] | i,j in size(cols),i")
                                          .WithConstraintExpression("$cols[i] + i <> $cols[j] + j | i,j in size(cols),i")
                                          .WithConstraintExpression("$cols[i] - i <> $cols[j] - j | i,j in size(cols),i")
                                          .WithVisualizerBinding("for x,y in 1..size(cols),1..size(cols): if <cols,x> = %y: board(x:x,y:y,side:white,piece:Queen)")
                                          .WithChessboard("board")
                                          .Build();

            return workspace;
        }
    }
}
