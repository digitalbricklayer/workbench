using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Test using the 4 Queens problem on a 4x4 chessboard.
    /// </summary>
    [TestFixture]
    public class FourQueensTests
    {
        private const int ExpectedQueens = 4;

        [Test]
        public void SolveWithFourQueensModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithChessboardVisualizerAssignsFourQueens()
        {
            var sut = CreateWorkspace();
            sut.Solve();
            var chessboardVisualizer = (ChessboardVisualizerModel) sut.Solution.GetVisualizerFor("board");
            var allQueenSquares = chessboardVisualizer.GetSquaresOccupiedBy(PieceType.Queen);
            Assert.That(allQueenSquares, Has.Count.EqualTo(ExpectedQueens));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            return WorkspaceModel.Create("4 Queens Model")
                                 .WithConstraint("board[1] > 0")
                                 .AddAggregate("board", ExpectedQueens * ExpectedQueens, "0..1")
                                 .WithChessboardVisualizerBindingTo("board")
                                 .Build();
        }
    }
}
