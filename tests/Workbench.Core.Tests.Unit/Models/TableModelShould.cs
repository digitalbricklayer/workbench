using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class TableModelShould
    {
        [Test]
        public void AddTestColumnGetColumnCellsReturnsCells()
        {
            var sut = TableModel.Default;
            sut.AddColumnAfter(0, new TableColumnModel("Test"));
            var testColumnData = sut.GetColumnDataByName("Test");
            var columnCells = testColumnData.GetCells();
            var expectedColumnCellCount = sut.GetRows().Count;
            Assert.That(columnCells.Count, Is.EqualTo(expectedColumnCellCount));
        }

        [Test]
        public void AddTestRowGetColumnCellsReturnsCells()
        {
            var sut = TableModel.Default;
            sut.AddColumnAfter(0, new TableColumnModel("Test"));
            sut.AddRowAfter(0, new TableRowModel());
            var testRow = sut.GetRowAt(1);
            var a = sut.GetCellBy(2, 1);
            a.Text = "a";
            var b = sut.GetCellBy(2, 2);
            b.Text = "b";
            var c = sut.GetCellBy(2, 3);
            c.Text = "c";
            var actualCellContent = testRow.GetCellContent();
            var expectedCellContent = new[] {"a", "b", "c"};
            CollectionAssert.AreEqual(expectedCellContent, actualCellContent);
        }
    }
}
