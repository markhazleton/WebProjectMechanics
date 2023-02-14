using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebProjectMechanics.API.Controllers;

namespace WebProjectMechanics.API.Tests.Controllers
    {
    [TestClass]
    public class MineralControllerTests
        {
        [TestMethod]
        public void Get_StateUnderTest_ExpectedBehavior()
            {
            // Arrange
            var mineralController = new MineralController();
            string id = null;

            // Act
            var result = mineralController.Get(id);

            // Assert
            Assert.IsNotNull(result);
            }
        }
    }
