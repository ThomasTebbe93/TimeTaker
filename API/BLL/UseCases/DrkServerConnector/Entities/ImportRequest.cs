namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class ImportRequest
    {
        public bool? ImportServiceLogTypes { get; set; }
        public bool? ImportServiceLogDescriptions { get; set; }
        public ImportRequestCredentials Credentials { get; set; }

        public ImportRequest()
        {
        }
    }

    public class ImportRequestCredentials
    {
        public string DrkServerLogin { get; set; }
        public string DrkServerPassword { get; set; }

        public ImportRequestCredentials()
        {
        }
    }
}