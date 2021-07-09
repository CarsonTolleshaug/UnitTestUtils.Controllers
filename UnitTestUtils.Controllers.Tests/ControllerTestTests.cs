using NUnit.Framework;
using UnitTestUtils.Controllers.Example.Controllers;

namespace UnitTestUtils.Controllers.Tests
{
    public class ControllerTestTests
    {
        private ControllerTest<ExampleMvcController> _controllerTest;

        [SetUp]
        public void Setup()
        {
            _controllerTest = new ControllerTest<ExampleMvcController>();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}