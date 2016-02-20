namespace RestApi.Test
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using Controllers;
    using Models;
    using NUnit.Framework;

    [TestFixture]
    public class PatientsControllerTests
    {
        private const int PatientId = 1;

        [Test]
        public void GetReturnsPatientWithOneEpisodeWhenPatientExists()
        {
            // Arrange
            var patientContext = new InMemoryPatientContext();
            var episode = new Episode { AdmissionDate = DateTime.UtcNow, Diagnosis = "Manflu", DischargeDate = DateTime.UtcNow.AddDays(1), PatientId = 1, EpisodeId = 1 };
            var patient = new Patient { DateOfBirth = DateTime.UtcNow, FirstName = "Grant", LastName = "Warren", NhsNumber = "123456789", PatientId = 1 };

            patientContext.Patients.Add(patient);
            patientContext.Episodes.Add(episode);
            var container = UnityConfig.RegisterComponents(patientContext);
            var patientsController = new PatientsController(container.Resolve(typeof(InMemoryPatientContext), "PatientContext") as InMemoryPatientContext);
           
            // Act
            var actualPatient = patientsController.Get(PatientId);

            // Assert
            Assert.IsNotNull(actualPatient);
            Assert.That(actualPatient, Is.EqualTo(patient));
            Assert.That(actualPatient.Episodes.Count(), Is.EqualTo(1));
            Assert.That(actualPatient.Episodes.First(), Is.EqualTo(episode));
        }

        [Test]
        public void GetReturnsNotFoundWhenPatientDoesNotExist()
        {
            // Arrange
            var patientContext = new InMemoryPatientContext();
            var container = UnityConfig.RegisterComponents(patientContext);
            var patientsController = new PatientsController(container.Resolve(typeof(InMemoryPatientContext), "PatientContext") as InMemoryPatientContext);

            // Act
            var statusCode = Assert.Throws<HttpResponseException>(() => patientsController.Get(PatientId)).Response.StatusCode;

            // Assert
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
