using System;
using System.Collections.Generic;
using System.Text;
using NextMoveSample.Wpf.ViewModels;
using Xunit;

namespace NextMove.Wpf.Tests
{
    public class ValidationVeiwModel_shold
    {

        [Fact]
        public void ReturnNameOfVeiwModelWithoutSubfix()
        {
            var viewModel = new ValidationViewModel("ExampleViewModel");
            Assert.Equal("Example", viewModel.Name);
        }

        [Fact]
        public void ReturnNameOfVeiwModel()
        {
            var viewModel = new MessageViewModel(null, null);
            Assert.Equal("MessageViewModel", viewModel.ViewModelName);
        }
    }
}
