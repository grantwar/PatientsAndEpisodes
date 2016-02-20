using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace RestApi
{
    using Interfaces;
    using Models;

    public static class UnityConfig
    {
        public static IUnityContainer RegisterComponents(InMemoryPatientContext testPatientContext = null)
        {
            var container = new UnityContainer();

            if (testPatientContext == null)
            {
                container.RegisterType<IDatabaseContext, PatientContext>();
            }
            else
            {
                container.RegisterInstance(typeof(InMemoryPatientContext), "PatientContext", testPatientContext);
            }

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            return container;
        }
    }
}