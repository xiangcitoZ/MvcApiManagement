using MvcApiManagement.Models;
using System.Net.Http.Headers;
using System.Web;

namespace MvcApiManagement.Services
{
    public class ServiceApiManagement
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiEmpleados;
        private string UrlApiDepartamentos;

        public ServiceApiManagement(IConfiguration configuration)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApiDepartamentos = configuration.GetValue<string>
                ("ApiUrls:ApiDepartamentos");
            this.UrlApiEmpleados =
                configuration.GetValue<string>("ApiUrls:ApiEmpleados");
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            using (HttpClient client = new HttpClient()) 
            {
                var queryString =
                    HttpUtility.ParseQueryString(string.Empty);
                string request =
                    "/api/empleados?" + queryString;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                client.DefaultRequestHeaders.CacheControl =
                    CacheControlHeaderValue.Parse("no-cache");
                
                HttpResponseMessage response = 
                    await client.GetAsync(this.UrlApiEmpleados + request);

                if(response.IsSuccessStatusCode) 
                {
                    List<Empleado> empleados = 
                        await response.Content.ReadAsAsync<List<Empleado>>(); 
                    return empleados;
                }else
                {
                    return null;
                }

            }
        }

        public async Task<List<Departamento>> 
            GetDepartamentosAsync(string suscripcion)
        {
            using (HttpClient client = new HttpClient())
            {
                var queryString =
                    HttpUtility.ParseQueryString(string.Empty);
                string request =
                    "/api/departamentos?" + queryString;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                client.DefaultRequestHeaders.CacheControl =
                    CacheControlHeaderValue.Parse("no-cache");

                client.DefaultRequestHeaders.Add
                    ("Ocp-Apim-Subscription-Key", suscripcion);

                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApiDepartamentos + request);

                if (response.IsSuccessStatusCode)
                {
                    List<Departamento> departamentos =
                        await response.Content.ReadAsAsync<List<Departamento>>();
                    return departamentos;
                }
                else
                {
                    return null;
                }

            }
        }


    }
}
