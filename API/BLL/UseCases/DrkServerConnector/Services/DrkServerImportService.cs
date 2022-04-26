using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using API.BLL.Base;
using API.BLL.Helper;
using API.BLL.UseCases.DrkServerConnector.Entities;
using API.BLL.UseCases.DrkServerServiceLogDescriptions.Daos;
using API.BLL.UseCases.DrkServerServiceLogDescriptions.Entities;
using API.BLL.UseCases.DrkServerServiceLogTypes.Daos;
using API.BLL.UseCases.DrkServerServiceLogTypes.Entities;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace API.BLL.UseCases.DrkServerConnector.Services

{
    public interface IDrkServerImportService
    {
        Task<RequestResult> Import(Context context, ImportRequest importRequest);
    }

    public class DrkServerImportService : IDrkServerImportService
    {
        private readonly AppSettings appSettings;
        private readonly IServiceLogDescriptionDao descriptionDao;
        private readonly IServiceLogTypeDao typeDao;

        public DrkServerImportService(
            IServiceLogDescriptionDao descriptionDao,
            IServiceLogTypeDao typeDao,
            IOptions<AppSettings> appSettings)
        {
            this.descriptionDao = descriptionDao;
            this.typeDao = typeDao;
            this.appSettings = appSettings.Value;
        }

        public async Task<RequestResult> Import(Context context, ImportRequest importRequest)
        {
            if (!context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                    .Contains(Rights.AdministrationImport))
                return new RequestResult()
                {
                    PermissionFailure = new PermissionFailure()
                    {
                        FailureMessage = PermissionFailureMessage.MissingPermission,
                        UnderlyingRight = Rights.DutyHoursEditBooking
                    },
                    StatusCode = StatusCode.PermissionFailure
                };

            try
            {
                var connector = new ServerConnector(
                    appSettings.DrkServerIssuer,
                    appSettings.DrkServerClientId,
                    appSettings.DrkServerClientSecret,
                    importRequest.Credentials.DrkServerLogin,
                    importRequest.Credentials.DrkServerPassword
                );

                var serviceLogTypes = new EntityOrError<List<ServiceLogType>>();
                if (importRequest.ImportServiceLogTypes == true)
                {
                    serviceLogTypes = await GetServiceLogTypes(connector);
                    if (serviceLogTypes.HasError())
                        return HandleError(serviceLogTypes.Exception);
                }

                var serviceLogDescriptions = new EntityOrError<List<ServiceLogDescription>>();
                if (importRequest.ImportServiceLogDescriptions == true)
                {
                    serviceLogDescriptions = await GetServiceLogDescriptions(connector);
                    if (serviceLogDescriptions.HasError())
                        return HandleError(serviceLogDescriptions.Exception);
                }

                using var transactionScope = new TransactionScope();
                
                if (importRequest.ImportServiceLogTypes == true)
                {
                    typeDao.DeleteAll();
                    typeDao.CreateMany(serviceLogTypes.Value);
                }

                if (importRequest.ImportServiceLogDescriptions == true)
                {
                    descriptionDao.DeleteAll();
                    descriptionDao.CreateMany(serviceLogDescriptions.Value);
                }

                transactionScope.Complete();

                return new RequestResult()
                {
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return new RequestResult()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Exception = e
                };
            }
        }

        private RequestResult HandleError(Exception e)
        {
            if (e is UnauthorizedAccessException)
                return new RequestResult()
                {
                    StatusCode = StatusCode.ValidationError,
                    ValidationFailures = new List<ValidationFailure>()
                    {
                        new ValidationFailure("DrkServerPassword", "validation.error.userNameOrPasswordInvalid")
                    }
                };

            return new RequestResult()
            {
                StatusCode = StatusCode.InternalServerError,
                Exception = e
            };
        }

        private Task<EntityOrError<List<ServiceLogType>>> GetServiceLogTypes(ServerConnector connector) =>
            connector.GetFromServer<List<ServiceLogType>>(
                $"{appSettings.DrkServerApiEndpoint}/code-entries",
                "[\"VALUELIST_SERVICELOG_TYP\"]");

        private Task<EntityOrError<List<ServiceLogDescription>>> GetServiceLogDescriptions(ServerConnector connector) =>
            connector.GetFromServer<List<ServiceLogDescription>>(
                $"{appSettings.DrkServerApiEndpoint}/code-entries",
                "[\"VALUELIST_SERVICELOG_DESCRIPTION\"]");
    }
}