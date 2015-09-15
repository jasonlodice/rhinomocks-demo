using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace RhinoMocksDemo
{
    /// <summary>
    /// This test fixture shows the capabilities of 
    /// Rhino Mocks
    /// </summary>
    [TestClass]
    public class AnimalTest
    {
        public interface IAnimal
        {
            string Name { get; set; }
            string Speak();
            string ObeyCommand(string command);
            event EventHandler ReadyToEat;
        }

        /// <summary>
        /// Properties are automatically implemented
        /// </summary>
        [TestMethod]
        public void Properties_Are_Automatically_Implemented()
        {
            //  Stubs return the canned responses that you set up
            var animal = MockRepository.GenerateStub<IAnimal>();
            
            animal.Name = "Buddy";
            
            Assert.AreEqual("Buddy", animal.Name);
        }

        /// <summary>
        /// Before you stub a function, it will return null by default
        /// </summary>
        [TestMethod]
        public void Functions_Will_Return_Null_By_Default()
        {
            var animal = MockRepository.GenerateStub<IAnimal>();
            
            var response = animal.Speak();
            
            Assert.IsNull(response);
        }

        /// <summary>
        /// Functions can be stubbed with a return value
        /// </summary>
        [TestMethod]
        public void Function_Return_Value_Can_Be_Setup()
        {
            var animal = MockRepository.GenerateStub<IAnimal>();
            
            //  for every call to Speak() return "Bark"
            animal.Stub(x => x.Speak()).Return("Bark");
            
            Assert.AreEqual("Bark", animal.Speak());
        }

        /// <summary>
        /// Rhino Mocks can be used to fire events
        /// </summary>
        [TestMethod]
        public void Stubs_Can_Raise_Events()
        {
            var isAnimalReadyToEat = false;

            var animal = MockRepository.GenerateStub<IAnimal>();
            
            //  when ReadyToEat fires, set flag true
            animal.ReadyToEat += (s, e) => isAnimalReadyToEat = true;

            //  tell Rhino Mocks to fire the event
            animal.Raise(x=>x.ReadyToEat += null, this,EventArgs.Empty);

            Assert.IsTrue(isAnimalReadyToEat);
        }

        /// <summary>
        /// A Function can be set up to throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Functions_Can_Be_Setup_To_Throw_Exception()
        {
            var animal = MockRepository.GenerateStub<IAnimal>();
            
            animal.Stub(x => x.Speak()).Throw(new DivideByZeroException());

            animal.Speak();
        }

        /// <summary>
        /// Functions can return different values for different arguments
        /// </summary>
        [TestMethod]
        public void Functions_Can_Be_Setup_To_Match_Different_Arguments()
        {
            var animal = MockRepository.GenerateStub<IAnimal>();
            
            animal.Stub(x => x.ObeyCommand(Arg.Is("Jump"))).Return("Jumping");
            
            animal.Stub(x => x.ObeyCommand(Arg.Is("Run"))).Return("Running");

            Assert.AreEqual("Jumping", animal.ObeyCommand("Jump"));

            Assert.AreEqual("Running", animal.ObeyCommand("Run"));
        }

        /// <summary>
        /// Rhino Mocks keeps track of calls to mocked objects 
        /// and allows you to make assertions
        /// </summary>
        [TestMethod]
        public void Rhino_Keeps_Track_Of_What_Was_Called_On_The_Stub()
        {
            var animal = MockRepository.GenerateStub<IAnimal>();

            animal.Speak();

            animal.AssertWasCalled(x=>x.Speak());

            animal.AssertWasNotCalled(x=>x.ObeyCommand(Arg<string>.Is.Anything));
        }               
    }
}
