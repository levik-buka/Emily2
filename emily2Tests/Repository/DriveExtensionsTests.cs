using Microsoft.VisualStudio.TestTools.UnitTesting;
using emily2.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emily2.Family;

namespace emily2.Repository.Tests
{
    [TestClass()]
    public class DriveExtensionsTests
    {
        [TestMethod()]
        public void GetFamilyMemberJsonFilenameTest1()
        {
            var dirInfo = new DirectoryInfo("C:\\FOLDER\\sub_FOLDER");
            Assert.AreEqual("C:\\FOLDER\\sub_FOLDER\\sub_FOLDER.json", dirInfo.GetFamilyMemberConfigFilename());
        }

        [TestMethod()]
        public void GetFamilyMemberJsonFilenameTest2()
        {
            var dirInfo = new DirectoryInfo("C:\\FOLDER\\sub_FOLDER\\");
            Assert.AreEqual("C:\\FOLDER\\sub_FOLDER\\sub_FOLDER.json", dirInfo.GetFamilyMemberConfigFilename());
        }

        [TestMethod()]
        public void GetFamilyMemberJsonFilenameTestWithNoData()
        {
            try
            {
                var member = new FamilyMember();
                member.GetFamilyMemberPathToConfigFile("C:\\FOLDER\\sub_FOLDER");
                Assert.Fail("Empty family member should throw an exception");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual("Can not get family member's directory name because of missing name ()", e.Message);
            }
        }

        [TestMethod()]
        public void GetFamilyMemberJsonFilenameTestWithFirstNameOnly()
        {
            var member = new FamilyMember()
            {
                FirstName = "first"
            };
            Assert.AreEqual("C:\\FOLDER\\sub_FOLDER\\first\\first.json", member.GetFamilyMemberPathToConfigFile("C:\\FOLDER\\sub_FOLDER"));
        }

        [TestMethod()]
        public void GetFamilyMemberJsonFilenameTestWithLastNameOnly()
        {
            var member = new FamilyMember()
            {
                LastName = "last"
            };
            Assert.AreEqual("C:\\FOLDER\\sub_FOLDER\\last\\last.json", member.GetFamilyMemberPathToConfigFile("C:\\FOLDER\\sub_FOLDER"));
        }

        [TestMethod()]
        public void GetFamilyMemberJsonFilenameTestWithIndexOnly()
        {
            var member = new FamilyMember()
            {
                Index = 11
            };
            Assert.AreEqual("C:\\FOLDER\\sub_FOLDER\\11\\11.json", member.GetFamilyMemberPathToConfigFile("C:\\FOLDER\\sub_FOLDER"));
        }

        [TestMethod()]
        public void GetFamilyMemberJsonFilenameTestWithoutIndex()
        {
            var member = new FamilyMember()
            {
                FirstName = "first",
                LastName = "last"
            };
            Assert.AreEqual("C:\\FOLDER\\sub_FOLDER\\last first\\last first.json", member.GetFamilyMemberPathToConfigFile("C:\\FOLDER\\sub_FOLDER\\"));
        }
    }
}