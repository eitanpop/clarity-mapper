using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ClarityMapper.DomainObjects.FHIR;
using ClarityMapper.V2.Converters;
using HL7.Dotnetcore;

namespace ClarityMapper.V2.Tests
{
    public static class TestUtility
    {
        public static string GetHL7FakeData(string fileName)
        {
            var path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(TestUtility)).Location) + "/";
            return File.ReadAllText(path + "MockHL7Data/" + fileName);
        }

        public static Message GetMessageFromHL7V2(string hl7) 
        {
            var message = new Message(hl7);
            message.ParseMessage();
            return message;
        }

    }
}
