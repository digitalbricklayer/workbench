using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverNQueensTests
    {
        [Test]
        public void SolveWithNQueensModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithChessboardVisualizerDoesX()
        {
            var sut = CreateWorkspace();
            sut.Solve();
            var chessboardVisualizer = (ChessboardVisualizerModel) sut.Solution.GetVisualizerFor("board");
            Assert.That(chessboardVisualizer.Pieces, Is.Not.Empty);
        }

        private static WorkspaceModel CreateWorkspace()
        {
            return WorkspaceModel.Create("NQueens Model")
                                 .WithConstraint("board[1] > 0")
                                 .AddAggregate("board", 64, "0..1")
                                 .WithChessboardVisualizerBindingTo("board")
                                 .Build();
        }
    }
}
