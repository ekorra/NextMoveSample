using System;
using System.Windows.Media;
using NextMoveSample.Wpf.ViewModels;
using Xunit;

namespace NextMove.Wpf.Tests
{
    public class ProcessViewModel_Shold
    {
        [Fact]
        public void RerturnCorrectNameForProcess()
        {
            const string DpoId = "urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0";
            var processViewModell = new ProcessViewModel(DpoId);

            Assert.Equal("Arkivmelding - PlanByggOgGeodata", processViewModell.Name);
        }

        [Fact]
        public void ReturnCorrectTypeForDpoProcess()
        {
            const string DpoId = "urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0";
            var processViewModell = new ProcessViewModel(DpoId);

            Assert.Equal(ProcessType.DPO, processViewModell.ProcessType);
        }

        [Fact]
        public void ReturnCorrectTypeForDpaProcess()
        {
            const string DpoId = "urn:no:difi:profile:avtalt:avtalt:ver1.0";
            var processViewModell = new ProcessViewModel(DpoId);

            Assert.Equal(ProcessType.DPA, processViewModell.ProcessType);
        }

        [Fact]
        public void ReturnCorrectTypeForDpiVedtakProcess()
        {
            const string DpoId = "urn:no:difi:profile:digitalpost:vedtak:ver1.0";
            var processViewModell = new ProcessViewModel(DpoId);

            Assert.Equal(ProcessType.DPI_VEDTAK, processViewModell.ProcessType);
        }

        [Fact]
        public void ReturnCorrectTypeForDpiInfoProcess()
        {
            const string DpoId = "urn:no:difi:profile:digitalpost:info:ver1.0";
            var processViewModell = new ProcessViewModel(DpoId);

            Assert.Equal(ProcessType.DPI_INFO, processViewModell.ProcessType);
        }

        [Fact]
        public void SettCorrectTypeAndNameWhenIdPropertyIsSet()
        {
            
            const string initId = "urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0";
            const string NewId = "urn:no:difi:profile:arkivmelding:naturOgMiljoe:ver1.0";
            var processViewModell = new ProcessViewModel(initId);
            processViewModell.Id = NewId;

            Assert.Equal(ProcessType.DPO, processViewModell.ProcessType);
            Assert.Equal("Arkivmelding - NaturOgMiljoe", processViewModell.Name);
        }
    }
}
