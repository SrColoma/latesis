using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EmitterTest
{
    private GameObject mediatorObject;
    private GameObject emitterObject;
    private MediatorController mediatorController;
    private Emitter emitter;
    private bool eventReceived;
    private object[] receivedArguments;

    [SetUp]
    public void SetUp()
    {
        // Creamos un objeto para MediatorController si no existe
        mediatorObject = new GameObject("MediatorController");
        mediatorController = mediatorObject.AddComponent<MediatorController>();

        // Creamos un objeto para Emitter
        emitterObject = new GameObject("Emitter");
        emitter = emitterObject.AddComponent<Emitter>();

        // Aseguramos que MediatorController sea la instancia Singleton
        if (MediatorController.Instance == null)
        {
            mediatorController = mediatorObject.GetComponent<MediatorController>();
        }

        // Inicializamos las variables para los tests
        eventReceived = false;
        receivedArguments = null;
    }

    [TearDown]
    public void TearDown()
    {
        // Limpiamos los objetos creados después de cada test
        UnityEngine.Object.DestroyImmediate(mediatorObject);
        UnityEngine.Object.DestroyImmediate(emitterObject);
    }

    [Test]
    public void RegisterEvent_ShouldRegisterEventSuccessfully()
    {
        string eventID = "TestEvent";
        mediatorController.RegisterEvent(eventID);
        LogAssert.NoUnexpectedReceived();
    }

    [Test]
    public void EmitEvent_WithoutListeners_ShouldNotInvokeAnyListeners()
    {
        string eventID = "TestEventNoListener";
        mediatorController.RegisterEvent(eventID);
        mediatorController.EmitEvent(eventID, new object[] { "No listener data" });
        LogAssert.NoUnexpectedReceived();
    }

    [Test]
    public void EmitEvent_WithListeners_ShouldInvokeListener()
    {
        string eventID = "TestEventWithListener";
        mediatorController.RegisterEvent(eventID);

        // Suscribimos un listener al evento
        mediatorController.SubscribeEvent(eventID, args =>
        {
            eventReceived = true;
            receivedArguments = args;
        });

        object[] testArgs = { 42, "Argumento", 3.14f };
        mediatorController.EmitEvent(eventID, testArgs);

        // Verificamos que el listener fue invocado correctamente
        Assert.IsTrue(eventReceived, "El evento no fue recibido por el listener");
        CollectionAssert.AreEqual(testArgs, receivedArguments, "Los argumentos recibidos no coinciden");
    }

    [Test]
    public void Emitter_ShouldEmitEventThroughMediator()
    {
        // Configuramos el identificador y los argumentos del evento en el Emitter
        string eventID = "EventFromEmitter";
        object[] testArgs = { "Mensaje", 123, true };

        // Asignamos los valores al Emitter
        emitterObject.GetComponent<Emitter>().SendMessage("setEventID", eventID);
        emitterObject.GetComponent<Emitter>().SendMessage("setArguments", testArgs);

        mediatorController.RegisterEvent(eventID);
        
        mediatorController.SubscribeEvent(eventID, args =>
        {
            eventReceived = true;
            receivedArguments = args;
        });

        emitter.Emit();

        // Verificamos que el listener del receptor sea invocado
        Assert.IsTrue(eventReceived);
        CollectionAssert.AreEqual(testArgs, receivedArguments, "El receptor no se activó o no recibió argumentos correctamente");
    }
}
