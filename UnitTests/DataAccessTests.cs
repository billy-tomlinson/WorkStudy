using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkStudy.Model;
using WorkStudy.Services;

namespace UnitTests
{


    [TestClass]
    public class DataAccessTests
    {

        string connString = "/Users/billytomlinson/Library/Developer/CoreSimulator/Devices/C85D97E2-C3D8-409E-9E44-AB1EE920460A/data/Containers/Data/Application/2B73AE32-494A-43EB-8933-2595666A3B42/Documents/../Library/WorkStudy.db3";
        
        [TestMethod]
        public void AddAndRetrieveActivitySample()
        {
            var activityStudy = new ActivitySampleStudy()
            {
                Name = "Activity One",
                Date = DateTime.Now,
                StudyNumber = 1,
                IsRated = true,
                StudiedBy = "Billy",
                Department = "PaintShop"
            };

            var service = new DataAccessService(connString);
            var id = service.AddActivitySampleStudy(activityStudy);

            var sample = service.GetActivitySampleStudyById(id);

            Assert.AreEqual(id, sample.Id);
            Assert.AreEqual(activityStudy.Department, sample.Department);
        }
    }
}
