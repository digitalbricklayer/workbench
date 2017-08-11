using System.Threading;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class GridViewModelTests
    {
        [Test]
        public void GetRowDataFromGridReturnsExpectedValue()
        {
            var theGridModel = new GridModel();
            var sut = new GridViewModel(theGridModel);
            sut.AddColumn(new GridColumnModel("X"));
            sut.AddColumn(new GridColumnModel("Y"));
            sut.AddColumn(new GridColumnModel("Z"));
            sut.AddRow(new GridRowModel("1", "2", "3"));
            sut.AddRow(new GridRowModel("4", "5", "6"));
            sut.AddRow(new GridRowModel("7", "8", "9"));

            var actualRow = sut.GetRowAt(1);
            Assert.That(actualRow.Cells[2].Text, Is.EqualTo("6"));
        }
    }
}
