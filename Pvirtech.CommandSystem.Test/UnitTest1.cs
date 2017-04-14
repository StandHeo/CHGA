using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Pvirtech.Metro.Controls;
using Newtonsoft.Json;

namespace Pvirtech.CommandSystem.Test
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			MetroWindow d = new  MetroWindow();
			d.Show();
			 
		}

        [TestMethod]
        public void SerilizedTest()
        {
            var aa = new { p1="p1",p2 = "sxxxxxxxxx" ,p3="p3"};
            var jsonStr = JsonConvert.SerializeObject(aa);
            var aa1 = JsonConvert.DeserializeObject<MockClass>(jsonStr);

        }

        public class MockClass
        {
            public string P1 { get; set; }
            public string P2 { get; set; }
        }
	}
}
