using System;
using Xunit;

namespace NextMove.Lib.Tests
{
    public class SbdAdressInfo_GetForrettningsmeldingShould
    {
        [Fact]
        public void ReturnForettningsmeldingString()
        {
            string forettningsmeldingType = "arkivmelding";
            SbdAddressInfo sbdAddressInfo = new SbdAddressInfo(1234,
                1234, "", "urn:no:difi:arkivmelding:xsd::arkivmelding");
            var result = sbdAddressInfo.ForettningsmeldingType;

            Assert.True(result==forettningsmeldingType);
        }
            
    }

    public class SbdAddressInfo_CtorShould
    {
        private int validOrgNr = 910076787;
        private string validProcessId = "urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0";
        private string validDocumentId = "urn:no:difi:arkivmelding:xsd::arkivmelding";

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1234567890)]
        [InlineData(12345678)]
        public void ThrowExceptoinOnInvlidSendeOrgnr(int sender)
        {
            var ex = Assert.Throws<ArgumentException>(()=>   new SbdAddressInfo(sender, validOrgNr, validProcessId, validDocumentId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(123456789012)]
        [InlineData(12345678)]
        public void ThrowExceptoinOnInvlidReceiverOrgnr(long receiver)
        {
            var ex = Assert.Throws<ArgumentException>(()=>   new SbdAddressInfo(validOrgNr, receiver, validProcessId, validDocumentId));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void ThrowExceptoinOnInvlidProcessId(string processId)
        {
            var ex = Assert.Throws<ArgumentException>(()=>   new SbdAddressInfo(validOrgNr, validOrgNr, processId, validDocumentId));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("urn:no:difi:profile:arkivmelding:")]
        public void ThrowExceptoinOnInvlidDocumentID(string documentId)
        {
            var ex = Assert.Throws<ArgumentException>(()=>   new SbdAddressInfo(validOrgNr, validOrgNr, validProcessId, documentId));
        }
    }
}

      
