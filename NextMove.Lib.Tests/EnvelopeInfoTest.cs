using System;
using Xunit;

namespace NextMove.Lib.Tests
{
    public class SbdAdressInfo_GetForrettningsmeldingShould
    {
        private string validOrgNr = "910076787";
        private string validProcessId = "urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0";
        private string validDocumentId = "urn:no:difi:arkivmelding:xsd::arkivmelding";

        [Fact]
        public void ReturnForettningsmeldingString()
        {
            string forettningsmeldingType = "arkivmelding";
            EnvelopeInfo envelopeInfo = new EnvelopeInfo(validOrgNr,
                validOrgNr, validProcessId, validDocumentId);
            var result = envelopeInfo.ForettningsmeldingType;

            Assert.True(result==forettningsmeldingType);
        }
            
    }

    public class SbdAddressInfo_CtorShould
    {
        private static string validOrgNr = "910076787";
        private static string validProcessId = "urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0";
        private static string validDocumentId = "urn:no:difi:arkivmelding:xsd::arkivmelding";
        private static string icd = "9908";
        private static string validPersonnumber = "06068700602";

        [Fact]
        public void AcceptIcdPrefixedSenderOrgnr()
        {
            var sender = $"{icd}:{validOrgNr}";
            EnvelopeInfo envelopeInfo = new EnvelopeInfo(sender,
                validOrgNr, validProcessId, validDocumentId);
            var result = envelopeInfo.ForettningsmeldingType;

            Assert.NotNull(result);
        }

        [Fact]
        public void AcceptIcdPrefixedReceiverOrgnr()
        {
            var receiver = $"{icd}:{validOrgNr}";
            EnvelopeInfo envelopeInfo = new EnvelopeInfo(validOrgNr,
                receiver, validProcessId, validDocumentId);
            var result = envelopeInfo.ForettningsmeldingType;

            Assert.NotNull(result);
        }

        [Fact]
        public void ThrowExceptionOnIcdPrefixedPersonNumber()
        {
            var receiver = $"{icd}:{validPersonnumber}";
            var ex = Assert.Throws < ArgumentException >(() => new EnvelopeInfo(validOrgNr,
                receiver, validProcessId, validDocumentId));
        }

        [Theory]
        [InlineData("910076787")]
        [InlineData("06068700602")]
        public void AcceptOrgAndPersonnumberForReceiver(string reciver)
        {
            var receiver = $"{icd}:{validOrgNr}";
            EnvelopeInfo envelopeInfo = new EnvelopeInfo(validOrgNr,
                receiver, validProcessId, validDocumentId);
            var result = envelopeInfo.ForettningsmeldingType;

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("1234567890")]
        [InlineData("9908:1234567890")]
        [InlineData("12345678")]
        [InlineData("9908:12345678")]
        public void ThrowExceptoinOnInvlidSendeOrgnr(string sender)
        {
            var ex = Assert.Throws<ArgumentException>(()=>   new EnvelopeInfo(sender, validOrgNr, validProcessId, validDocumentId));
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("123456789012")]
        [InlineData("9908:123456789012")]
        [InlineData("12345678")]
        [InlineData("9908:12345678")]
        public void ThrowExceptoinOnInvlidReceiverOrgnr(string receiver)
        {
            var ex = Assert.Throws<ArgumentException>(()=>   new EnvelopeInfo(validOrgNr, receiver, validProcessId, validDocumentId));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void ThrowExceptoinOnInvlidProcessId(string processId)
        {
            var ex = Assert.Throws<ArgumentException>(()=>   new EnvelopeInfo(validOrgNr, validOrgNr, processId, validDocumentId));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("urn:no:difi:profile:arkivmelding:")]
        public void ThrowExceptoinOnInvlidDocumentID(string documentId)
        {
            var ex = Assert.Throws<ArgumentException>(()=>   new EnvelopeInfo(validOrgNr, validOrgNr, validProcessId, documentId));
        }
    }
}

      
