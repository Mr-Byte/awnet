﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AW.Async.Tests
{
    [TestClass]
    public class AsyncInstanceTests
    {
        [TestMethod]
        public void TestLoginWithSuccess()
        {
            var instanceMock = new Mock<IInstance>();
            SetupDisposable(instanceMock);

            instanceMock.Setup(instance => instance.Login())
                .Returns(Result.Success)
                .Raises(instance => instance.CallbackLogin += null, instanceMock.Object, Result.Success)
                .Verifiable();

            using (var testInstance = instanceMock.Object)
            {
                var task = testInstance.LoginAsync();

                task.Wait();
                instanceMock.Verify();
                Assert.AreEqual(Result.Success, task.Result);
            }
        }

        [TestMethod]
        public void TestLoginWithFailure()
        {
            var instanceMock = new Mock<IInstance>();
            SetupDisposable(instanceMock);

            instanceMock.Setup(instance => instance.Login())
                .Returns(Result.Success)
                .Raises(instance => instance.CallbackLogin += null, instanceMock.Object, Result.InvalidPassword)
                .Verifiable();

            using (var testInstance = instanceMock.Object)
            {
                var task = testInstance.LoginAsync();

                task.Wait();
                instanceMock.Verify();
                Assert.AreEqual(Result.InvalidPassword, task.Result);
            }
        }

        [TestMethod]
        public void TestEnterWithSuccess()
        {
            var instanceMock = new Mock<IInstance>();
            SetupDisposable(instanceMock);

            instanceMock.Setup(instance => instance.Enter("aworld"))
                .Returns(Result.Success)
                .Raises(instance => instance.CallbackEnter += null, instanceMock.Object, Result.Success)
                .Verifiable();

            using (var testInstance = instanceMock.Object)
            {
                var task = testInstance.EnterAsync("aworld");

                task.Wait();
                instanceMock.Verify();
                Assert.AreEqual(Result.Success, task.Result);
            }
        }

        [TestMethod]
        public void TestEnterWithFailure()
        {
            var instanceMock = new Mock<IInstance>();
            SetupDisposable(instanceMock);

            instanceMock.Setup(instance => instance.Enter("aworld"))
                .Returns(Result.Success)
                .Raises(instance => instance.CallbackEnter += null, instanceMock.Object, Result.NoSuchWorld)
                .Verifiable();

            using (var testInstance = instanceMock.Object)
            {
                var task = testInstance.EnterAsync("aworld");
                task.Wait();

                instanceMock.Verify();
                Assert.AreEqual(Result.NoSuchWorld, task.Result);
            }
        }

        #region Helper methods

        private static void SetupDisposable(Mock<IInstance> instanceMock)
        {
            instanceMock.Setup(instance => instance.Dispose())
                .Raises(instance => instance.Disposing += null, instanceMock.Object);
        }

        #endregion
    }
}
