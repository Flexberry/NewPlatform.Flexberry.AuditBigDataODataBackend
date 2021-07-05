namespace NewPlatform.Flexberry.AuditBigData
{
    using System;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using ICSSoft.Services;
    using IIS.Caseberry.Logging.Objects;

    using NewPlatform.Flexberry;
    using NewPlatform.Flexberry.AspNet.WebApi.Cors;
    using NewPlatform.Flexberry.ORM.ODataService.Extensions;
    using NewPlatform.Flexberry.ORM.ODataService.Files;
    using NewPlatform.Flexberry.ORM.ODataService.Model;
    using NewPlatform.Flexberry.ORM.ODataService.WebApi.Extensions;
    using NewPlatform.Flexberry.Services;

    using Unity;
    using Unity.AspNet.WebApi;

    /// <summary>
    /// Configure OData Service.
    /// </summary>
    internal static class ODataConfig
    {
        /// <summary>
        /// Configure OData by DataObjects assembly.
        /// </summary>
        /// <param name="config">Http configuration object.</param>
        /// <param name="container">Unity container.</param>
        public static void Configure(HttpConfiguration config, IUnityContainer container, HttpServer httpServer)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            // To support CORS.
            config.EnableCors(new DynamicCorsPolicyProvider(true));

            // Use constructor with true first parameter for enable SupportsCredentials.

            // Use Unity as WebAPI dependency resolver
            config.DependencyResolver = new UnityDependencyResolver(container);

            // Config file upload.
            const string fileControllerPath = "api/File";
            config.MapODataServiceFileRoute("File", fileControllerPath);
            var fileAccessor = new DefaultDataObjectFileAccessor(new Uri("http://localhost"), fileControllerPath, "Uploads");
             container.RegisterInstance<IDataObjectFileAccessor>(fileAccessor);

            // Create EDM model builder
            var assemblies = new[]
            {  
                Assembly.Load("NewPlatform.Flexberry.AuditBigData.Objects"),
                typeof(ApplicationLog).Assembly,
                typeof(UserSetting).Assembly,
                typeof(FlexberryUserSetting).Assembly,
                typeof(Lock).Assembly,
            };

            var builder = new DefaultDataObjectEdmModelBuilder(assemblies);

            // Map OData Service
            var token = config.MapDataObjectRoute(builder, httpServer);
        }
    }
}